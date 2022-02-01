using UnityEngine;
using System.Collections;

public class MenuDecorSpawnerScript : MonoBehaviour
{
    #region Outside variables
    [SerializeField] private Transform bulletPrefab;
    [SerializeField] private Transform muzzlePos;
    [SerializeField] private GameObject shootFX;
                     private AudioSource shootSound;
    #endregion

    void Awake()
    {
        shootSound = GetComponent<AudioSource>();
        Shoot();
    }

    private void Shoot()
    {
        StartCoroutine(ShootingCoroutine());
    }

    private void ShootFXHandle()
    {
        var shootParticles = Instantiate(shootFX, muzzlePos.position, muzzlePos.rotation);
        Destroy(shootParticles.gameObject, 2);
    }

    private IEnumerator ShootingCoroutine()
    {
        while (true)
        {
            float random = Random.Range(0.5f, 3);
            Instantiate(bulletPrefab, muzzlePos.position, muzzlePos.rotation);

            ShootFXHandle();
            shootSound.PlayOneShot(shootSound.clip, 0.3f);
            yield return new WaitForSeconds(random);
        }
    }
}
