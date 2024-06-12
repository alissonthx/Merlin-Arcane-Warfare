using Unity.Netcode;
using UnityEngine;

public class ShootProjectiles : NetworkBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireRate = 4f;
    [SerializeField] private float projectileSpeed = 15f;
    private InputManager inputManager;
    private float timeToFire;
    private Camera cam;

    private void Start()
    {
        inputManager = InputManager.Instance;
        cam = Camera.main;
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

        if (IsOwner)
        {
            // Create a ray from the center of the screen 
            Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            RaycastHit hit;
            Vector3 targetPoint;

            // Perform the raycast and check if it hits something
            if (Physics.Raycast(ray, out hit))
            {
                targetPoint = hit.point;
            }
            else
            {
                targetPoint = ray.GetPoint(1000f); // Arbitrary large distance            
            }

            Vector3 direction = (targetPoint - firePoint.position).normalized;
            projectileGO.GetComponent<Rigidbody>().velocity = direction * projectileSpeed;
        }

        Debug.DrawLine(transform.position, new Vector3(Screen.width / 2, Screen.height / 2, 0));
    }

    public Transform GetFirePoint()
    {
        return firePoint;
    }
}
