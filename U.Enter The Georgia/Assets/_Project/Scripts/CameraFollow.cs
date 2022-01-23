using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private PlayerScript player;
    private Vector3 cameraOffset;

    private void Awake()
    {
        cameraOffset = transform.position - player.transform.position;
    }

    private void LateUpdate()
    {
        if (!player.isAlive) { return; }

        transform.position = player.transform.position + cameraOffset;
    }
}
