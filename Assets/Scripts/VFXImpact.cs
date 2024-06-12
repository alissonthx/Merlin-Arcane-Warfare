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
        SetParticlePlayingState(true);
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
}
