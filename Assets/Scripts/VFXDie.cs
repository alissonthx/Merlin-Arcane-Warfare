using Unity.Netcode;
using UnityEngine;

public class VFXDie : NetworkBehaviour
{
    private NetworkObject dieExplosionNetworkObject;

    private void Awake()
    {
        dieExplosionNetworkObject = GetComponent<NetworkObject>();
    }

    private void OnEnable()
    {
        if (IsServer)
        {
            dieExplosionNetworkObject.Spawn();
        }
    }
}
