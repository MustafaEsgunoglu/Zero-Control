using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelTimer : MonoBehaviour
{
    [SerializeField] float m_TimeLimit = 60f; // Time limit in seconds
    [SerializeField] bool m_ShopScene = false;

    void Start()
    {
        if(m_ShopScene)
        {
            Debug.Log("You are in shop scene!");
        }
        else
        {
            StartCoroutine(StartTimer());
        }
    }

    IEnumerator StartTimer()
    {
        yield return new WaitForSeconds(m_TimeLimit);
        Debug.Log("New scene loading.");
    }
}
