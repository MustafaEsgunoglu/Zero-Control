using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeShop : MonoBehaviour
{
    [Header(" --- The Money --- ")]
    [SerializeField] int m_Cost;
    [SerializeField] TextMeshProUGUI[] m_TextMoney;

    [Header(" --- The Fill Bar --- ")]
    [SerializeField] Image[] m_Bar;

    [Header(" --- The Buttons --- ")]
    [SerializeField] Button m_StartButton;
    [SerializeField] Button[] m_BuyButtons;
    // Button upHp;
    // Button upConsumableHp;
    // Button upDodgeupChance;
    // Button upEngineSpeed;
    // Button upEngineRotateSpeed;
    // Button upEngineBreaks;
    // Button upRelodeSpeed;
    // Button upBulletSpeed;
    // Button upBulletDamage;

    [SerializeField] int[] m_UpgradeBought; // Should have 9 elements.
    public Sprite[] m_ShopShips;
    public Image[] m_ShopShipParts;

    GameObject m_Player;
    PlayerMoney m_PlayerMoney;
    [SerializeField] AudioClip audioClip;
    AudioSource audioSource;

    public int[] GetUpgradeBought() => m_UpgradeBought;
    public void SetUpgradeBought(int[] value) { m_UpgradeBought = value; }

    void Start()
    {
        m_Player = FindAnyObjectByType<PlayerMoney>().gameObject;
        m_PlayerMoney = m_Player.GetComponent<PlayerMoney>();
        audioSource = gameObject.AddComponent<AudioSource>(); // Create an AudioSource
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 1f; // Make it 3D
        audioSource.volume = 1f; // Set volume
        audioSource.clip = audioClip; // Assign the audio clip

        if (m_BuyButtons == null || m_StartButton == null || m_TextMoney == null || m_Bar == null)
        {
            Debug.Log("We're not in the shop scene");
        }
        else
        {
            UpdateButtons();
        }
    }

    void UpdateButtons()
    {
        // Create a cost array for each upgrade.
        int[] upgradeCost = new int[9];
        for (int i = 0; i < 9; i++)
        {
            if (m_UpgradeBought[i] < 10)
                upgradeCost[i] = (m_UpgradeBought[i] + 1) * m_Cost;
            else
                upgradeCost[i] = -1; // Using -1 to indicate this upgrade is maxed out.
        }

        int healthMoney = m_PlayerMoney.GetHealthMoney();
        int engineMoney = m_PlayerMoney.GetEngineMoney();
        int cannonMoney = m_PlayerMoney.GetCannonMoney();

        // Update health upgrade buttons (indices 0-2)
        for (int i = 0; i < 3; i++)
        {
            if (upgradeCost[i] != -1)
            {
                m_BuyButtons[i].interactable = healthMoney >= upgradeCost[i];
                m_BuyButtons[i].GetComponentInChildren<TextMeshProUGUI>().SetText(upgradeCost[i].ToString());
            }
            else
            {
                m_BuyButtons[i].interactable = false;
                m_BuyButtons[i].GetComponentInChildren<TextMeshProUGUI>().SetText("max");
            }
        }

        // Update engine upgrade buttons (indices 3-5)
        for (int i = 3; i < 6; i++)
        {
            if (upgradeCost[i] != -1)
            {
                m_BuyButtons[i].interactable = engineMoney >= upgradeCost[i];
                m_BuyButtons[i].GetComponentInChildren<TextMeshProUGUI>().SetText(upgradeCost[i].ToString());
            }
            else
            {
                m_BuyButtons[i].interactable = false;
                m_BuyButtons[i].GetComponentInChildren<TextMeshProUGUI>().SetText("max");
            }
        }

        // Update cannon upgrade buttons (indices 6-8)
        for (int i = 6; i < 9; i++)
        {
            if (upgradeCost[i] != -1)
            {
                m_BuyButtons[i].interactable = cannonMoney >= upgradeCost[i];
                m_BuyButtons[i].GetComponentInChildren<TextMeshProUGUI>().SetText(upgradeCost[i].ToString());
            }
            else
            {
                m_BuyButtons[i].interactable = false;
                m_BuyButtons[i].GetComponentInChildren<TextMeshProUGUI>().SetText("max");
            }
        }

        // Set current money texts
        m_TextMoney[0].SetText(healthMoney.ToString());
        m_TextMoney[1].SetText(engineMoney.ToString());
        m_TextMoney[2].SetText(cannonMoney.ToString());

        // Update fill image bars for each upgrade
        for (int i = 0; i < m_Bar.Length; i++)
        {
            m_Bar[i].fillAmount = Mathf.Clamp(m_UpgradeBought[i] * 0.1f, 0, 1);
        }

        GetComponent<PlayerIO>().ChangeSkin(this);

        PlayAudio();
    }

    public void StartButton()
    {
        LevelManager levelManager = GetComponent<LevelManager>();
        ResetPlayerStats();

        levelManager.LoadNextScene();
    }

    private void ResetPlayerStats()
    {
        int hp = 20 + (m_UpgradeBought[0] * 10);
        m_Player.GetComponent<Health>().SetHp(hp);

        int consumable = 3 + (m_UpgradeBought[1] * 1);
        m_Player.GetComponent<Health>().SetConsumableHp(consumable);

        int dodge = 5 + (m_UpgradeBought[2] * 2);
        m_Player.GetComponent<Health>().SetDodgeChance(dodge);

        int thrustForce = 9 + (m_UpgradeBought[3] * 1);
        int maxSpeed = 5 + (m_UpgradeBought[3] * 1);
        m_Player.GetComponent<PlayerMover>().SetThrustForce(thrustForce);
        m_Player.GetComponent<PlayerMover>().SetMaxSpeed(maxSpeed);

        int cannonRotationSpeed = 175 + (m_UpgradeBought[4] * 30);
        m_Player.GetComponent<PlayerMover>().SetRotationSpeed(cannonRotationSpeed);

        float breaks = 0.1f + (m_UpgradeBought[5] * 0.1f);
        m_Player.GetComponent<PlayerMover>().SetStopDamping(breaks);

        if (m_UpgradeBought[6] < 3)
        {
            float rotationSpeed = 90;
            float reloadSpeed = 1;
            m_Player.GetComponent<Cannon>().SetRotationSpeed(rotationSpeed);
            m_Player.GetComponent<Cannon>().SetReloadSpeed(reloadSpeed);
        }
        else if (m_UpgradeBought[6] >= 3 && m_UpgradeBought[6] < 6)
        {
            float rotationSpeed = 180;
            float reloadSpeed = 0.5f;
            m_Player.GetComponent<Cannon>().SetRotationSpeed(rotationSpeed);
            m_Player.GetComponent<Cannon>().SetReloadSpeed(reloadSpeed);
        }
        else if (m_UpgradeBought[6] >= 6 && m_UpgradeBought[6] < 10)
        {
            float rotationSpeed = 360;
            float reloadSpeed = 0.25f;
            m_Player.GetComponent<Cannon>().SetRotationSpeed(rotationSpeed);
            m_Player.GetComponent<Cannon>().SetReloadSpeed(reloadSpeed);
        }
        else if (m_UpgradeBought[6] == 10)
        {
            float rotationSpeed = 360;
            float reloadSpeed = 0.125f;
            m_Player.GetComponent<Cannon>().SetRotationSpeed(rotationSpeed);
            m_Player.GetComponent<Cannon>().SetReloadSpeed(reloadSpeed);
        }

        int bulletSpeed = 10 + (m_UpgradeBought[7] * 2);
        m_Player.GetComponent<Cannon>().SetBulletSpeed(bulletSpeed);

        int bulletDamage = 3 + (m_UpgradeBought[8] * 2);
        m_Player.GetComponent<Cannon>().SetBulletDamage(bulletDamage);
    }

    // Health upgrades
    public void UpgradeHp()
    {
        if (m_UpgradeBought[0] >= 10) return;
        
        int cost = (m_UpgradeBought[0] + 1) * m_Cost;
        m_PlayerMoney.SetHealthMoney(m_PlayerMoney.GetHealthMoney() - cost);
        m_UpgradeBought[0]++;

        UpdateButtons();
        int hp = 20 + (m_UpgradeBought[0] * 10);
        m_Player.GetComponent<Health>().SetHp(hp);
    }

    public void UpgradeConsumableHp()
    {
        if (m_UpgradeBought[1] >= 10) return;
        int cost = (m_UpgradeBought[1] + 1) * m_Cost;
        m_PlayerMoney.SetHealthMoney(m_PlayerMoney.GetHealthMoney() - cost);
        m_UpgradeBought[1]++;
        UpdateButtons();
        int consumable = 3 + (m_UpgradeBought[1] * 1);
        m_Player.GetComponent<Health>().SetConsumableHp(consumable);
    }

    public void UpgradeDodgeChance()
    {
        if (m_UpgradeBought[2] >= 10) return;
        int cost = (m_UpgradeBought[2] + 1) * m_Cost;
        m_PlayerMoney.SetHealthMoney(m_PlayerMoney.GetHealthMoney() - cost);
        m_UpgradeBought[2]++;
        UpdateButtons();
        int dodge = 5 + (m_UpgradeBought[2] * 2);
        m_Player.GetComponent<Health>().SetDodgeChance(dodge);
    }

    // Engine upgrades
    public void UpgradeEngineSpeed()
    {
        if (m_UpgradeBought[3] >= 10) return;
        int cost = (m_UpgradeBought[3] + 1) * m_Cost;
        m_PlayerMoney.SetEngineMoney(m_PlayerMoney.GetEngineMoney() - cost);
        m_UpgradeBought[3]++;
        UpdateButtons();
        int thrustForce = 9 + (m_UpgradeBought[3] * 1);
        int maxSpeed = 5 + (m_UpgradeBought[3] * 1);
        m_Player.GetComponent<PlayerMover>().SetThrustForce(thrustForce);
        m_Player.GetComponent<PlayerMover>().SetMaxSpeed(maxSpeed);
    }

    public void UpgradeEngineRotationSpeed()
    {
        if (m_UpgradeBought[4] >= 10) return;
        int cost = (m_UpgradeBought[4] + 1) * m_Cost;
        m_PlayerMoney.SetEngineMoney(m_PlayerMoney.GetEngineMoney() - cost);
        m_UpgradeBought[4]++;
        UpdateButtons();
        int rotationSpeed = 175 + (m_UpgradeBought[4] * 30);
        m_Player.GetComponent<PlayerMover>().SetRotationSpeed(rotationSpeed);
    }

    public void UpgradeBreaks()
    {
        if (m_UpgradeBought[5] >= 10) return;
        int cost = (m_UpgradeBought[5] + 1) * m_Cost;
        m_PlayerMoney.SetEngineMoney(m_PlayerMoney.GetEngineMoney() - cost);
        m_UpgradeBought[5]++;
        UpdateButtons();
        float breaks = 0.1f + (m_UpgradeBought[5] * 0.1f);
        m_Player.GetComponent<PlayerMover>().SetStopDamping(breaks);
    }

    // Cannon upgrades
    public void UpgradeReloadSpeed()
    {
        if (m_UpgradeBought[6] >= 10) return;
        int cost = (m_UpgradeBought[6] + 1) * m_Cost;
        m_PlayerMoney.SetCannonMoney(m_PlayerMoney.GetCannonMoney() - cost);
        m_UpgradeBought[6]++;
        UpdateButtons();

        if (m_UpgradeBought[6] < 3)
        {
            float rotationSpeed = 90;
            float reloadSpeed = 1;
            m_Player.GetComponent<Cannon>().SetRotationSpeed(rotationSpeed);
            m_Player.GetComponent<Cannon>().SetReloadSpeed(reloadSpeed);
        }
        else if (m_UpgradeBought[6] >= 3 && m_UpgradeBought[6] < 6)
        {
            float rotationSpeed = 180;
            float reloadSpeed = 0.5f;
            m_Player.GetComponent<Cannon>().SetRotationSpeed(rotationSpeed);
            m_Player.GetComponent<Cannon>().SetReloadSpeed(reloadSpeed);
        }
        else if (m_UpgradeBought[6] >= 6 && m_UpgradeBought[6] < 10)
        {
            float rotationSpeed = 360;
            float reloadSpeed = 0.25f;
            m_Player.GetComponent<Cannon>().SetRotationSpeed(rotationSpeed);
            m_Player.GetComponent<Cannon>().SetReloadSpeed(reloadSpeed);
        }
        else if (m_UpgradeBought[6] == 10)
        {
            float rotationSpeed = 360;
            float reloadSpeed = 0.125f;
            m_Player.GetComponent<Cannon>().SetRotationSpeed(rotationSpeed);
            m_Player.GetComponent<Cannon>().SetReloadSpeed(reloadSpeed);
        }
    }

    public void UpgradeBulletSpeed()
    {
        if (m_UpgradeBought[7] >= 10) return;
        int cost = (m_UpgradeBought[7] + 1) * m_Cost;
        m_PlayerMoney.SetCannonMoney(m_PlayerMoney.GetCannonMoney() - cost);
        m_UpgradeBought[7]++;
        UpdateButtons();
        int bulletSpeed = 10 + (m_UpgradeBought[7] * 2);
        m_Player.GetComponent<Cannon>().SetBulletSpeed(bulletSpeed);
    }

    public void UpgradeBulletDamage()
    {
        if (m_UpgradeBought[8] >= 10) return;
        int cost = (m_UpgradeBought[8] + 1) * m_Cost;
        m_PlayerMoney.SetCannonMoney(m_PlayerMoney.GetCannonMoney() - cost);
        m_UpgradeBought[8]++;
        UpdateButtons();
        int bulletDamage = 3 + (m_UpgradeBought[8] * 2);
        m_Player.GetComponent<Cannon>().SetBulletDamage(bulletDamage);
    }

    public void ChangeSkin()
    {
        // Implementation for changing skin if necessary.
    }

    public void PlayAudio()
    {
        if (audioSource == null || audioSource.clip == null)
        {
            Debug.LogError("AudioSource or AudioClip is missing!");
            return;
        }
        audioSource.pitch = Random.Range(0.9f, 1.1f);
        audioSource.Play();
    }
}
