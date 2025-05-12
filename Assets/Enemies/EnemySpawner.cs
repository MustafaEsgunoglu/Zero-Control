using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] m_enemies; // Enemy prefabs

    [SerializeField] private Transform[] m_spawnPoints; // Spawn positions
    [SerializeField] private float m_distance; // Minimum distance to avoid spawning
    [SerializeField] private float m_spawnDelay; // Delay before each wave spawns
    [SerializeField] float m_EnemySpawnCount;
    private Transform m_Player; // Player's Transform,


    private void Start()
    {
        m_Player = FindAnyObjectByType<PlayerMover>().transform;
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        for (int i = 0; i < m_EnemySpawnCount; i++)
        {
            yield return new WaitForSeconds(m_spawnDelay);

            // Build a list of spawn points that are far enough from the player.
            List<Transform> validSpawnPoints = new();
            foreach (Transform spawnPoint in m_spawnPoints)
            {
                if (Vector3.Distance(spawnPoint.position, m_Player.position) >= m_distance)
                {
                    validSpawnPoints.Add(spawnPoint);
                }
            }

            // If there's at least one valid spawn point, choose one at random.
            if (validSpawnPoints.Count > 0)
            {
                Transform chosenSpawn = validSpawnPoints[Random.Range(0, validSpawnPoints.Count)];
                GameObject randomEnemy = m_enemies[Random.Range(0, m_enemies.Length)];
                Instantiate(randomEnemy, chosenSpawn.position, chosenSpawn.rotation);
            }
            else
            {
                Debug.LogWarning("No valid spawn point found for enemy spawn.");
            }
        }
    }
}
