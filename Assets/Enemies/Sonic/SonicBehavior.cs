using System.Collections;
using UnityEngine;
using Pathfinding;

public class SonicBehavior : MonoBehaviour
{
    [Header(" --- References --- ")]
    [SerializeField] Transform m_Cannon;
    [SerializeField] Transform m_GunPoint;

    [Header(" --- Cannon Settings --- ")]
    [SerializeField] float m_BulletSpeed = 10f;
    [SerializeField] float m_BulletDamage = 10f;
    [SerializeField] float m_FireTime = 2f;
    [SerializeField] float m_CircleTime = 3f;

    EnemyBulletPool enemyBulletPool;
    Transform m_Player;
    AIPath m_AiPath;
    bool m_IsAiming;
    bool m_IsFiring;
    bool m_IsCircling;
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
        enemyBulletPool = FindAnyObjectByType<EnemyBulletPool>();
        m_Player = FindAnyObjectByType<PlayerMover>().transform;
        m_AiPath = GetComponent<AIPath>();
    }

    void FixedUpdate()
    {
        if (m_IsAiming)
        {
            AimAtPlayer();
        }
    }

    void AimAtPlayer()
    {
        Vector2 aimDirection = (m_Player.position - m_Cannon.position).normalized;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        m_Cannon.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("SonicTarget"))
        {
            if (m_IsFiring || m_IsCircling)
            { return; }

            StartCoroutine(StartFiring());
        }
    }

    IEnumerator StartFiring()
    {
        m_IsFiring = true;
        m_IsCircling = true;
        m_IsAiming = true;

        // Stop AI movement
        m_AiPath.isStopped = true;
        m_AiPath.SetPath(null);

        // Start firing animation
        PlayAudio();
        yield return new WaitForSeconds(m_FireTime);
        
        animator.SetBool("isFiring", true);
        // Fire the bullet
        Fire();

        m_IsFiring = false;

        // Resume AI movement
        m_AiPath.isStopped = false;

        // Circling phase (adds some randomness)
        float circlingTime = m_CircleTime + Random.Range(-1.0f, 1.0f);
        yield return new WaitForSeconds(circlingTime);
        
        // Stop firing animation
        animator.SetBool("isFiring", false);

        m_IsCircling = false;
    }

    void Fire()
    {
        Vector2 fireDirection = (m_Player.position - m_Cannon.position).normalized;

        // Get bullet from pool
        GameObject bullet = enemyBulletPool.GetBulletFromPool();
        if (bullet != null)
        {
            bullet.transform.position = m_GunPoint.position;
            bullet.transform.rotation = Quaternion.identity;

            // Set bullet damage
            EnemyBullet enemyBullet = bullet.GetComponent<EnemyBullet>();
            enemyBullet.SetBulletDamage(m_BulletDamage);

            // Set bullet speed
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.linearVelocity = fireDirection * m_BulletSpeed;
        }
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
