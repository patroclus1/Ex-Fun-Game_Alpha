using UnityEngine;

public class EnemyBulletScript : MonoBehaviour
{
    private PlayerScript player;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float bulletForce = 100f;
    [SerializeField] private float lifetime = 4f;
    [SerializeField] private int damage;
    [SerializeField] private GameObject bulletHitFX;
    private Rigidbody _bullet;
    private bool hasHit;

    private void Awake()
    {
        damage = Random.Range(5, 15);

        _bullet = GetComponent<Rigidbody>();
        Destroy(gameObject, lifetime);
    }

    public void Initialize(PlayerScript player)
    {
        this.player = player;
    }

    private void FixedUpdate()
    {
        _bullet.velocity = transform.forward * speed * bulletForce * Time.fixedDeltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player.TakeDamage(damage);
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
        var spawnedFX = Instantiate(bulletHitFX, transform.position, transform.rotation);
        Destroy(spawnedFX.gameObject, 2);
    }

    private void OnTriggerExit(Collider other)
    {
        gameObject.tag = "Untagged";
    }
}
