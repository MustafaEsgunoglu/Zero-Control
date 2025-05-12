using System.Collections;
using UnityEngine;

public class ExplosionVFX : MonoBehaviour
{
    [SerializeField] AudioClip audioClip;
    AudioSource audioSource;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>(); // Create an AudioSource
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 1f; // Make it 3D
        audioSource.volume = 1f; // Set volume
        audioSource.clip = audioClip; // Assign the audio clip
        AudioSource.PlayClipAtPoint(audioClip, transform.position);

        StartCoroutine(Explode());
    }
    IEnumerator Explode()
    {
        yield return new WaitForSecondsRealtime(2f);
        Destroy(gameObject); 
    }
    /*public void Explosion() 
    {
        if (animator != null)
        {
            animator.SetTrigger(ExplosionTrigger);
        }
        else
        {
            Debug.LogWarning("Animator is not assigned or found!");
        }
    } */
}
