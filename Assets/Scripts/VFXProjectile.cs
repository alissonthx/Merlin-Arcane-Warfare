using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class VFXProjectile : NetworkBehaviour
{
    [SerializeField] private GameObject vfxImpact;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float projectileSpeed = 15f;
    private GameObject collidedObject;
    private NetworkObject networkObject;
    private NetworkObject impactNetworkObject;
    private Camera cam;

    private void Awake()
    {
        networkObject = GetComponent<NetworkObject>();
        impactNetworkObject = vfxImpact.GetComponent<NetworkObject>();

        cam = Camera.main;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // projectile's position
        var explodePoint = transform.position;
        if (collision.contacts.Length > 0)
        {
            explodePoint = collision.contacts[0].point;
            collidedObject = collision.gameObject;
        }
        HandleExplosion(explodePoint);
    }


    private void HandleExplosion(Vector3 explodePoint)
    {
        // Instantiate and spawn the explosion effect
        var impact = Instantiate(vfxImpact);
        impact.GetComponent<NetworkObject>().Spawn();

        // position the explosion effect
        impact.transform.position = explodePoint;

        // Spawn the bullet if is not spawned yet
        if (!networkObject.IsSpawned)
            networkObject.Spawn();

        // Play impact sound at the collision point
        SFXManager.Instance.PlayRandomImpactSFX(explodePoint);

        // Checks if the actual gameobject has interface to deal damage
        IDamageable damageable = collidedObject.gameObject.GetComponent<IDamageable>();
        if (damageable != null)
        {
            float weaponDamage = 25f;
            damageable.Damage(weaponDamage);

            Destroy(gameObject);
            StartCoroutine(DeSpawnBullets(networkObject, 0.1f));

            Destroy(impact, 1f);
            StartCoroutine(DeSpawnBullets(impactNetworkObject, 0.5f));
        }
        else
        {
            Destroy(gameObject);
            StartCoroutine(DeSpawnBullets(networkObject, 0.1f));

            Destroy(impact, 1f);
            StartCoroutine(DeSpawnBullets(impactNetworkObject, 0.5f));
        }
    }

    // public override void OnNetworkDespawn()
    // {
    //     if (m_SpawnedExplosion)
    //     {
    //         m_SpawnedExplosion.SetParticlePlayingState(true);
    //     }
    //     base.OnNetworkDespawn();
    // }

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return;

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

        Vector3 direction = (targetPoint - ShootProjectiles.Instance.GetFirePoint().position).normalized;
        GetComponent<Rigidbody>().velocity = direction * projectileSpeed;
    }

    private IEnumerator DeSpawnBullets(NetworkObject networkObjectToDespawn, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (networkObjectToDespawn.IsSpawned)
            networkObjectToDespawn.Despawn();
    }
}

