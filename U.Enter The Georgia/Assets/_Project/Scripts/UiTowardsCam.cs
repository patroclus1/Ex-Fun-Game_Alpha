using UnityEngine;

public class UiTowardsCam : MonoBehaviour
{
    private Transform cam;

    void Awake()
    {
        cam = Camera.main.transform;
    }

    private void LateUpdate()
    {
        transform.LookAt(cam.position);
        transform.Rotate(0,180,0);
    }
}
