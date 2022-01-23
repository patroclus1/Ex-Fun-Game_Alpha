using UnityEngine;
using UnityEngine.UI;

public class HUDbullets : MonoBehaviour
{
    private int bullets;

    [SerializeField] GunScript GunScript;
    [SerializeField] GameObject bulletCountImage;
    [SerializeField] Transform displayArea;

    [SerializeField] GameObject[] bulletsArray;

    void Awake()
    {
        bullets = GunScript.BulletCount;
        bulletsArray = new GameObject[bullets];
        GenerateAmmoHUD();
    }

    public void GenerateAmmoHUD()
    {
        bullets = GunScript.BulletCount;
        bulletsArray = new GameObject[bullets];
        for (int i = 0; i < bullets; i++)
        {
            bulletsArray[i] = SpawnSprites(i);
        }
    }

    private GameObject SpawnSprites(int index)
    {
        Vector3 individualPos = new Vector3(displayArea.position.x, displayArea.position.y + index * 40, displayArea.position.z);

        GameObject spawnedSprite = Instantiate(bulletCountImage, individualPos, Quaternion.identity, displayArea);
        spawnedSprite.name = $"bullet {index}";
        return spawnedSprite;
    }

    public void UpdateBulletCount()
    {
        bullets = GunScript.BulletCount;
        Destroy(bulletsArray[bullets]);
    }

    public void RefreshBullets()
    {
        bullets = GunScript.BulletCount;
        for (int i = 0; i < bullets; i++)
        {
            Destroy(bulletsArray[i]);
        }
    }
}
