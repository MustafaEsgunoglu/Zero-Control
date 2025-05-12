using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    [SerializeField] float lifeTime = 2;
    [SerializeField] float m_BulletDamage = 1;
    Cannon cannon;
    float timer;

    public void SetBulletDamage(float BulletDamage) { m_BulletDamage = BulletDamage; }
    public float GetBulletDamage() { return m_BulletDamage; }

    void Start()
    {
        cannon = FindAnyObjectByType<Cannon>();
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
            transform.localScale = new(2.5f, 2.5f, 2.5f);
            cannon.ReturnBulletToPool(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        cannon.ReturnBulletToPool(gameObject);
    }
}
