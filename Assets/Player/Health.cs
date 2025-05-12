using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    [Header(" --- Health Point Stats --- ")]
    [SerializeField] float m_Hp = 100;
    [SerializeField] float m_CrashDamage = 2;
    [SerializeField] float m_ConsumableHp = 5;
    [SerializeField] float m_DodgeChance = 0;
    float m_MaxHp;
    [SerializeField] ExplosionVFX explosion;
    [SerializeField] AudioClip audioClip;
    AudioSource audioSource;
    [SerializeField] AudioClip[] audioClips; // Array to store the 5 audio clips
    AudioSource audioSource2;

    // Getter and Setters.
    public float GetMaxHp()
    { return m_MaxHp; }
    public float GetHp()
    { return m_Hp; }
    public void SetHp(float hp)
    { m_Hp = hp; }

    public float GetConsumableHp()
    { return m_ConsumableHp; }
    public void SetConsumableHp(float consumableHp)
    { m_ConsumableHp = consumableHp; }

    public float GetDodgeChance()
    { return m_DodgeChance; }
    public void SetDodgeChance(float dodgeChance)
    { m_DodgeChance = dodgeChance; }


    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>(); // Create an AudioSource
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 1f; // Make it 3D
        audioSource.volume = 1f; // Set volume
        audioSource.clip = audioClip; // Assign the audio clip
        audioSource2 = GetComponent<AudioSource>(); 
        m_MaxHp = m_Hp;
    }

    void Update()
    {
        if (m_Hp <= 0)
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
            
            AudioSource.PlayClipAtPoint(audioClip, transform.position);
            gameObject.SetActive(false);

            FindAnyObjectByType<LevelManager>().died = true;
            Invoke("RestartLevel", 0.5f);
        }
    }

    void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("EnemyBullet"))
        {
            float damage = other.gameObject.GetComponent<EnemyBullet>().GetBulletDamage();
            Dodge(damage);
        }
        else if (other.gameObject.CompareTag("Kamikaze"))
        {
            float damage = other.gameObject.GetComponent<KamikazeBehavior>().GetDamage();
            Dodge(damage);
        }
        else
        {
            Dodge(m_CrashDamage);
        }
    }

    void Dodge(float damage)
    {
        float chance;
        if (m_DodgeChance > 0)
        {
            chance = Random.Range(0, 100);
            if (chance > m_DodgeChance)
            {
                m_Hp -= damage;
                PlayRandomAudio();
            }
        }
        else
        {
            m_Hp -= damage;
            PlayRandomAudio();
        }
    }

    public void PlayAudio()
    {
        if (audioSource == null || audioSource.clip == null)
        {
            Debug.LogError("AudioSource or AudioClip is missing!");
            return;
        }
        audioSource.pitch = Random.Range(0.9f, 1.1f);
        audioSource.Play(); // Play the sound from the enemy
    }
    public void PlayRandomAudio()
    {
        if (audioClips.Length == 0 || audioSource == null)
        {
            Debug.LogError("No audio clips assigned or AudioSource is missing!");
            return;
        }

        AudioClip randomClip = audioClips[Random.Range(0, audioClips.Length)]; // Pick a random clip
        audioSource.PlayOneShot(randomClip);
    }

}
