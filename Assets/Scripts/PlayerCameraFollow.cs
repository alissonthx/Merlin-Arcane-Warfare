using UnityEngine;
using Cinemachine;
using System;
using Unity.Netcode;

public class PlayerCameraFollow : NetworkBehaviour
{
    [SerializeField] private CinemachineVirtualCamera virtualCamera;

    private void Awake()
    {
        virtualCamera = GameObject.FindWithTag("Camera").GetComponent<CinemachineVirtualCamera>();
    }

    private void OnEnable()
    {
        GameManager.OnGameInitialize += OnGameInitialize_SetCameraFollowTarget;
    }

    private void OnDisable()
    {
        GameManager.OnGameInitialize -= OnGameInitialize_SetCameraFollowTarget;
    }

    private void OnGameInitialize_SetCameraFollowTarget(object sender, EventArgs e)
    {
        if (virtualCamera != null && IsOwner)
        {
            print("change transform");
            virtualCamera.Follow = transform;
        }
    }

}
