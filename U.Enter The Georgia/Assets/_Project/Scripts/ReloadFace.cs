using UnityEngine;

public class ReloadFace : MonoBehaviour
{
    [SerializeField] Camera cam;

    private void Update()
    {
        if (!cam.transform.hasChanged) { return; }

        transform.LookAt(cam.transform.position);
        transform.Rotate(0, 180, 0);
    }
}
