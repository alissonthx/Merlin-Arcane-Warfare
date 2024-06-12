using Unity.Netcode;
using UnityEngine;

public class AimIKDinamic : NetworkBehaviour
{
    private Camera cam;
    [SerializeField] private float distanceForward = 10f;

    private void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        if (IsOwner)
        {
            Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            Vector3 worldPosition = ray.GetPoint(distanceForward);
            transform.position = worldPosition;
        }
    }
}
