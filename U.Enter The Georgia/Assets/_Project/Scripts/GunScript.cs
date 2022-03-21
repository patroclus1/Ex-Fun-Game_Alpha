using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections;

public class GunScript : MonoBehaviour
{
    private PlayerInput _playerInput;
    private InputAction _shoot;
    private InputAction _reload;

    #region Outside variables
    [SerializeField] private GameObject _player;
    [SerializeField] private Transform _bulletPrefab;
    [SerializeField] private Transform _muzzlePos;
    [SerializeField] private HUDbullets _hud;
    [SerializeField] private CameraShake _cameraFX;
    [SerializeField] private GameObject _shootFX;
    #endregion

    #region States
    private bool _isShooting;
    private bool _isReloading;
    #endregion

    #region Ammo & reload

    [SerializeField] private Slider _slider;
    [SerializeField] private GameObject _reloadUI;
    [SerializeField] private float _reloadTime;
    [SerializeField] private int _bullets = 8;
                     private int _defaultBullets;
                     private float _refReload;

    public int BulletCount { get { return _bullets; } }

    #endregion

    #region Audio

                     private AudioSource _shootSound;
    [SerializeField] private AudioClip _shootClip;

    #endregion


    void Awake()
    {
        _shootSound = GetComponent<AudioSource>();
        _playerInput = _player.GetComponent<PlayerInput>();

        _shoot = _playerInput.actions["Shoot"];
        _reload = _playerInput.actions["Reload"];

        _defaultBullets = _bullets;
    }

    private void Update()
    {
        Shoot();
        Reload();
    }

    private void Shoot()
    {
        if (_shoot == null) return;

        if (_isShooting) return;

        if (_isReloading) return;

        if (_shoot.triggered && _bullets > 0)
        {
            // Core
            Instantiate(_bulletPrefab, _muzzlePos.position, _muzzlePos.rotation);
            _bullets--;

            // FX and UI
            ShootFXHandle();
        }
    }

    private void ShootFXHandle()
    {
        var shootParticles = Instantiate(_shootFX, _muzzlePos.position, _muzzlePos.rotation);
        Destroy(shootParticles.gameObject, 2);

        _shootSound.PlayOneShot(_shootClip, 0.5f);

        StartCoroutine(_cameraFX.cameraShake(0.15f, 0.05f));
        _hud.UpdateBulletCount();
    }

    private void Reload()
    {
        if (_isReloading) return;
        if (_bullets == _defaultBullets) return;

        if (_reload.triggered || _bullets == 0)
        {
            _isReloading = true;

            StartCoroutine(IWaitForReloading());
        }
    }

    IEnumerator IWaitForReloading()
    {
        float startTime = Time.time;

        while (Time.time < startTime + _reloadTime)
        {
            _reloadUI.SetActive(true);
            _slider.value += Mathf.SmoothDamp(0,1, ref _refReload, _reloadTime);
            yield return null;
        }
        _bullets = _defaultBullets;
        _reloadUI.SetActive(false);
        _slider.value = 0;
        _isReloading = false;
        _hud.RefreshBullets();
        _hud.GenerateAmmoHUD();
    }
}
