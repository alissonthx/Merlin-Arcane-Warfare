using System;
using Unity.Netcode;
using UnityEngine;

public class VFXImpact : NetworkBehaviour
{
    public ParticleSystem ParticleSystem;


    private void OnEnable()
    {
        SetParticlePlayingState();
    }


    public override void OnNetworkSpawn()
    {
        // Authority starts the particle system (locally) when spawned
        // if (HasAuthority)
        // {
        SetParticlePlayingState(true);
        // }
        base.OnNetworkSpawn();
    }
   
    public void SetParticlePlayingState(bool isPlaying = false)
    {
        if (ParticleSystem)
        {
            if (isPlaying)
            {
                ParticleSystem.Play();
            }
            else
            {
                ParticleSystem.Stop();
            }
        }
    }

    // private NetworkObject impactNetworkObject;

    // private void Awake()
    // {
    //     impactNetworkObject = GetComponent<NetworkObject>();
    // }

    // private void OnEnable()
    // {
    //     if (!IsOwner)
    //     {
    //         return;
    //     }

    //     if (IsServer)
    //     {
    //         OnImpact();
    //     }
    //     else
    //     {
    //         OnImpactServerRpc();
    //     }
    // }

    // [ServerRpc]
    // private void OnImpactServerRpc()
    // {
    //     OnImpact();
    // }

    // private void OnImpact()
    // {
    //     impactNetworkObject.Spawn();
    // }
}
