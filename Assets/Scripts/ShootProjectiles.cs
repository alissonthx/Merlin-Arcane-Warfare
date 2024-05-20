using Unity.Netcode;
using UnityEngine;

public class ShootProjectiles : NetworkBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float projectileSpeed = 10f;
    private InputManager inputManager;
    private Camera cam;
    private float distanceOfRay = 1000f;
    private float fireRate = 4f;
    private float timeToFire;

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
        // Create a ray from the center of the screen
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, distanceOfRay))
        {
            // Instantiate projectile at the firePoint position
            GameObject projectileGO = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

            // Calculate direction towards hit point
            Vector3 direction = (hit.point - firePoint.position).normalized;

            // Set projectile velocity
            projectileGO.GetComponent<Rigidbody>().velocity = direction * projectileSpeed;

            // Spawn the projectile over the network
            if (IsServer)
            {
                // Directly call Spawn on the NetworkObject component
                projectileGO.GetComponent<NetworkObject>().Spawn();
            }
        }
        else
        {
            Debug.LogWarning("Raycast didn't hit anything. Not spawning projectile.");
        }
    }
}
