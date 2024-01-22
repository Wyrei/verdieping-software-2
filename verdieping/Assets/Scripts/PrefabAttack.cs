using UnityEngine;

public class PrefabAttack : MonoBehaviour
{
    [SerializeField] private float Timer;
    
    Stats stats;
    
    void Start()
    {
        stats = GetComponentInParent<Stats>();
        
        if (stats == null)
        {
            Debug.LogError("Stats script not found. Make sure it's on the same GameObject or a parent GameObject.");
        }
    }
    
    void Update()
    {
        Timer -= Time.deltaTime;

        if (Timer <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
            EnemyHealth enemyHealth = collision.collider.GetComponent<EnemyHealth>();

            if (enemyHealth != null && stats != null)
            {
                enemyHealth.TakeDamage(stats.Damage);
            }
            Destroy(gameObject);
        }
    }
}

