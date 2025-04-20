using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class gunController : MonoBehaviour
{
    [Header("参照オブジェクト")]
    [SerializeField] GameObject pivot;
    [SerializeField] GameObject player;
    [SerializeField] GameObject reticle;

    [Header("弾の制御")]
    [SerializeField] GameObject bullet;
    [SerializeField] float reticleLength = 10f;
    [SerializeField] float shootPower = 500f;
    [SerializeField] float minRotationDistance = 0.1f;
    [SerializeField] int maxAmmo = 6;
    [SerializeField] GameObject muzzlePos;
    [SerializeField]  int remainingAmmo;
    [Header("薬莢の設定")]
    [SerializeField] GameObject cartridgePrefab;           // 再利用する薬莢プレハブ
    [SerializeField] int initialCartridgePoolSize = 10;    // 最初に確保しておく薬莢の数
    [SerializeField] float cartridgeLifetime = 1.0f;       // 表示後に非表示にするまでの時間（秒）

    private Queue<GameObject> cartridgePool = new Queue<GameObject>();  // cartridge用オブジェクトプール

    private float previousAngle = 0f;
    private float angleDeg;
    private Vector3 mousePos;

    public int GetRemainingAmmo() => remainingAmmo;
    public System.Action<int> OnAmmoChanged;
    public static gunController Instance;

    Plane plane = new Plane(); // 未使用だが保持


    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        remainingAmmo = maxAmmo;
        // 初期UI表示のために明示的に通知
        OnAmmoChanged?.Invoke(remainingAmmo);
        // 🔁 cartridgeプレハブをあらかじめ生成して非アクティブにし、プールに入れておく
        for (int i = 0; i < initialCartridgePoolSize; i++)
        {
            GameObject cart = Instantiate(cartridgePrefab);
            cart.SetActive(false);
            cartridgePool.Enqueue(cart);
        }
    }

    void Update()
    {
        // マウス位置を取得（ワールド座標）
        Vector3 screenMouse = Input.mousePosition;
        screenMouse.z = Mathf.Abs(Camera.main.transform.position.z);
        mousePos = Camera.main.ScreenToWorldPoint(screenMouse);

        // pivotの回転を制御
        Vector2 dir = transform.position - mousePos;
        if (dir.magnitude > minRotationDistance)
        {
            previousAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        }
        angleDeg = previousAngle;

        if (Input.GetMouseButtonDown(0)) // 左クリック
        {
            Vector3 mousePos = Input.mousePosition;
            Shoot();
        }


        if (Input.GetKeyDown(KeyCode.R)) ReloadAmmo();

        // クリックされたときに呼び出されるメソッド

        pivot.transform.rotation = Quaternion.Euler(0f, 0f, angleDeg);

        // 左クリックで発射


        DrawReticle();
    }

    void Shoot()
    {
        Debug.Log("Shoot");
        // 残弾がない場合は発射不可
        if (remainingAmmo <= 0) return;

        // プレイヤーに力を加える（発射演出）
        Rigidbody2D rb2D = player.GetComponent<Rigidbody2D>();
        float xcomponent = Mathf.Cos(angleDeg * Mathf.Deg2Rad) * shootPower;
        float ycomponent = Mathf.Sin(angleDeg * Mathf.Deg2Rad) * shootPower;
        Vector2 force = new Vector2(xcomponent, ycomponent);
        rb2D.AddForce(force);

        // 残弾減少
        remainingAmmo--;

        // 🔁 薬莢を再利用して発射（見た目演出）
        GameObject cart = GetCartridgeFromPool(); // プールから取り出す（または生成）
        cart.transform.position = transform.position;
        cart.transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles);
        cart.SetActive(true);

        // 一定時間後に非表示＋プールに戻す
        StartCoroutine(DisableAfterTime(cart, cartridgeLifetime));

        // 残弾数をUIへ通知
        OnAmmoChanged?.Invoke(remainingAmmo);

        Instantiate(bullet, muzzlePos.transform.position,Quaternion.Euler( transform.rotation.eulerAngles));
    }

    // 🔁 プールから薬莢を取得（なければ新規生成）
    GameObject GetCartridgeFromPool()
    {
        Debug.Log("GetCartridgeFromPool");
        if (cartridgePool.Count > 0)
        {
            return cartridgePool.Dequeue();
        }
        else
        {
            GameObject newCart = Instantiate(cartridgePrefab);
            newCart.SetActive(false);
            return newCart;
        }
    }

    // ⏱ 一定時間経過後に薬莢を非表示にしてプールへ戻す
    IEnumerator DisableAfterTime(GameObject obj, float time)
    {
        yield return new WaitForSeconds(time);
        obj.SetActive(false);
        cartridgePool.Enqueue(obj);
    }

    // 🎯 レティクルの描画位置を更新
    void DrawReticle()
    {
        Vector3 origin = pivot.transform.position;
        Vector3 flatMouse = mousePos;
        origin.z = 0f;
        flatMouse.z = 0f;

        Vector3 direction = (flatMouse - origin).normalized;

        // Playerレイヤーを無視してRaycast
        int mask = ~(1 << LayerMask.NameToLayer("Player"));
        RaycastHit2D hit = Physics2D.Raycast(origin, direction, reticleLength, mask);

        Vector3 retPos;
        if (hit.collider != null)
        {
            retPos = new Vector3(hit.point.x, hit.point.y, 0.1f);
        }
        else
        {
            retPos = origin + direction * reticleLength;
            retPos.z = 0.1f;
        }

        reticle.transform.position = retPos;

        // デバッグ用Ray表示
        Debug.DrawRay(origin, direction * reticleLength, Color.red, 0f);
    }

    void ReloadAmmo()
    {
        Debug.Log("ReloadAmmo");
        remainingAmmo = maxAmmo;
        OnAmmoChanged?.Invoke(remainingAmmo);

    }


    public void OnAmmoGained(int amount)
    {
        remainingAmmo += amount;
        remainingAmmo = Mathf.Min(remainingAmmo, maxAmmo);
        OnAmmoChanged?.Invoke(remainingAmmo);
        //Debug.Log($"Ammo gained: {amount}");
    }

}
