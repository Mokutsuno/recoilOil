using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class gunController : MonoBehaviour
{
    [SerializeField] GameObject pivot;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    Plane plane = new Plane();
    float distance = 0;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        Vector3 mousePos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float angleRad = Mathf.Atan2(mousePos.y - transform.position.y, mousePos.x - transform.position.x);
        float angleDeg = (180 / Mathf.PI) * angleRad-90;
        pivot.transform.rotation = Quaternion.Euler(0f, 0f, angleDeg-90);
        Debug.DrawLine(transform.position,mousePos, Color.white, Time.deltaTime);
    }
}


