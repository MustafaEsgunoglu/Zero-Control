using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] AudioClip audioClip;
    AudioSource audioSource;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>(); // Create an AudioSource
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 1f; // Make it 3D
        audioSource.volume = 0.2f; // Set volume
        audioSource.clip = audioClip; // Assign the audio clip
        PlayAudio();
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
