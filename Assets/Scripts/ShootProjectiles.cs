using Unity.Netcode;
using UnityEngine;

public class ShootProjectiles : NetworkBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float projectileSpeed = 10f;
    private InputManager inputManager;
    private Camera cam;
    private float fireRate = 4f;
    private float timeToFire;

    private void Start()
    {
        inputManager = InputManager.Instance;
        cam = Camera.main;
    }

    private void FixedUpdate()
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

        GameObject projectileGO = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

        // Calculate direction towards the target point
        Vector3 direction = (targetPoint - firePoint.position).normalized;
        projectileGO.GetComponent<Rigidbody>().velocity = direction * projectileSpeed;

        // Spawn the projectile over the network
        if (IsServer)
        {
            // Directly call Spawn on the NetworkObject component
            projectileGO.GetComponent<NetworkObject>().Spawn();
        }
    }
}
