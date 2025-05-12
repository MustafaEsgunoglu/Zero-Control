using UnityEngine;
using System.Collections;

public class EnemyHP : MonoBehaviour
{
    [Header("Enemy Health")]
    [SerializeField] float m_Health;
    [SerializeField] GameObject explosion;
    [SerializeField] SpriteRenderer[] spriteRenderers;

    public float GetHealth()
    { return m_Health; }

    public void SetHealth(float health)
    { m_Health = health; }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("PlayerBullet"))
        {
            PlayerBullet bullet = other.gameObject.GetComponent<PlayerBullet>();
            StartCoroutine(ChangeColor());
            m_Health -= bullet.GetBulletDamage();

            if (m_Health > 0)
            {
                other.gameObject.SetActive(false);
            }
            else
            {
                if (GetComponent<EnemyMoney>() != null)
                {
                    GetComponent<EnemyMoney>().DropGearandScrew();
                    Instantiate(explosion, transform.position, Quaternion.identity);
                    Destroy(gameObject);
                    other.gameObject.SetActive(false);
                }
            }
        }
        if (CompareTag("Kamikaze"))
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    bool isChangingColor = false;

    IEnumerator ChangeColor()
    {
        if (isChangingColor) yield break; // Already running
        isChangingColor = true;

        if (spriteRenderers != null)
        {
            spriteRenderers[0].color = Color.red;

            if(spriteRenderers.Length > 1)
            {spriteRenderers[1].color = Color.red;}

            yield return new WaitForSeconds(0.25f);

            spriteRenderers[0].color = Color.white;

            if(spriteRenderers.Length > 1)
            {spriteRenderers[1].color = Color.white;}
        }

        isChangingColor = false;
    }
}

