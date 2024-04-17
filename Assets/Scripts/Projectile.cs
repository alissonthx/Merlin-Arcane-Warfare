using UnityEngine;

public class Projectile : MonoBehaviour
{
    private void OnCollisionEnter(Collision col)
    {
        Destroy(gameObject);
    }
}
