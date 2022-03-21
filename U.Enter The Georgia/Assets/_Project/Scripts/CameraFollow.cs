using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private PlayerScript _player;
                     private Transform _transform;
                     private Vector3 _cameraOffset;

    private void Awake()
    {
        _transform = transform;
        _cameraOffset = transform.position - _player.transform.position;
    }

    private void LateUpdate()
    {
        if (!_player.IsPlayerAlive) { return; }

        _transform.position = _player.transform.position + _cameraOffset;
    }
}
