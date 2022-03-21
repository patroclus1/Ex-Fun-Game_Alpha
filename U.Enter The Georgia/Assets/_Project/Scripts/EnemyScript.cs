using System.Collections;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    private PlayerScript _player;
    private EnemySpawner _spawner;

    [SerializeField] private Transform _muzzlePosition;
    [SerializeField] private EnemyBulletScript _bulletPrefab;
    [SerializeField] private float _fireInterval = 4f;

    [SerializeField] private float _moveSpeed = 25f;
    [SerializeField] private bool _isCollidingWithPlayer;
    [SerializeField] private string _playerTag = "Player";
    [SerializeField] private string _bulletTag = "rBullet";
    [SerializeField] private int _health = 100;
    [SerializeField] private int _damage;
    [SerializeField] private ParticleSystem _deathFX;
    [SerializeField] private AudioClip _shootSound;
    [SerializeField] private GameObject _bulletHitFx;
    private MeshRenderer _meshRenderer;
    private float _defaultMoveSpeed;

    private Vector3 _persistantHeight;
    private Rigidbody _rb;
    private Transform _transform;

    public float SetMoveSpeed
    {
        set { _moveSpeed = value; }
    }

    public float GetMoveSpeed
    {
        get { return _moveSpeed; }
    }

    public float SetShootInterval
    {
        set { _fireInterval = value; }
    }

    public int EnemyHealth
    {
        get { return _health; }
        set { _health = value; }
    }


    void Awake()
    {
        _persistantHeight = transform.position;
        _defaultMoveSpeed = _moveSpeed;
        _meshRenderer = GetComponent<MeshRenderer>();
        _transform = GetComponent<Transform>();
        _rb = GetComponent<Rigidbody>();

        StartCoroutine(ShootRoutine());
    }

    private void Update()
    {
        if (!_player.IsPlayerAlive) { return; }
        
        _transform.LookAt(_player.transform.position);
    }

    private void FixedUpdate()
    {
        if (!_player.IsPlayerAlive) { return; }

        if (_isCollidingWithPlayer) { return; }

        FollowTarget();
        //PersistHeight();
    }

    private void PersistHeight()
    {
        _transform.position = new Vector3(_transform.position.x, _persistantHeight.y, _transform.position.z);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag(_playerTag))
        {
            StopFollowOnCollision();
            _player.TakeDamage(_damage);
        }
        else if (collision.collider.CompareTag(_bulletTag))
        {
            HandleBulletHitFX();

            DamageOrDeath();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag(_playerTag))
        {
            _isCollidingWithPlayer = false;
            _moveSpeed = _defaultMoveSpeed;
        }
    }

    private void FollowTarget()
    {
        _rb.AddForce(_transform.forward * _moveSpeed * Time.fixedDeltaTime, ForceMode.VelocityChange);
    }

    private IEnumerator ShootRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(_fireInterval);
            var spawnedBullet = Instantiate(_bulletPrefab, _muzzlePosition.position, _muzzlePosition.rotation);
            spawnedBullet.Initialize(_player);
            AudioSource.PlayClipAtPoint(_shootSound, _transform.position);
        }
    }

    private void StopFollowOnCollision()
    {
        _isCollidingWithPlayer = true;
        _moveSpeed = 0;
    }

    private void DamageOrDeath()
    {
        if (_health > 0) 
        { 
            _health = _health - 50; 
        }
        else if (_health <= 0)
        {
            gameObject.SetActive(false);
            Destroy(gameObject, 2);
            _spawner.EnemyKilled();
            var particles = Instantiate(_deathFX, _transform.position, Quaternion.identity);
            Destroy(particles.gameObject, 2);
        }
    }

    private void HandleBulletHitFX()
    {
        var spawnedFX = Instantiate(_bulletHitFx, _transform.position, transform.rotation);
        Destroy(spawnedFX.gameObject, 2);

        StartCoroutine(ColorFlicker());
    }

    public void Initialize(PlayerScript playerPos, EnemySpawner spawner)
    {
        _player = playerPos;
        this._spawner = spawner;
    }

    private IEnumerator ColorFlicker()
    {
        for (int i = 0; i < 3; i++)
        {
            _meshRenderer.material.color = Color.white;
            yield return new WaitForSeconds(0.2f);
            _meshRenderer.material.color = Color.black;
            yield return new WaitForSeconds(0.2f);
        }
    }
}
