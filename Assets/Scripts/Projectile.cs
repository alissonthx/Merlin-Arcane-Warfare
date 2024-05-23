using Unity.Netcode;
using UnityEngine;

public class Projectile : NetworkBehaviour
{
    private void OnCollisionEnter(Collision col)
    {
        // Checks if the actual gameobject have interface to deal damage
        // IDamageable damageable = col.gameObject.GetComponent<IDamageable>();
        // if (damageable != null && col.gameObject.tag == "Player")
        // {
        //     float weaponDamage = 10f;
        //     damageable.Damage(weaponDamage);
        //     Destroy(gameObject);
        // }
        // else
        // {
        //     Destroy(gameObject);
        // }

        // Destroy(gameObject);        

        // if (IsServer && networkObject.IsSpawned)
        //     networkObject.Despawn();
    }
}

