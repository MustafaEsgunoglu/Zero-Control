using UnityEngine;

[System.Serializable]
public class PlayerData
{

    [Header(" --- Health --- ")]
    public float m_HP;
    public float m_ConsumableHP;
    public float m_DodgeChance;

    [Header(" --- Cannon --- ")]
    public float m_RotationSpeed;
    public float m_ReloadSpeed;
    public float m_BulletSpeed;
    public float m_BulletDamage;

    [Header(" --- Engine --- ")]
    public float m_ThrustForce;
    public float m_MaxSpeed;
    public float m_EngineRotationSpeed;
    public float m_StopDamping;

    [Header(" --- Money --- ")]
    public int m_HealthMoney;
    public int m_EngineMoney;
    public int m_CanonMoney;
    public int[] m_Upgrades;


    public PlayerData(GameObject player)
    {
        Health health = player.GetComponent<Health>();
        PlayerMover engine = player.GetComponent<PlayerMover>();
        Cannon cannon = player.GetComponent<Cannon>();
        PlayerMoney playerMoney = player.GetComponent<PlayerMoney>();
        UpgradeShop upgradeShop = GameObject.FindAnyObjectByType<UpgradeShop>();

        m_HP = health.GetHp();
        m_ConsumableHP = health.GetConsumableHp();
        m_DodgeChance = health.GetDodgeChance();

        m_ThrustForce = engine.GetThrustForce();
        m_MaxSpeed = engine.GetMaxSpeed();
        m_EngineRotationSpeed = engine.GetRotationSpeed();
        m_StopDamping = engine.GetStopDamping();

        m_RotationSpeed = cannon.GetRotationSpeed();
        m_ReloadSpeed = cannon.GetReloadSpeed();
        m_BulletSpeed = cannon.GetBulletSpeed();
        m_BulletDamage = cannon.GetBulletDamage();

        m_HealthMoney = playerMoney.GetHealthMoney();
        m_EngineMoney = playerMoney.GetEngineMoney();
        m_CanonMoney = playerMoney.GetCannonMoney();

        Debug.Log("I'm here");
        m_Upgrades = upgradeShop.GetUpgradeBought();
    }

    public PlayerData()
    {
        m_HP = 20;
        m_ConsumableHP = 3;
        m_DodgeChance = 5;

        m_ThrustForce = 7;
        m_MaxSpeed = 7;
        m_EngineRotationSpeed = 175;
        m_StopDamping = 0.1f;

        m_RotationSpeed = 90;
        m_ReloadSpeed = 1;
        m_BulletSpeed = 10;
        m_BulletDamage = 3;

        m_HealthMoney = 0;
        m_EngineMoney = 0;
        m_CanonMoney = 0;

        m_Upgrades = new int[] {0,0,0,0,0,0,0,0,0};
    }
}
