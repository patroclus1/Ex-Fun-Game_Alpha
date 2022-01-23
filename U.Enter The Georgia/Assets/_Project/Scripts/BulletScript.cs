using UnityEngine;

public class BulletScript : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float bulletForce = 10f;
    [SerializeField] private float lifetime = 2f;
    private Rigidbody _bullet;
    private bool hasHit;

    private void Awake()
    {
        _bullet = GetComponent<Rigidbody>();
        Destroy(gameObject, lifetime);
    }

    private void FixedUpdate()
    {
        if (hasHit) { return; }

        _bullet.velocity = transform.forward * speed * bulletForce * Time.fixedDeltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        _bullet.velocity = Vector3.zero;
        _bullet.useGravity = true;
        hasHit = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        gameObject.tag = "Untagged";
    }
}
