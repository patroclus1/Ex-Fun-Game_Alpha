using System.Collections;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    private PlayerScript player;
    private EnemySpawner spawner;

    [SerializeField] private Transform muzzlePos;
    [SerializeField] private EnemyBulletScript bulletPrefab;
    [SerializeField] private float shootInterval = 4f;

    [SerializeField] private float moveSpeed = 25f;
    [SerializeField] private bool isCollidingWithPlayer;
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private string bulletTag = "rBullet";
    [SerializeField] private int Health = 100;
    [SerializeField] private int damage;
    [SerializeField] private ParticleSystem deathFX;
    [SerializeField] private AudioClip shootClip;
    [SerializeField] private GameObject bulletHitFx;
    private MeshRenderer meshR;
    private float defaultSpeed;

    private Vector3 persistantHeight;
    private Rigidbody rb;

    public float SetMoveSpeed
    {
        set { moveSpeed = value; }
    }

    public float GetMoveSpeed
    {
        get { return moveSpeed; }
    }

    public float SetShootInterval
    {
        set { shootInterval = value; }
    }


    public int enemyHealth
    {
        get { return Health; }
        set { Health = value; }
    }


    void Awake()
    {
        persistantHeight = transform.position;
        defaultSpeed = moveSpeed;
        meshR = GetComponent<MeshRenderer>();
        rb = GetComponent<Rigidbody>();

        StartCoroutine(ShootRoutine());
    }

    private void Update()
    {
        if (!player.isAlive) { return; }
        transform.LookAt(player.transform.position);
    }

    private void FixedUpdate()
    {
        if (!player.isAlive) { return; }

        if (isCollidingWithPlayer) { return; }

        FollowTarget();
        PersistHeight();
    }

    private void PersistHeight()
    {
        transform.position = new Vector3(transform.position.x, persistantHeight.y, transform.position.z);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag(playerTag))
        {
            StopFollowOnCollision();
            player.TakeDamage(damage);
        }
        else if (collision.collider.CompareTag(bulletTag))
        {
            HandleBulletHitFX();

            DamageOrDeath();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag(playerTag))
        {
            isCollidingWithPlayer = false;
            moveSpeed = defaultSpeed;
        }
    }

    private void FollowTarget()
    {
        rb.AddForce(transform.forward * moveSpeed * Time.fixedDeltaTime, ForceMode.VelocityChange);
    }

    private IEnumerator ShootRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(shootInterval);
            var spawnedBullet = Instantiate(bulletPrefab, muzzlePos.position, muzzlePos.rotation);
            spawnedBullet.Initialize(player);
            AudioSource.PlayClipAtPoint(shootClip, transform.position);
        }
    }

    private void StopFollowOnCollision()
    {
        isCollidingWithPlayer = true;
        moveSpeed = 0;
    }

    private void DamageOrDeath()
    {
        if (Health > 0) 
        { 
            Health = Health - 50; 
        }
        else if (Health <= 0)
        {
            gameObject.SetActive(false);
            Destroy(gameObject, 2);
            spawner.EnemyKilled();
            var particles = Instantiate(deathFX, transform.position, Quaternion.identity);
            Destroy(particles.gameObject, 2);
        }
    }

    private void HandleBulletHitFX()
    {
        var spawnedFX = Instantiate(bulletHitFx, transform.position, transform.rotation);
        Destroy(spawnedFX.gameObject, 2);

        StartCoroutine(ColorFlicker());
    }

    public void Initialize(PlayerScript playerPos, EnemySpawner spawner)
    {
        player = playerPos;
        this.spawner = spawner;
    }

    private IEnumerator ColorFlicker()
    {
        for (int i = 0; i < 3; i++)
        {
            meshR.material.color = Color.white;
            yield return new WaitForSeconds(0.2f);
            meshR.material.color = Color.black;
            yield return new WaitForSeconds(0.2f);
        }
    }
}
