using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class PlayerIO : MonoBehaviour
{
    GameObject m_Player;

    void Start()
    {
        m_Player = FindAnyObjectByType<PlayerMover>().gameObject;

        PlayerData loadedData = ReadPlayer();    // Read file code will be put here and will change IsInitialized to true;
        if (loadedData != null)
        {
            InitializePlayer(loadedData);
        }
        else
        {
            Debug.Log("No player data file found.");
        }
    }

    public void WriteFreshPlayer()
    {
        BinaryFormatter formatter = new();
        string path = Application.persistentDataPath + "/player.pipi";
        FileStream stream = new(path, FileMode.Create);

        PlayerData playerdata = new();

        formatter.Serialize(stream, playerdata);
        stream.Close();
    }

    public void WritePlayer()
    {
        BinaryFormatter formatter = new();
        string path = Application.persistentDataPath + "/player.pipi";
        FileStream stream = new(path, FileMode.Create);

        PlayerData playerdata = new(m_Player);

        formatter.Serialize(stream, playerdata);
        stream.Close();
    }

    public PlayerData ReadPlayer()
    {
        string path = Application.persistentDataPath + "/player.pipi";
        if (File.Exists(path))
        {
            using (FileStream stream = new FileStream(path, FileMode.Open))
            {
                // Check if the stream has any data.
                if (stream.Length == 0)
                {
                    Debug.LogWarning("The player data file is empty.");
                    return null;
                }

                BinaryFormatter formatter = new BinaryFormatter();
                PlayerData playerdata = (PlayerData)formatter.Deserialize(stream);
                return playerdata;
            }
        }
        else
        {
            return null;
        }
    }

    void InitializePlayer(PlayerData playerData)
    {
        Health health = m_Player.GetComponent<Health>();
        if (health != null)
        {
            health.SetHp(playerData.m_HP);
            health.SetConsumableHp(playerData.m_ConsumableHP);
            health.SetDodgeChance(playerData.m_DodgeChance);
        }
        else
        {
            Debug.LogError("Health component not found!");
        }

        Cannon cannon = m_Player.GetComponent<Cannon>();
        if (cannon != null)
        {
            cannon.SetRotationSpeed(playerData.m_RotationSpeed);
            cannon.SetReloadSpeed(playerData.m_ReloadSpeed);
            cannon.SetBulletSpeed(playerData.m_BulletSpeed);
            cannon.SetBulletDamage(playerData.m_BulletDamage);
        }
        else
        {
            Debug.LogError("Cannon component not found!");
        }

        PlayerMover playerMover = m_Player.GetComponent<PlayerMover>();
        if (playerMover != null)
        {
            playerMover.SetThrustForce(playerData.m_ThrustForce);
            playerMover.SetMaxSpeed(playerData.m_MaxSpeed);
            playerMover.SetRotationSpeed(playerData.m_EngineRotationSpeed);
            playerMover.SetStopDamping(playerData.m_StopDamping);
        }
        else
        {
            Debug.LogError("PlayerMover component not found!");
        }

        PlayerMoney playerMoney = m_Player.GetComponent<PlayerMoney>();
        if (playerMoney != null)
        {
            playerMoney.SetHealthMoney(playerData.m_HealthMoney);
            playerMoney.SetEngineMoney(playerData.m_EngineMoney);
            playerMoney.SetCannonMoney(playerData.m_CanonMoney);
        }

        UpgradeShop upgrade = FindAnyObjectByType<UpgradeShop>();
        if (upgrade != null)
        {
            upgrade.SetUpgradeBought(playerData.m_Upgrades);
        }

        ChangeSkin(upgrade);
    }

    public void ChangeSkin(UpgradeShop upgrade)
    {

        ChangeSprites changeSprites = m_Player.GetComponent<ChangeSprites>();
        int[] upgradeBought = upgrade.GetUpgradeBought();


        // Hp/Armor Skin
        if (upgradeBought[0] + upgradeBought[1] + upgradeBought[2] < 8)
        {
            changeSprites.ChangeSprite(0, 0);
            if (upgrade.m_ShopShipParts.Length > 0)
            { upgrade.m_ShopShipParts[0].sprite = upgrade.m_ShopShips[0]; }
        }
        else if (upgradeBought[0] + upgradeBought[1] + upgradeBought[2] < 15)
        {
            changeSprites.ChangeSprite(0, 1);
            if (upgrade.m_ShopShipParts.Length > 0)
            { upgrade.m_ShopShipParts[0].sprite = upgrade.m_ShopShips[1]; }
        }
        else if (upgradeBought[0] + upgradeBought[1] + upgradeBought[2] < 23)
        {
            changeSprites.ChangeSprite(0, 2);
            if (upgrade.m_ShopShipParts.Length > 0)
            { upgrade.m_ShopShipParts[0].sprite = upgrade.m_ShopShips[2]; }
        }
        else if (upgradeBought[0] + upgradeBought[1] + upgradeBought[2] < 30)
        {
            changeSprites.ChangeSprite(0, 3);
            if (upgrade.m_ShopShipParts.Length > 0)
            { upgrade.m_ShopShipParts[0].sprite = upgrade.m_ShopShips[3]; }
        }
        else if (upgradeBought[0] + upgradeBought[1] + upgradeBought[2] == 30)
        {
            changeSprites.ChangeSprite(0, 4);
            if (upgrade.m_ShopShipParts.Length > 0)
            { upgrade.m_ShopShipParts[0].sprite = upgrade.m_ShopShips[4]; }
        }


        // Engine Skin
        if (upgradeBought[3] + upgradeBought[4] + upgradeBought[5] < 8)
        {
            changeSprites.ChangeSprite(1, 0);
            if (upgrade.m_ShopShipParts.Length > 0)
            { upgrade.m_ShopShipParts[1].sprite = upgrade.m_ShopShips[5]; }
        }
        else if (upgradeBought[3] + upgradeBought[4] + upgradeBought[5] < 15)
        {
            changeSprites.ChangeSprite(1, 1);
            if (upgrade.m_ShopShipParts.Length > 0)
            { upgrade.m_ShopShipParts[1].sprite = upgrade.m_ShopShips[6]; }
        }
        else if (upgradeBought[3] + upgradeBought[4] + upgradeBought[5] < 23)
        {
            changeSprites.ChangeSprite(1, 2);
            if (upgrade.m_ShopShipParts.Length > 0)
            { upgrade.m_ShopShipParts[1].sprite = upgrade.m_ShopShips[7]; }
        }
        else if (upgradeBought[3] + upgradeBought[4] + upgradeBought[5] < 30)
        {
            changeSprites.ChangeSprite(1, 3);
            if (upgrade.m_ShopShipParts.Length > 0)
            { upgrade.m_ShopShipParts[1].sprite = upgrade.m_ShopShips[8]; }
        }
        else if (upgradeBought[3] + upgradeBought[4] + upgradeBought[5] == 30)
        {
            changeSprites.ChangeSprite(1, 4);
            if (upgrade.m_ShopShipParts.Length > 0)
            { upgrade.m_ShopShipParts[1].sprite = upgrade.m_ShopShips[9]; }
        }


        // Cannon Skin
        if (upgradeBought[6] + upgradeBought[7] + upgradeBought[8] < 8)
        {
            changeSprites.ChangeSprite(2, 0);
            //changeSprites.ChangeAnimator(0);
            if (upgrade.m_ShopShipParts.Length > 0)
            { upgrade.m_ShopShipParts[2].sprite = upgrade.m_ShopShips[10]; }
        }
        else if (upgradeBought[6] + upgradeBought[7] + upgradeBought[8] < 15)
        {
            changeSprites.ChangeSprite(2, 1);
            //changeSprites.ChangeAnimator(1);
            if (upgrade.m_ShopShipParts.Length > 0)
            { upgrade.m_ShopShipParts[2].sprite = upgrade.m_ShopShips[11]; }
        }
        else if (upgradeBought[6] + upgradeBought[7] + upgradeBought[8] < 23)
        {
            changeSprites.ChangeSprite(2, 2);
            //changeSprites.ChangeAnimator(2);
            if (upgrade.m_ShopShipParts.Length > 0)
            { upgrade.m_ShopShipParts[2].sprite = upgrade.m_ShopShips[12]; }
        }
        else if (upgradeBought[6] + upgradeBought[7] + upgradeBought[8] < 30)
        {
            changeSprites.ChangeSprite(2, 3);
            //changeSprites.ChangeAnimator(3);
            if (upgrade.m_ShopShipParts.Length > 0)
            { upgrade.m_ShopShipParts[2].sprite = upgrade.m_ShopShips[13]; }
        }
        else if (upgradeBought[6] + upgradeBought[7] + upgradeBought[8] == 30)
        {
            changeSprites.ChangeSprite(2, 4);
            //changeSprites.ChangeAnimator(4);
            if (upgrade.m_ShopShipParts.Length > 0)
            { upgrade.m_ShopShipParts[2].sprite = upgrade.m_ShopShips[14]; }
        }

        if (upgrade.m_ShopShipParts.Length > 0)
        {
            upgrade.m_ShopShipParts[2].SetNativeSize();
            upgrade.m_ShopShipParts[2].SetNativeSize();
            upgrade.m_ShopShipParts[2].SetNativeSize();
        }
    }
}
