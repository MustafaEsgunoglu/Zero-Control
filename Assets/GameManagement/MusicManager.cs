using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMusicManager : MonoBehaviour
{
    [SerializeField] private AudioClip gameMusic;  // Assign in Inspector
    [SerializeField] private AudioClip shopMusic;  // Assign in Inspector
    private AudioSource audioSource;
    [SerializeField] private float gameMusicVolume = 0.5f; // Set volume for game music
    [SerializeField] private float shopMusicVolume = 0.5f; // Set volume for shop music
    private string lastSceneName = "";

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        audioSource = GetComponent<AudioSource>() ?? gameObject.AddComponent<AudioSource>();

        SceneManager.sceneLoaded += OnSceneChanged;
        PlayMusic();
    }

    void OnSceneChanged(Scene scene, LoadSceneMode mode)
    {
        PlayMusic();
    }

    void PlayMusic()
    {
        string currentScene = SceneManager.GetActiveScene().name;

        if (currentScene != lastSceneName)
        {
            lastSceneName = currentScene;
            AudioClip newClip = (currentScene == "UpgradeShop") ? shopMusic : gameMusic;

            if(audioSource == null) {return;}
            if (audioSource.clip != newClip)
            {
                audioSource.Stop();
                audioSource.clip = newClip;
                audioSource.loop = (currentScene == "UpgradeShop"); // Loop only shop music
                audioSource.Play();
            }
        }
    }
}
