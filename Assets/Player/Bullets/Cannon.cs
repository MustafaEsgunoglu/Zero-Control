using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    [Header(" --- Cannon --- ")]
    [SerializeField] GameObject m_cannon;
    [SerializeField] float m_RotationSpeed = 180;
    [SerializeField] float m_ReloadSpeed = 0.5f;
    [SerializeField] bool m_TurnLeft = false;

    [Header(" --- Bullet --- ")]
    [SerializeField] GameObject m_BulletPrefab;
    [SerializeField] Transform m_GunPosition;
    [SerializeField] float m_BulletSpeed = 10;
    [SerializeField] float m_BulletDamage = 5;
    [SerializeField] int m_PoolSize = 10;

    [Header("---Refrences---")]
    [SerializeField] Animator m_Animator;

    float m_LastFiredAngle;
    List<GameObject> m_BulletPool;
    [SerializeField] AudioClip audioClip;
    AudioSource audioSource;

    public float GetRotationSpeed() { return m_RotationSpeed; }
    public void SetRotationSpeed(float RotationSpeed) { m_RotationSpeed = RotationSpeed; }
    public float GetBulletDamage() { return m_BulletDamage; }
    public void SetBulletDamage(float BulletDamage) { m_BulletDamage = BulletDamage; }
    public float GetBulletSpeed() { return m_BulletSpeed; }
    public void SetBulletSpeed(float BulletSpeed) { m_BulletSpeed = BulletSpeed; }
    public float GetReloadSpeed() { return m_ReloadSpeed; }
    public void SetReloadSpeed(float ReloadSpeed) { m_ReloadSpeed = ReloadSpeed; }
    public float GetLastFiredAngle() { return m_LastFiredAngle; }
    public void SetLastFiredAngle(float angle) { m_LastFiredAngle = angle; }


    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>(); // Create an AudioSource
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 1f; // Make it 3D
        audioSource.volume = 1f; // Set volume
        audioSource.clip = audioClip; // Assign the audio clip
        if (audioClip == null)
        {
            Debug.LogError("AudioClip is missing! Assign an AudioClip in the Inspector.");
        }
        else
        {
            audioSource.clip = audioClip;
        }
        m_LastFiredAngle = 0;
        InitializePool();
    }
    public void ResetLastFiredAngle()
    {
        m_LastFiredAngle = 0;
    }
    void FixedUpdate()
    {
        RotateCannon();
        CheckAndFire();
    }

    void InitializePool()
    {
        m_BulletPool = new List<GameObject>();
        for (int i = 0; i < m_PoolSize; i++)
        {
            GameObject bullet = Instantiate(m_BulletPrefab);
            bullet.SetActive(false);
            bullet.transform.localScale = new(2.5f, 2.5f, 2.5f);
            m_BulletPool.Add(bullet);
        }
    }

    void RotateCannon()
    {
        Vector3 direction = m_TurnLeft ? Vector3.forward : Vector3.back;
        m_cannon.transform.Rotate(m_RotationSpeed * Time.fixedDeltaTime * direction);
    }

    void CheckAndFire()
    {
        float reloadAngle = m_RotationSpeed * m_ReloadSpeed;
        float currentAngle = m_cannon.transform.eulerAngles.z;
        float angleDiff = Mathf.DeltaAngle(m_LastFiredAngle, currentAngle);

        if ((m_TurnLeft && angleDiff >= reloadAngle) || (!m_TurnLeft && angleDiff <= -reloadAngle))
        {
            while (Mathf.Abs(angleDiff) >= reloadAngle)
            {
                m_LastFiredAngle = (m_LastFiredAngle + (m_TurnLeft ? reloadAngle : -reloadAngle) + 360f) % 360f;
                angleDiff = Mathf.DeltaAngle(m_LastFiredAngle, currentAngle);
                Fire();
            }
        }
    }

    void Fire()
    {
        StartCoroutine(Animate());
        GameObject bullet = GetBulletFromPool();
        if (bullet == null) return;

        bullet.transform.position = m_GunPosition.position;
        bullet.transform.rotation = Quaternion.Euler(0, 0, m_LastFiredAngle);
        bullet.GetComponent<PlayerBullet>().SetBulletDamage(m_BulletDamage);
        bullet.SetActive(true);
        PlayAudio();

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        float rad = m_LastFiredAngle * Mathf.Deg2Rad;
        Vector2 bulletDirection = new(Mathf.Cos(rad), Mathf.Sin(rad));
        rb.linearVelocity = bulletDirection * m_BulletSpeed;

    }

    GameObject GetBulletFromPool()
    {
        foreach (GameObject bullet in m_BulletPool)
        {
            if (!bullet.activeInHierarchy)
            {
                return bullet;
            }
        }
        return null; // No available bullet
    }

    public void ReturnBulletToPool(GameObject bullet)
    {
        bullet.SetActive(false);
    }
    IEnumerator Animate()
    {

        m_Animator.SetBool("isFiring", true);
        yield return new WaitForSeconds(0.125f);
        m_Animator.SetBool("isFiring", false);

    }
    public void PlayAudio()
    {
        if (audioSource == null || audioClip == null)
        {
            Debug.LogError("AudioSource or AudioClip is missing!");
            return;
        }

        audioSource.volume = 2;
        audioSource.pitch = Random.Range(0.9f, 1.1f);
        audioSource.PlayOneShot(audioClip); // Doesn't interrupt other sounds
    }
}
