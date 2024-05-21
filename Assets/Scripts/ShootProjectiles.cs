using System;
using Unity.Netcode;
using UnityEngine;

public class ShootProjectiles : NetworkBehaviour
{
    public event EventHandler OnShooting;
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
        // Create a ray from the center of the screen towards a point far away
        Vector3 shootDirection = cam.transform.forward;
        Vector3 targetPoint = firePoint.position + shootDirection * 1000f; // Arbitrary large distance

        GameObject projectileGO = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

        // Calculate direction towards the target point in the sky and set velocity
        Vector3 direction = (targetPoint - firePoint.position).normalized;
        projectileGO.GetComponent<Rigidbody>().velocity = direction * projectileSpeed;

        // Spawn the projectile over the network
        if (IsServer)
        {
            // Directly call Spawn on the NetworkObject component
            projectileGO.GetComponent<NetworkObject>().Spawn();
            OnShooting?.Invoke(this, EventArgs.Empty);
        }
    }
}
