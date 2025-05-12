using System.Collections;
using UnityEngine;

public class PowerupHealth : MonoBehaviour
{
    GameObject m_Player;
    void Start()
    {
        m_Player = FindAnyObjectByType<PlayerMover>().gameObject;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            HealPlayer();
            Destroy(gameObject);
        }
    }
    void HealPlayer()
    {
        Health health = m_Player.GetComponent<Health>();
        float maxHp = health.GetMaxHp();
        float originalValue = health.GetHp();
        float healAmount = health.GetConsumableHp();
     
        health.SetHp(originalValue + healAmount);
        if(health.GetHp() > maxHp)
        {
            health.SetHp(maxHp);
        }

    }
}
