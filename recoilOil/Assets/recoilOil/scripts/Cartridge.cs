using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Cartridge : MonoBehaviour
{
    [SerializeField] Vector2 force;
    [SerializeField] float torque;
    Rigidbody2D rb2D;

    void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    // 🔁 SetActive(true) のたびに実行される
    void OnEnable()
    {
        // 速度をリセットしないと前回の挙動を引きずる
        rb2D.linearVelocity = Vector2.zero;
        rb2D.angularVelocity = 0f;

        // 再度力を加える
        rb2D.AddForce(force, ForceMode2D.Impulse);
        rb2D.AddTorque(torque, ForceMode2D.Impulse);
    }
}
