using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    float m_BulletDamage = 1;
    [SerializeField] float lifeTime = 2;
    EnemyBulletPool enemyBulletPool;
    float timer;

    public float GetBulletDamage()
    { return m_BulletDamage; }
    public void SetBulletDamage(float BulletDamage)
    { m_BulletDamage = BulletDamage; }
    void Start()
    {
        enemyBulletPool = FindAnyObjectByType<EnemyBulletPool>();
    }

    void OnEnable()
    {
        timer = 0; // Reset timer when bullet is reused
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= lifeTime)
        {
            transform.localScale = new(1, 1, 1);
            enemyBulletPool.ReturnBulletToPool(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        transform.localScale = new(1, 1, 1);
        enemyBulletPool.ReturnBulletToPool(gameObject);

    }
}


