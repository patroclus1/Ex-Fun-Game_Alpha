using UnityEngine;

public class EnemyBulletScript : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _bulletForce = 100f;
    [SerializeField] private float _lifetime = 4f;
    [SerializeField] private int _damage;
    [SerializeField] private GameObject _bulletHitFX;
                     private PlayerScript _player;
                     private Rigidbody _bulletRb;

    private void Awake()
    {
        _damage = Random.Range(5, 15);

        _bulletRb = GetComponent<Rigidbody>();
        Destroy(gameObject, _lifetime);
    }

    public void Initialize(PlayerScript player)
    {
        this._player = player;
    }

    private void FixedUpdate()
    {
        _bulletRb.velocity = transform.forward * _speed * _bulletForce * Time.fixedDeltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _player.TakeDamage(_damage);
            BulletHitFX();
            Destroy(gameObject);
        }
        else if (!other.CompareTag("Enemy"))
        {
            BulletHitFX();
            Destroy(gameObject);
        }
    }

    private void BulletHitFX()
    {
        var spawnedFX = Instantiate(_bulletHitFX, transform.position, transform.rotation);
        Destroy(spawnedFX.gameObject, 2);
    }

    private void OnTriggerExit(Collider other)
    {
        gameObject.tag = "Untagged";
    }
}
