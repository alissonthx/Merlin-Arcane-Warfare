using System;
using System.Collections;
using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(NetworkObject))]
public class PlayerController : NetworkBehaviour, IDamageable
{
    public static event EventHandler OnPlayerJoined;
    [SerializeField] private float playerSpeed = 2.5f;
    [SerializeField] private float rotateSpeed = 0.2f;
    [SerializeField] private float jumpHeight = 1.0f;
    [SerializeField] private float gravityValue = -19.81f;
    [SerializeField] private float health = 100f;
    [SerializeField] private float currentHealth;

    [SerializeField] private Vector2 defaultInitialPositionOnPlane = new Vector2(-8, 8);

    [SerializeField] private NetworkVariable<Vector3> networkPositionDirection = new NetworkVariable<Vector3>();

    [SerializeField] private NetworkVariable<Vector3> networkRotationDirection = new NetworkVariable<Vector3>();

    [SerializeField] private NetworkVariable<PlayerState> networkPlayerState = new NetworkVariable<PlayerState>();
    [SerializeField] private GameObject[] hiddenPartBody;
    [SerializeField] private GameObject fullBody;
    [SerializeField] private GameObject vfxDie;

    private CharacterController characterController;
    // Client caches positions
    private Vector3 oldInputPosition = Vector3.zero;
    private Vector3 oldInputRotation = Vector3.zero;
    private PlayerState oldPlayerState = PlayerState.Idle;
    private Animator animator;
    private Transform cameraTransform;
    private NetworkObject vfxDieNetworkObject;
    private bool isGrounded;
    private Vector3 playerVelocity;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        cameraTransform = Camera.main.GetComponent<Transform>();
        vfxDieNetworkObject = vfxDie.GetComponent<NetworkObject>();
    }

    private void Start()
    {
        // Player temporarily disabled
        fullBody.SetActive(false);
        playerSpeed = 0f;

        StartCoroutine(Respawn(4f, 1f));

        if (IsClient && IsOwner)
        {
            OnPlayerJoined?.Invoke(this, EventArgs.Empty);

            // disable body parts in fps view
            foreach (GameObject bodyParts in hiddenPartBody)
            {
                bodyParts.SetActive(false);
            }
        }
    }

    private void Update()
    {
        if (currentHealth <= 0)
            Die();

        if (IsClient && IsOwner)
        {
            ClientInput();
        }

        isGrounded = characterController.isGrounded;
        if (isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = 0;
        }


        ClientMoveAndRotate();
        ClientVisuals();
    }

    private void ClientMoveAndRotate()
    {
        if (networkPositionDirection.Value != Vector3.zero)
        {
            characterController.Move(networkPositionDirection.Value * Time.deltaTime * playerSpeed);
        }
        if (networkRotationDirection.Value != Vector3.zero)
        {
            characterController.transform.Rotate(networkRotationDirection.Value * Time.deltaTime * rotateSpeed);
        }
    }

    private void ClientVisuals()
    {
        if (oldPlayerState != networkPlayerState.Value)
        {
            oldPlayerState = networkPlayerState.Value;
            animator.SetTrigger($"{networkPlayerState.Value}");
        }
    }

    private void ClientInput()
    {
        Vector3 inputPosition = new Vector3(InputManager.Instance.GetPlayerMovement().x, 0, InputManager.Instance.GetPlayerMovement().y);
        Vector3 inputRotation = new Vector3(0, InputManager.Instance.GetMouseDelta().x, 0);

        // moves camera and player together
        inputPosition = cameraTransform.forward * inputPosition.z + cameraTransform.right * inputPosition.x;
        inputPosition.y = 0f;
        characterController.Move(inputPosition * Time.deltaTime * playerSpeed);

        // Rotate the player to match the camera's rotation
        transform.rotation = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0);

        // Changes the height position of the player
        if (InputManager.Instance.PlayerJumpedThisFrame() && isGrounded)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
            UpdatePlayerStateServerRpc(PlayerState.Jump);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        characterController.Move(playerVelocity * Time.deltaTime);

        // Change animation states
        if (InputManager.Instance.PlayerShootedThisFrame())
            UpdatePlayerStateServerRpc(PlayerState.Attack);
        if (InputManager.Instance.GetPlayerMovement() == Vector2.zero)
            UpdatePlayerStateServerRpc(PlayerState.Idle);
        else if (InputManager.Instance.GetPlayerMovement() != Vector2.zero && !InputManager.Instance.PlayerJumpedThisFrame())
            UpdatePlayerStateServerRpc(PlayerState.Walk);
        else if (InputManager.Instance.GetPlayerMovement().y < 0 && !InputManager.Instance.PlayerJumpedThisFrame())
            UpdatePlayerStateServerRpc(PlayerState.WalkBack);

        // Let server know about position and rotation client changes
        if (oldInputPosition != inputPosition || oldInputRotation != inputRotation)
        {
            oldInputPosition = inputPosition;
            UpdateClientPositionAndRotationServerRpc(inputPosition, inputRotation);
        }
    }



    [ServerRpc]
    public void UpdateClientPositionAndRotationServerRpc(Vector3 newPosition, Vector3 newRotation)
    {
        networkPositionDirection.Value = newPosition;
        networkRotationDirection.Value = newRotation;
    }

    [ServerRpc]
    public void UpdatePlayerStateServerRpc(PlayerState state)
    {
        networkPlayerState.Value = state;
    }

    public void Damage(float damage)
    {
        currentHealth -= damage;
    }

    public void Die()
    {
        // Player temporarily disabled
        fullBody.SetActive(false);
        playerSpeed = 0f;

        // Spawn die VFX
        GameObject vfxDieGO = Instantiate(vfxDie, transform.position, Quaternion.identity);
        DeSpawnDieVFX(vfxDieGO, vfxDieNetworkObject, 1f);

        // Screen black and White
        if (IsOwner)
            PostProcessingEffects.Instance.BlackWhiteScreen();

        // Coroutine to respawn the player and turn on again controllers        
        StartCoroutine(Respawn(4f, 2f));
    }

    private IEnumerator DeSpawnDieVFX(GameObject vfxDieGO, NetworkObject networkObjectToDespawn, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(vfxDieGO);
        if (networkObjectToDespawn.IsSpawned)
            networkObjectToDespawn.Despawn();
    }

    private IEnumerator Respawn(float time, float respawnRange)
    {
        currentHealth = health;
        yield return new WaitForSeconds(time);
        fullBody.SetActive(true);
        playerSpeed = 2.5f;
        GameManager.Instance.StartRound();

        // Reset Screen effects
        PostProcessingEffects.Instance.ResetScreen();
        UIManager.Instance.StartRoundUI();

        //Random respawn position
        transform.position = new Vector3(UnityEngine.Random.Range(defaultInitialPositionOnPlane.x * respawnRange, defaultInitialPositionOnPlane.y * respawnRange), 0,
                   UnityEngine.Random.Range(defaultInitialPositionOnPlane.x * respawnRange, defaultInitialPositionOnPlane.y * respawnRange));
    }
}