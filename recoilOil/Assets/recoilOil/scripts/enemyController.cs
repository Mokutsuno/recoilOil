using UnityEngine;

public class enemyController : MonoBehaviour,IDamagable, IAmmoSpawnable
{
    [SerializeField] int ammo;
    [SerializeField] int health;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void ReceiveDamage(int damage)
    {
        health -= damage;
        Debug.Log("Enemy damaged!");
        if (health <= 0) {
            // Ammo¶¬‚ð’Ê’mi‚±‚±‚Å“n‚·j
            gunController.Instance?.OnAmmoGained(ammo); 
            Destroy(gameObject);
        }
    }
    public int SpawnAmmo()
    {
        if(health <= 0)
        {
            Debug.Log("SpawnAmmo");
            return ammo;
        }
        return 0;
    }
}