using UnityEngine;

public class ShootProjectiles : MonoBehaviour
{
    [SerializeField] private Transform projectile;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float projectileSpeed = 10f;
    private InputManager inputManager;
    private Camera cam;
    private Vector3 destination;
    private float distanceOfRay = 1000f;

    private void Start()
    {
        inputManager = InputManager.Instance;
        cam = Camera.main;
    }

    private void Update()
    {
        if (inputManager.PlayerShootedThisFrame())
        {
            ShootProjectile();
        }
    }

    private void ShootProjectile()
    {
        // creates a ray in the center of the screen
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
            destination = hit.point;
        else
            destination = ray.GetPoint(distanceOfRay);

        Transform projectileGO = Instantiate(projectile, firePoint.position, Quaternion.identity);
        projectileGO.GetComponent<Rigidbody>().velocity = (destination - firePoint.position).normalized * projectileSpeed;
    }
}
