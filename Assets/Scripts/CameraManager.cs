using UnityEngine;
using Cinemachine;

public class CameraFollow : MonoBehaviour
{
    public GameObject playerPrefab;
    public CinemachineVirtualCamera virtualCamera;

    void Start()
    {
        if (playerPrefab != null && virtualCamera != null)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                virtualCamera.Follow = player.transform;
            }
            else
            {
                Debug.LogError("Player(Clone) object not found.");
            }
        }
        else
        {
            Debug.LogError("Player prefab or virtual camera reference is missing.");
        }
    }
}
