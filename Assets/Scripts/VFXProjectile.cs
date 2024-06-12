using System;
using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class VFXProjectile : NetworkBehaviour
{
    [SerializeField] private GameObject vfxImpact;
    private GameObject collidedObject;
    private NetworkObject impactNetworkObject;

    private void Awake()
    {
        impactNetworkObject = GetComponent<NetworkObject>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!IsSpawned)
        {
            return;
        }

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
        var instance = Instantiate(vfxImpact);

        // position the explosion
        instance.transform.position = explodePoint;

        // Spawn the explosion
        // impactNetworkObject.Spawn();

        // Play impact sound at the collision point
        SFXManager.Instance.PlayRandomImpactSFX(explodePoint);

        // Checks if the actual gameobject has interface to deal damage
        IDamageable damageable = collidedObject.gameObject.GetComponent<IDamageable>();
        if (damageable != null)
        {
            float weaponDamage = 25f;
            damageable.Damage(weaponDamage);

            Destroy(gameObject);
            StartCoroutine(DeSpawnBullets(impactNetworkObject, 0.1f));

            Destroy(instance, 0.5f);
            StartCoroutine(DeSpawnBullets(impactNetworkObject, 1f));
        }
        else
        {
            Destroy(gameObject);
            StartCoroutine(DeSpawnBullets(impactNetworkObject, 0.1f));

            Destroy(instance, 0.5f);
            StartCoroutine(DeSpawnBullets(impactNetworkObject, 1f));
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

    private IEnumerator DeSpawnBullets(NetworkObject networkObjectToDespawn, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (networkObjectToDespawn.IsSpawned)
            networkObjectToDespawn.Despawn();
    }
}

