using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private Transform _transform;
    private void Awake()
    {
        _transform = transform;
    }

    public IEnumerator cameraShake(float duration, float magnitude)
    {
        Vector3 originalPos = _transform.localPosition;
        float elapsed = 0.0f;
        while (elapsed < duration)
        {
            float x = Random.Range(-1, 1f) * magnitude;
            float y = Random.Range(-1, 1f) * magnitude;

            float currentPosX = _transform.localPosition.x + x;
            float currentPosY = _transform.localPosition.y + y;

            _transform.localPosition = new Vector3(currentPosX, currentPosY, originalPos.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        _transform.localPosition = originalPos;
    }
}
