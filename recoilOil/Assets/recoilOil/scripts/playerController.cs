using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class playerController : MonoBehaviour
{
    private Rigidbody2D rb2D;
    [SerializeField] public float playerMoveForce;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb2D= GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A)) { movePlayer(0); print("A"); }
        ;
        if (Input.GetKey(KeyCode.D)) { movePlayer(1); print("D"); }
        ;
    }

    private void movePlayer(int i)
    {
        if (i == 0)
        {
            rb2D.linearVelocityX = -playerMoveForce;

        }
        if (i == 1)
        {
            rb2D.linearVelocityX = playerMoveForce;
                }
    }
}
