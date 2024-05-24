using Unity.Netcode;
using UnityEngine;

public class ShootProjectiles : NetworkBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float projectileSpeed = 15f;
    [SerializeField]private float fireRate = 4f;
    private InputManager inputManager;
    private Camera cam;
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
        Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;
        Vector3 targetPoint;
        // Instantiate the bullet
        GameObject projectileGO = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

        // Perform the raycast and check if it hits something
        if (Physics.Raycast(ray, out hit))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.GetPoint(1000f); // Arbitrary large distance            
        }

        // Calculate direction towards the target point
        Vector3 direction = (targetPoint - firePoint.position).normalized;
        projectileGO.GetComponent<Rigidbody>().velocity = direction * projectileSpeed;

        // Spawn the projectile over the network
        if (IsServer)
        {
            projectileGO.GetComponent<NetworkObject>().Spawn();
        }
    }
}
