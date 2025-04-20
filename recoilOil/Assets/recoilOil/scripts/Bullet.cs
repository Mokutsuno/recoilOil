using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
    private Rigidbody2D rb2D;
    [SerializeField] float velocity = 10f;
    [SerializeField] int damage;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();

        // transform.up は Z軸回転に対する「前方（上）」の方向を返す（Unity 2Dでよく使う）
        rb2D.linearVelocity= -transform.right * velocity;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        var ammoSource = collision.GetComponent<IAmmoSpawnable>();
        if (ammoSource != null)
        {
            int ammo = ammoSource.SpawnAmmo();

            // 🔔 gunController に通知
           // gunController.Instance?.OnAmmoGained(ammo);
        }

        var target = collision.GetComponent<IDamagable>();
        target?.ReceiveDamage(damage);

        Destroy(gameObject); // 弾を消す
    }
}