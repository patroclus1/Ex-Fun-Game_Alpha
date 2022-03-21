using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerScript : MonoBehaviour
{
    // Controls
    private Transform _transform;
    private CharacterController _controller;
    private PlayerInput _playerInput;
    private InputAction _move;
    private InputAction _dash;
    private InputAction _escape;

    private Vector2 _input;
    private Vector3 _moveDirection;

    // Animation
    private Animator _anim;

    // Gameplay
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _dashForce = 15f;
    [SerializeField] private float _dashTime = 0.25f;
    [SerializeField] private int _health;

    private bool _isDashing;
    private float _velocityZ;
    private float _velocityX;


    private bool _isPlayerAlive = true;

    private Vector3 _persistantHeight;

    // FX and UI
    [SerializeField] private LayerMask _defaultLayer;
    [SerializeField] private LayerMask _invincibleLayer;
    [SerializeField] private Light _playerLight;
    [SerializeField] private GameObject _playerGFX;
    [SerializeField] private CameraShake _cameraFX;
    [SerializeField] private ParticleSystem _deathFX;
                     private AudioSource _hitSound;

    [SerializeField] private GameObject _deathUI;
    [SerializeField] private GameObject _pauseUI;
    [SerializeField] private GameObject _victoryUI;
                     private Color _defaultColor;

    public bool Dashing
    {
        get { return _isDashing; }
    }

    public int Health { get => _health; }

    public bool IsPlayerAlive
    {
        get { return _isPlayerAlive; }
    }

    public bool ReportPlayerDeath
    {
        set { _isPlayerAlive = value; }
    }


    void Awake()
    {
        _deathUI.SetActive(false);
        _pauseUI.SetActive(false);

        Cursor.lockState = CursorLockMode.Confined;
        _hitSound = GetComponent<AudioSource>();

        _transform = GetComponent<Transform>();
        _anim = GetComponentInChildren<Animator>();
        _controller = GetComponent<CharacterController>();
        _playerInput = GetComponent<PlayerInput>();

        _move = _playerInput.actions["Move"];
        _dash = _playerInput.actions["Dash"];
        _escape = _playerInput.actions["Escape"];

        _defaultColor = _playerLight.color;
        gameObject.layer = _defaultLayer;
    }


    private void Update()
    {
        Move();
        Dash();
        OpenMenuOnEscape();
    }

    private void Move()
    {
        if (_isDashing) return;

        _input = _move.ReadValue<Vector2>();
        _moveDirection = new Vector3(_input.x, 0, _input.y);

        if (_moveDirection.magnitude > 0) 
        {
            _moveDirection.Normalize();
            _controller.Move(_moveDirection * _moveSpeed * Time.deltaTime);
        }

        // Animating
        _velocityZ = Vector3.Dot(_moveDirection.normalized, _transform.forward);
        _velocityX = Vector3.Dot(_moveDirection.normalized, _transform.right);

        _anim.SetFloat("Vertical", _velocityZ, 0.1f, Time.deltaTime);
        _anim.SetFloat("Horizontal", _velocityX, 0.1f, Time.deltaTime);

        // Patching height
        if (_transform.position.y == 1f) return;

        PersistHeight();
    }

    private void PersistHeight()
    {
        _persistantHeight = new Vector3(_transform.position.x, 1f, _transform.position.z);
        _transform.position = _persistantHeight;
    }

    private void Dash()
    {
        if (_isDashing) return;

        if (_dash.triggered)
        {
            _velocityZ = Vector3.Dot(_moveDirection.normalized, _transform.forward);
            _anim.SetTrigger("Roll");
            _anim.SetFloat("RollDirection", _velocityZ);
            StartCoroutine(StartDash());
            StartCoroutine(_cameraFX.cameraShake(0.1f, 0.04f));
        }
    }

    private IEnumerator StartDash()
    {
        float startTime = Time.time;
        _isDashing = true;
        gameObject.layer = _invincibleLayer;

        while (Time.time < startTime + _dashTime)
        {
            _controller.Move(_moveDirection.normalized * _dashForce * Time.deltaTime);
            yield return null;
        }
        _isDashing = false;
        gameObject.layer = _defaultLayer;
    }

    public void TakeDamage(int dmg)
    {
        if (_health > 0)
        {
            StartCoroutine(StartAlphaFlicker());
            _health -= dmg;
            _hitSound.Play();
        }
        if (_health <= 0)
        {
            StopAllCoroutines();
            _health = 0;
            PlayerDied();
        }
        StartCoroutine(_cameraFX.cameraShake(0.1f, 0.1f));
    }

    public void PlayerDied()
    {
        var particles = Instantiate(_deathFX, _transform.position, Quaternion.identity);
        Destroy(particles.gameObject, 2);
        Destroy(_playerGFX, 0.5f);
        
        enabled = false;
        _isPlayerAlive = false;
        enabled = false;

        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        Cursor.lockState = CursorLockMode.None;
        _pauseUI.SetActive(false);
        _deathUI.SetActive(true);
    }

    private IEnumerator StartAlphaFlicker()
    {
        gameObject.layer = _invincibleLayer;
        gameObject.tag = "Untagged";
        for (int i = 0; i < 3; i++)
        {
            _playerLight.color = Color.black;
            yield return new WaitForSeconds(0.2f);
            _playerLight.color = Color.red;
            yield return new WaitForSeconds(0.2f);
        }
        _playerLight.color = _defaultColor;
        gameObject.layer = _defaultLayer;
        gameObject.tag = "Player";
    }

    private void OpenMenuOnEscape()
    {
        if (_escape.triggered && !_pauseUI.activeInHierarchy && !_victoryUI.activeInHierarchy)
        {
            _pauseUI.SetActive(true);
        }
        else if (_escape.triggered && _pauseUI.activeInHierarchy)
        {
            _pauseUI.SetActive(false);
        }
    }
}
