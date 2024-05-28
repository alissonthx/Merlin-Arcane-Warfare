using Unity.Netcode;
using UnityEngine;

public class VFXImpact : NetworkBehaviour
{
    private NetworkObject impactNetworkObject;

    private void Awake()
    {
        impactNetworkObject = GetComponent<NetworkObject>();
    }

    private void OnEnable()
    {
        if (IsServer)
        {
            impactNetworkObject.Spawn();
        }
    }
}
