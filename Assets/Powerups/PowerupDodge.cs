using System.Collections;
using UnityEngine;

public class PowerupDodge : MonoBehaviour
{
    GameObject m_Player;
    [SerializeField] float duration = 3;
    [SerializeField] float multiplier = 1.5f;
    PowerupManager m_PowerupManager;
    void Start()
    {
        m_PowerupManager = FindAnyObjectByType<PowerupManager>();
        m_Player = FindAnyObjectByType<PlayerMover>().gameObject;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            m_PowerupManager.Dodge(m_Player, multiplier, duration);
            Destroy(gameObject);
        }
    }
    
}
