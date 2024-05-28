using System;
using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class VFXProjectile : NetworkBehaviour
{
    [SerializeField] private GameObject vfxImpact;
    private NetworkObject networkObject;
    private NetworkObject impactNetworkObject;

    private void Awake()
    {
        networkObject = GetComponent<NetworkObject>();
        impactNetworkObject = vfxImpact.GetComponent<NetworkObject>();
    }

    private void OnCollisionEnter(Collision col)
    {
        Debug.Log("Collides with something");
        // Checks if the actual gameobject has interface to deal damage
        IDamageable damageable = col.gameObject.GetComponent<IDamageable>();
        if (damageable != null)
        {
            print("Deal Damage");
            float weaponDamage = 25f;
            damageable.Damage(weaponDamage);

            Destroy(gameObject);
            StartCoroutine(DeSpawnBullets(networkObject, 0.1f));

            // Instantiate impact on point of collision
            var impact = Instantiate(vfxImpact, col.contacts[0].point, Quaternion.identity) as GameObject;
           
            Destroy(impact, 1);
            StartCoroutine(DeSpawnBullets(impactNetworkObject, 1f));
        }
        else
        {
            Destroy(gameObject);
            StartCoroutine(DeSpawnBullets(networkObject, 0.1f));

            // Instantiate impact on point of collision
            var impact = Instantiate(vfxImpact, col.contacts[0].point, Quaternion.identity) as GameObject;
            
            Destroy(impact, 1);
            StartCoroutine(DeSpawnBullets(impactNetworkObject, 1f));
        }
    }

    private IEnumerator DeSpawnBullets(NetworkObject networkObjectToDespawn, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (IsServer && networkObjectToDespawn.IsSpawned)
            networkObjectToDespawn.Despawn();
    }
}

