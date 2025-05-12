using System.Collections;
using UnityEngine;
using Pathfinding;

public class BulldozerBehavior : MonoBehaviour
{
    [Header(" --- References --- ")]
    [SerializeField] Transform m_Cannon;
    [SerializeField] Transform m_GunPoint;

    [Header(" --- Cannon Settings --- ")]
    [SerializeField] float m_BulletSpeed = 10f;
    [SerializeField] float m_BulletDamage = 10f;
    [SerializeField] float m_ReloadSpeed = 5f;
    [SerializeField] float m_FireDistance = 30f;
    [SerializeField] float m_StopDistance = 5f;

    EnemyBulletPool enemyBulletPool;
    Transform m_Player;
    AIPath m_AiPath;
    bool m_IsFiring;
    Animator animator;
    [SerializeField] AudioClip audioClip;
    AudioSource audioSource;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>(); // Create an AudioSource
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 1f; // Make it 3D
        audioSource.volume = 1f; // Set volume
        audioSource.clip = audioClip; // Assign the audio clip
        animator = GetComponentInChildren<Animator>();
        m_Player = FindAnyObjectByType<PlayerMover>().transform;
        m_AiPath = GetComponent<AIPath>();
        enemyBulletPool = FindAnyObjectByType<EnemyBulletPool>();
    }

    void FixedUpdate()
    {
        if (m_Player == null) { return; }

        float distanceToPlayer = Vector2.Distance(m_Player.position, transform.position);

        if (distanceToPlayer <= m_FireDistance)
        {
            if (!m_IsFiring)
            {
                StartCoroutine(FireCoroutine());
            }

            if (distanceToPlayer <= m_StopDistance)
            {
                if (!m_AiPath.isStopped)
                {
                    m_AiPath.isStopped = true;
                    m_AiPath.SetPath(null);
                }
            }
            else if (m_AiPath.isStopped)
            {
                m_AiPath.isStopped = false;
            }
        }
        else
        {
            if (m_AiPath.isStopped)
            {
                m_AiPath.isStopped = false;
            }
        }

        AimAtPlayer();
    }

    void AimAtPlayer()
    {
        Vector2 aimDirection = (m_Player.position - m_Cannon.position).normalized;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        m_Cannon.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    IEnumerator FireCoroutine()
    {
        m_IsFiring = true;
        animator.SetBool("isFiring", true); // Start animation
        PlayAudio();
        yield return new WaitForSeconds(0.3f); // Small delay to ensure animation starts

        Vector2 fireDirection = (m_Player.position - m_Cannon.position).normalized;

        GameObject bullet = enemyBulletPool.GetBulletFromPool();
        if (bullet != null)
        {
            bullet.transform.position = m_GunPoint.position;
            bullet.transform.rotation = Quaternion.identity;

            EnemyBullet enemyBullet = bullet.GetComponent<EnemyBullet>();
            enemyBullet.SetBulletDamage(m_BulletDamage);

            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.linearVelocity = fireDirection * m_BulletSpeed;
            bullet.transform.localScale = new (3,3,3);
        }

        yield return new WaitForSeconds(m_ReloadSpeed);

        animator.SetBool("isFiring", false); // Stop animation

        m_IsFiring = false;
    }

    public void PlayAudio()
    {
        if (audioSource == null || audioSource.clip == null)
        {
            Debug.LogError("AudioSource or AudioClip is missing!");
            return;
        }
        audioSource.pitch = Random.Range (0.9f, 1.1f);
        audioSource.Play(); // Play the sound from the enemy
    }
}