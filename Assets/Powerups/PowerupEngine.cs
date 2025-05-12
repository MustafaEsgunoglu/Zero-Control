using System.Collections;
using UnityEngine;

public class PowerupEngine : MonoBehaviour
{
    [SerializeField] float duration = 3;
    [SerializeField] float multiplier = 1.2f;
    PowerupManager m_PowerupManager;
    GameObject m_Player;


    void Start()
    {
        m_PowerupManager = FindAnyObjectByType<PowerupManager>();
        m_Player = FindAnyObjectByType<PlayerMover>().gameObject;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("a");
            m_PowerupManager.Engine(m_Player, multiplier, duration);
            Destroy(gameObject);
        }
    }
}
