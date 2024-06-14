using Unity.Netcode;
using UnityEngine;

public class ShootProjectiles : NetworkBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireRate = 4f;
    private float timeToFire;
    // [SerializeField] private float projectileSpeed = 15f;
    // private Camera cam;
    private InputManager inputManager;
    public static ShootProjectiles Instance;

    private void Start()
    {
        Instance = this;
        inputManager = InputManager.Instance;
        // cam = Camera.main;
    }

    private void Update()
    {
        if (inputManager.PlayerShootedThisFrame() && IsOwner)
        {
            if (Time.time > timeToFire)
            {
                timeToFire = Time.time + 1 / fireRate;
                ShootProjectile();
            }
        }
    }

    private void ShootProjectile()
    {
        if (!IsOwner)
        {
            return;
        }

        if (IsServer)
        {
            print("is Server!");
            OnFireWeapon();
        }
        else
        {
            OnFireWeaponServerRpc();
        }
    }

    [ServerRpc]
    private void OnFireWeaponServerRpc()
    {
        OnFireWeapon();
    }


    private void OnFireWeapon()
    {
        var projectileGO = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        var instanceNetworkObject = projectileGO.GetComponent<NetworkObject>();
        instanceNetworkObject.Spawn();

        // Play impact sound at the collision point
        SFXManager.Instance.PlayRandomProjectileSFX(transform.position);
    }

    public Transform GetFirePoint()
    {
        if (IsOwner)
            return firePoint;
        else return null;
    }
}
