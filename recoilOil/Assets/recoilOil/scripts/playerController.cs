using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Rigidbody2D))]
public class playerController : MonoBehaviour
{
    private Collider2D collider2D;
    private Rigidbody2D rb2D;
    [SerializeField] public float playerMoveForce;


    private void Awake()
    {
        collider2D = GetComponent<Collider2D>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
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

    // 銃側など他のスクリプトからこれを呼び出す
    // 銃側など他のスクリプトからこれを呼び出す
    public bool CheckClickFromScreenRay(Vector3 screenPosition)
    {
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(screenPosition);
        RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero);

        if (hit.collider != null && hit.collider == collider2D)
        {
            return true;
        }
        return false;
    }
}

