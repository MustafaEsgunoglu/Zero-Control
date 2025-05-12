using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletPool : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] int m_PoolSize;
    [SerializeField] GameObject m_BulletPrefab;
    List<GameObject> m_BulletPool;
    void Start()
    {
        InitializePool();
    }
    void InitializePool()
    {
        m_BulletPool = new List<GameObject>();
        for (int i = 0; i < m_PoolSize; i++)
        {
            GameObject bullet = Instantiate(m_BulletPrefab);
            bullet.SetActive(false);
            m_BulletPool.Add(bullet);
        }
    }

    // Update is called once per frame
    public GameObject GetBulletFromPool()
    {
        foreach (GameObject bullet in m_BulletPool)
        {
            if (!bullet.activeInHierarchy)
            {
                bullet.SetActive(true);
                return bullet;
            }
        }
        return null; // No available bullet
    }

    public void ReturnBulletToPool(GameObject bullet)
    {
        bullet.SetActive(false);
    }
}


