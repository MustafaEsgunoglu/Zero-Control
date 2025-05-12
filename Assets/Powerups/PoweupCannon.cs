using System.Collections;
using UnityEngine;

public class PoweupCannon : MonoBehaviour
{
    GameObject m_Player;
    [SerializeField] float duration = 3;
    [SerializeField] float multiplier = 2;
    PowerupManager m_PowerupManager;
    void Start()
    {
        m_PowerupManager = FindAnyObjectByType<PowerupManager>();
        m_Player = FindAnyObjectByType<PlayerMover>().gameObject;;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Cannon cannon = m_Player.GetComponent<Cannon>();
        if (other.gameObject.CompareTag("Player"))
        {
            m_PowerupManager.Cannon(m_Player, multiplier, duration);
            Destroy(gameObject);
        }
    }

    

}