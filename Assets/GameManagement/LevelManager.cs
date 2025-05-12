using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelManager : MonoBehaviour
{
    [SerializeField] float m_Timer = 60f; // Time limit in seconds
    readonly string[] m_Scenes = { "Level 1", "UpgradeShop", "Level 2", "UpgradeShop", "Lv2CutScene", /*"Level 3",*/ "UpgradeShop", "Level 4", "UpgradeShop", "Lv4CutScene", /*"Level 5",*/ "UpgradeShop", "Level 6", "UpgradeShop", "Lv6CutScene", /*"Level 7",*/ "UpgradeShop", "Level 8", "UpgradeShop", "Lv8CutScene", /*"Level 9",*/ "UpgradeShop", "Level 10", "EndScene"};

    // Use a static variable to persist the current scene index across scene loads
    public static int currentSceneIndex = 0;
    public bool died = false;

    public float GetTimer()
    {
        return m_Timer;
    }

    public static void SetCurrentSceneIndex(int index)
    {
        currentSceneIndex = index;
    }

    void Start()
    {
        if(m_Scenes.Length <= currentSceneIndex)
        {
            currentSceneIndex = 0;
        }
        // Only start the timer in non-shop and non-cutscene scenes.
        if (!IsShopScene() && !IsCutScene())
        {
            StartCoroutine(StartTimer());
        }

    }

    IEnumerator StartTimer()
    {
        yield return new WaitForSeconds(m_Timer);
        LoadNextScene();
    }

    public void LoadNextScene()
    {
        if(died)
        {return ;}

        currentSceneIndex++;
        if (currentSceneIndex >= m_Scenes.Length)
        {
            currentSceneIndex = 0; // Restart or handle end-of-game appropriately
        }

        // Save any necessary player data here
        GetComponent<PlayerIO>().WriteFreshPlayer();
        GetComponent<PlayerIO>().WritePlayer();

        SceneManager.LoadScene(m_Scenes[currentSceneIndex]);
    }

    bool IsShopScene()
    {
        return SceneManager.GetActiveScene().name == "UpgradeShop";
    }

    bool IsCutScene()
    {
        return SceneManager.GetActiveScene().name.Contains("CutScene");
    }
}
