using System;
using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(NetworkObject))]
public class PlayerController : NetworkBehaviour, IDamageable
{
    public static event EventHandler OnPlayerJoined;
    [SerializeField] private float playerSpeed = 5.5f;
    [SerializeField] private float rotateSpeed = 0.2f;
    [SerializeField] private float jumpHeight = 1.0f;
    [SerializeField] private float gravityValue = -19.81f;
    [SerializeField] private float health = 100f;
    [SerializeField] private float currentHealth;

    [SerializeField] private Vector2 defaultInitialPositionOnPlane = new Vector2(-4, 4);

    [SerializeField] private NetworkVariable<Vector3> networkPositionDirection = new NetworkVariable<Vector3>();

    [SerializeField] private NetworkVariable<Vector3> networkRotationDirection = new NetworkVariable<Vector3>();

    [SerializeField] private NetworkVariable<PlayerState> networkPlayerState = new NetworkVariable<PlayerState>();

    private CharacterController characterController;

    // Client caches positions
    private Vector3 oldInputPosition = Vector3.zero;
    private Vector3 oldInputRotation = Vector3.zero;
    private PlayerState oldPlayerState = PlayerState.Idle;

    private Animator animator;
    private Transform cameraTransform;
    private bool isGrounded;
    private Vector3 playerVelocity;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        cameraTransform = Camera.main.GetComponent<Transform>();
    }

    private void Start()
    {
        currentHealth = health;

        GameManager.Instance.StartRound();

        if (IsClient && IsOwner)
        {
            OnPlayerJoined?.Invoke(this, EventArgs.Empty);

            transform.position = new Vector3(UnityEngine.Random.Range(defaultInitialPositionOnPlane.x, defaultInitialPositionOnPlane.y), 0,
                   UnityEngine.Random.Range(defaultInitialPositionOnPlane.x, defaultInitialPositionOnPlane.y));
        }
    }

    private void Update()
    {
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

        // if (InputManager.Instance.PlayerShootedThisFrame())
        // {
        //     UpdatePlayerStateServerRpc(PlayerState.Attack);
        // }

        playerVelocity.y += gravityValue * Time.deltaTime;
        characterController.Move(playerVelocity * Time.deltaTime);

        // Change animation states
        if (InputManager.Instance.GetPlayerMovement() == Vector2.zero && !InputManager.Instance.PlayerJumpedThisFrame())
            UpdatePlayerStateServerRpc(PlayerState.Idle);
        if (InputManager.Instance.GetPlayerMovement() != Vector2.zero && !InputManager.Instance.PlayerJumpedThisFrame())
            UpdatePlayerStateServerRpc(PlayerState.Walk);
        if (InputManager.Instance.GetPlayerMovement().y < 0 && !InputManager.Instance.PlayerJumpedThisFrame())
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
        if (currentHealth <= 0)
        {
            print("I'm fucking dying");
            gameObject.SetActive(false);
            if (IsSpawned)
                GetComponent<NetworkObject>().Despawn();
        }
    }
}