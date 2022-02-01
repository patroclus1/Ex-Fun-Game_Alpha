using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections;

public class GunScript : MonoBehaviour
{
    private PlayerInput playerInput;
    private InputAction shoot;
    private InputAction reload;

    #region Outside variables
    [SerializeField] private GameObject player;
    [SerializeField] private Transform bulletPrefab;
    [SerializeField] private Transform muzzlePos;
    [SerializeField] private HUDbullets hud;
    [SerializeField] private CameraShake cameraFX;
    [SerializeField] private GameObject shootFX;
    #endregion

    #region States
    private bool isShooting;
    private bool isReloading;
    #endregion

    #region Ammo & reload

    [SerializeField] private int bullets = 8;
    private int defaultBullets;
    [SerializeField] private Slider slider;
    [SerializeField] private float reloadTime;
    [SerializeField] private GameObject reloadUI;
    private float refReload;

    public int BulletCount { get { return bullets; } }

    #endregion

    #region Audio

    [SerializeField] private AudioSource shootSound;
    [SerializeField] private AudioClip shootClip;

    #endregion


    void Awake()
    {
        playerInput = player.GetComponent<PlayerInput>();

        shoot = playerInput.actions["Shoot"];
        reload = playerInput.actions["Reload"];

        defaultBullets = bullets;
    }

    private void Update()
    {
        Shoot();
        Reload();
    }

    private void Shoot()
    {
        if (shoot == null) return;

        if (isShooting) return;

        if (isReloading) return;

        if (shoot.triggered && bullets > 0)
        {
            // Core
            Instantiate(bulletPrefab, muzzlePos.position, muzzlePos.rotation);
            bullets--;

            // FX and UI
            ShootFXHandle();
        }
    }

    private void ShootFXHandle()
    {
        var shootParticles = Instantiate(shootFX, muzzlePos.position, muzzlePos.rotation);
        Destroy(shootParticles.gameObject, 2);

        shootSound.PlayOneShot(shootClip, 0.5f);

        StartCoroutine(cameraFX.cameraShake(0.15f, 0.05f));
        hud.UpdateBulletCount();
    }

    private void Reload()
    {
        if (isReloading) return;
        if (bullets == defaultBullets) return;

        if (reload.triggered || bullets == 0)
        {
            isReloading = true;

            StartCoroutine(IWaitForReloading());
        }
    }

    IEnumerator IWaitForReloading()
    {
        float startTime = Time.time;

        while (Time.time < startTime + reloadTime)
        {
            reloadUI.SetActive(true);
            slider.value += Mathf.SmoothDamp(0,1, ref refReload, reloadTime);
            yield return null;
        }
        bullets = defaultBullets;
        reloadUI.SetActive(false);
        slider.value = 0;
        isReloading = false;
        hud.RefreshBullets();
        hud.GenerateAmmoHUD();
    }
}
