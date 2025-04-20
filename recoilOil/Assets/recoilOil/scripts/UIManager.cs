using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private gunController gun;
    [SerializeField] private GameObject bulletSpritePrefab;
    [SerializeField] private Transform spawnParent;
    [SerializeField] private Vector3 spawnOffset = Vector3.zero;
    [SerializeField] private float spacing = 50f;
    [SerializeField] private GameObject disappearingBulletPrefab;

    private List<GameObject> activeBullets = new List<GameObject>();     // 表示中
    private Queue<GameObject> bulletPool = new Queue<GameObject>();      // 非表示プール

    void Start()
    {
        if (gun != null)
        {
            gun.OnAmmoChanged += UpdateAmmoDisplay;

            int currentAmmo = gun.GetRemainingAmmo();
            UpdateAmmoDisplay(currentAmmo);
        }
    }

    void OnDestroy()
    {
        if (gun != null)
        {
            gun.OnAmmoChanged -= UpdateAmmoDisplay;
        }
    }


    void UpdateAmmoDisplay(int currentAmmo)
    {
        // 減った分 → アニメ付き消去
        while (activeBullets.Count > currentAmmo)
        {
            int lastIndex = activeBullets.Count - 1;
            GameObject bullet = activeBullets[lastIndex];
            activeBullets.RemoveAt(lastIndex);

            // ✅ アニメ付きの削除演出用プレハブを生成
            Vector3 pos = bullet.transform.position;
            Transform parent = bullet.transform.parent;
            Instantiate(disappearingBulletPrefab, pos, Quaternion.identity, parent);

            // プールに戻す
            bullet.SetActive(false);
            bulletPool.Enqueue(bullet);
        }

        // 増えた分 → プール or 生成
        while (activeBullets.Count < currentAmmo)
        {
            GameObject bullet;
            if (bulletPool.Count > 0)
            {
                bullet = bulletPool.Dequeue();
            }
            else
            {
                Vector3 pos = Vector3.zero;
                bullet = Instantiate(bulletSpritePrefab, pos, Quaternion.identity, spawnParent);
            }

            bullet.SetActive(true);
           // ResetBulletAnimator(bullet); // アニメ初期化（任意）
            activeBullets.Add(bullet);
        }

        // 並びなおし
        for (int i = 0; i < activeBullets.Count; i++)
        {
            Vector3 pos = spawnParent.position + new Vector3(i * spacing, 0, 0) + spawnOffset;
            activeBullets[i].transform.position = pos;
        }
    }
}
