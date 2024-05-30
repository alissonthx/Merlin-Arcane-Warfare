using UnityEngine;

public class AimIK : MonoBehaviour
{
    private Camera cam;
    [SerializeField] private float distanceForward = 10f;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        Vector3 worldPosition = ray.GetPoint(distanceForward);
        transform.position = worldPosition;
    }
}
