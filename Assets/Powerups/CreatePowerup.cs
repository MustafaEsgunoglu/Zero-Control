using System.Collections;
using UnityEngine;

public class CreatePowerup : MonoBehaviour
{
    GameObject m_Player;
    [SerializeField] float m_SpawnRadius;
    [SerializeField] GameObject m_HealthPowerup;
    [SerializeField] int m_HealthPowerupCount = 2;
    [SerializeField] float m_HealthPowerupMinDelay = 1.0f;
    [SerializeField] float m_HealthPowerupMaxDelay = 3.0f; // Maximum delay between spawns
    [SerializeField] GameObject m_EnginePowerup;
    [SerializeField] int m_EnginePowerupCount = 2;
    [SerializeField] float m_EnginePowerupMinDelay = 1.0f;
    [SerializeField] float m_EnginePowerupMaxDelay = 3.0f;
    [SerializeField] GameObject m_CanonPowerup;
    [SerializeField] int m_CanonPowerupCount = 2;
    [SerializeField] float m_CanonPowerupMinDelay = 1.0f;
    [SerializeField] float m_CanonPowerupMaxDelay = 3.0f;
    [SerializeField] GameObject m_DodgePowerup;
    [SerializeField] int m_DodgePowerupCount = 2;
    [SerializeField] float m_DodgePowerupMinDelay = 1.0f;
    [SerializeField] float m_DodgePowerupMaxDelay = 3.0f;
    void Start()
    {
        m_Player = FindAnyObjectByType<PlayerMover>().gameObject;
        CreatePowerups();
    }

    void CreatePowerups()
    {
        StartCoroutine(DropHealthPowerups(m_HealthPowerup, m_HealthPowerupCount));
        StartCoroutine(DropEnginePowerups(m_EnginePowerup, m_EnginePowerupCount));
        StartCoroutine(DropCanonPowerups(m_CanonPowerup, m_CanonPowerupCount));
        StartCoroutine(DropDodgePowerups(m_DodgePowerup, m_DodgePowerupCount));
    }

    IEnumerator DropHealthPowerups(GameObject powerupPrefab, int count)
    {
        if (powerupPrefab == null || m_Player == null) yield break;
        for (int i = 0; i < count; i++)
        {
            Vector2 randomOffset = new Vector2(
                Random.Range(-m_SpawnRadius, m_SpawnRadius),
                Random.Range(-m_SpawnRadius, m_SpawnRadius)
            );

            // Wait for a random time before spawning the next one
            float delay = Random.Range(m_HealthPowerupMinDelay, m_HealthPowerupMaxDelay);

            yield return new WaitForSecondsRealtime(delay);
            Vector2 spawnPosition = (Vector2)m_Player.transform.position + randomOffset;
            Instantiate(powerupPrefab, spawnPosition, Quaternion.identity);
        }
    }

    IEnumerator DropEnginePowerups(GameObject powerupPrefab, int count)
    {
        if (powerupPrefab == null || m_Player == null) yield break;
        for (int i = 0; i < count; i++)
        {
            Vector2 randomOffset = new Vector2(
                Random.Range(-m_SpawnRadius, m_SpawnRadius),
                Random.Range(-m_SpawnRadius, m_SpawnRadius)
            );

            // Wait for a random time before spawning the next one
            float delay = Random.Range(m_EnginePowerupMinDelay, m_EnginePowerupMaxDelay);

            yield return new WaitForSecondsRealtime(delay);
            Vector2 spawnPosition = (Vector2)m_Player.transform.position + randomOffset;
            Instantiate(powerupPrefab, spawnPosition, Quaternion.identity);
        }
    }

    IEnumerator DropCanonPowerups(GameObject powerupPrefab, int count)
    {
        if (powerupPrefab == null || m_Player == null) yield break;
        for (int i = 0; i < count; i++)
        {
            Vector2 randomOffset = new Vector2(
                Random.Range(-m_SpawnRadius, m_SpawnRadius),
                Random.Range(-m_SpawnRadius, m_SpawnRadius)
            );

            // Wait for a random time before spawning the next one
            float delay = Random.Range(m_CanonPowerupMinDelay, m_CanonPowerupMaxDelay);

            yield return new WaitForSecondsRealtime(delay);
            Vector2 spawnPosition = (Vector2)m_Player.transform.position + randomOffset;
            Instantiate(powerupPrefab, spawnPosition, Quaternion.identity);
        }
    }

    IEnumerator DropDodgePowerups(GameObject powerupPrefab, int count)
    {
        if (powerupPrefab == null || m_Player == null) yield break;
        for (int i = 0; i < count; i++)
        {
            Vector2 randomOffset = new Vector2(
                Random.Range(-m_SpawnRadius, m_SpawnRadius),
                Random.Range(-m_SpawnRadius, m_SpawnRadius)
            );

            // Wait for a random time before spawning the next one
            float delay = Random.Range(m_DodgePowerupMinDelay, m_DodgePowerupMaxDelay);

            yield return new WaitForSecondsRealtime(delay);
            Vector2 spawnPosition = (Vector2)m_Player.transform.position + randomOffset;
            Instantiate(powerupPrefab, spawnPosition, Quaternion.identity);
        }
    }
}
