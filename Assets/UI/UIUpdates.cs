using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIUpdates : MonoBehaviour
{
    [SerializeField] Image m_HealthBar;
    [SerializeField] Image m_TimerBar;
    [SerializeField] TextMeshProUGUI[] m_Money;
    [SerializeField] Button m_Button;

    Health m_PlayerHealth;
    PlayerMoney m_PlayerMoney;
    float m_MaximumHealth;
    float m_TimeLimit = 0;
    float m_Timer = 0;
    
    
    void Start()
    {
        m_PlayerHealth = FindAnyObjectByType<Health>();
        m_PlayerMoney = FindAnyObjectByType<PlayerMoney>();

        m_MaximumHealth = m_PlayerHealth.GetHp();
        m_TimeLimit = GetComponent<LevelManager>().GetTimer();
        
    }

    void FixedUpdate()
    {
        if (m_HealthBar == null || m_TimerBar == null)
        {
            return;
        }
        else if (m_TimeLimit < 1)
        {
            Debug.Log("Put a timer on LevelManager.cs");
            return;
        }

        // Update health bar
        float currentHealth = m_PlayerHealth.GetHp();
        float healthPercent = currentHealth / m_MaximumHealth;
        m_HealthBar.fillAmount = healthPercent;

        // Update timer bar
        m_Timer += Time.fixedDeltaTime;
        float timerPercent = m_Timer / m_TimeLimit;
        m_TimerBar.fillAmount = timerPercent;

        if (m_Timer >= m_TimeLimit - 10 )
        {
            
        }

        // Update money text
        m_Money[0].SetText(m_PlayerMoney.GetHealthMoney() + "");
        m_Money[1].SetText(m_PlayerMoney.GetEngineMoney() + "");
        m_Money[2].SetText(m_PlayerMoney.GetCannonMoney() + "");
    }

    

    public void MainMenuButton()
    {
        LevelManager.SetCurrentSceneIndex(0);
        SceneManager.LoadScene("Main Menu");
    }
}
