using UnityEngine;
using UnityEngine.InputSystem;

public class RotateScript : MonoBehaviour
{
    private PlayerScript playerScript;
    private PlayerInput playerInput;
    private InputAction mouse;
    private Camera cam;
    private float mouseRayDisatnce = 300f;

    void Awake()
    {
        playerScript = GetComponent<PlayerScript>();
        cam = Camera.main;
        playerInput = gameObject.GetComponentInParent<PlayerInput>();
        mouse = playerInput.actions["Mouse"];
    }

    private void Update()
    {
        if (playerScript.Dashing) return;

        RotateTowardsMouse();
    }

    private void RotateTowardsMouse()
    {
        Ray ray = cam.ScreenPointToRay(mouse.ReadValue<Vector2>());
        if (Physics.Raycast(ray, out RaycastHit hitInfo, mouseRayDisatnce))
        {
            var target = hitInfo.point - transform.position;
            target.y = transform.position.y;
            target.Normalize();
            transform.forward = target;
        }
    }
}
