using UnityEngine;
using Cinemachine;
using System;
using Unity.Netcode;

public class PlayerCameraFollow : NetworkBehaviour
{
    [SerializeField] private GameObject virtualCamera;

    private void OnEnable()
    {
        GameManager.OnGameInitialize += OnJoinCodeCreated_SetCameraFollowTarget;
    }

    private void OnDisable()
    {
        GameManager.OnGameInitialize -= OnJoinCodeCreated_SetCameraFollowTarget;
    }

    private void OnJoinCodeCreated_SetCameraFollowTarget(object sender, EventArgs e)
    {
        virtualCamera = GameObject.FindWithTag("Camera");

        if (IsOwner)
        {
            if (virtualCamera != null && this != null)
            {
                virtualCamera.GetComponent<CinemachineVirtualCamera>().Follow = transform;
            }
        }
    }

}
