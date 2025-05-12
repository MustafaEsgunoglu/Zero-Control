using UnityEngine;
using UnityEngine.SceneManagement;

public class StartAndExit : MonoBehaviour
{
    [SerializeField] int FirstLevel;
    public void StartButton()
    {
        LevelManager.SetCurrentSceneIndex(0);
        GetComponent<PlayerIO>().WriteFreshPlayer();
        SceneManager.LoadScene("StartCutScene");
    }
    public void ExitButton()
    {
        Application.Quit();
    }
}
