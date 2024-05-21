using Unity.Netcode;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag != "Bullet" && col.gameObject.tag != "Player")
        {
            Destroy(gameObject);
            // gameObject.GetComponent<NetworkObject>().Despawn();
        }
    }
}
