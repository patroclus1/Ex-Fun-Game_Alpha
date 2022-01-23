using UnityEngine;
using UnityEngine.InputSystem;

public class RotateScript : MonoBehaviour
{
    private PlayerInput playerInput;
    private InputAction mouse;
    private Camera cam;
    private float mouseRayDisatnce = 300f;

    void Awake()
    {
        cam = Camera.main;
        playerInput = gameObject.GetComponentInParent<PlayerInput>();
        mouse = playerInput.actions["Mouse"];
    }

    private void Update()
    {
        RotateTowardsMouse();
    }

    private void RotateTowardsMouse()
    {
        Ray ray = cam.ScreenPointToRay(mouse.ReadValue<Vector2>());
        if (Physics.Raycast(ray, out RaycastHit hitInfo, mouseRayDisatnce))
        {
            var target = hitInfo.point;
            target.y = transform.position.y;
            transform.LookAt(target);
        }
    }
}
