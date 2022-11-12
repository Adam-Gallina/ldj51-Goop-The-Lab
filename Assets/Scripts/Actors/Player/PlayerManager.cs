using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum UpgradeType { none, moveSpeed, jumpCount, coinValue, hand, spitballCount, spitballDamage, health, jumpHeight, ventSpeed, jumpLength }
public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    [SerializeField] private LinearUpgrade moveSpeed;
    [SerializeField] private SetUpgrade jumpLength;
    [SerializeField] private LinearUpgrade health;
    [SerializeField] private LinearUpgrade coinValue;

    [SerializeField] private LinearUpgrade hand;
    [SerializeField] private LinearUpgrade walljumpCount;
    [SerializeField] private LinearUpgrade spitballCount;
    [SerializeField] private LinearUpgrade spitballDamage;
    [SerializeField] private LinearUpgrade ventSpeed;
    public Dictionary<UpgradeType, Upgrade> upgrades = new Dictionary<UpgradeType, Upgrade>();

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        upgrades.Add(UpgradeType.moveSpeed, moveSpeed);
        upgrades.Add(UpgradeType.coinValue, coinValue);
        upgrades.Add(UpgradeType.health, health);
        upgrades.Add(UpgradeType.jumpLength, jumpLength);
        upgrades.Add(UpgradeType.hand, hand);
        upgrades.Add(UpgradeType.spitballCount, spitballCount);
        upgrades.Add(UpgradeType.spitballDamage, spitballDamage);
        upgrades.Add(UpgradeType.jumpCount, walljumpCount);
        upgrades.Add(UpgradeType.ventSpeed, ventSpeed);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        LevelUI.Instance.UpdateUpgradeButtons(upgrades);
    }

    public bool DoUpgrade(UpgradeType upgrade)
    {
        if (!Input.GetKey(KeyCode.LeftShift))
        {
            if (!GameController.Instance.TryRemoveCoins((int)upgrades[upgrade].Cost))
                return false;
        }

        if (!upgrades[upgrade].CanUpgrade)
            return false;

        if (!Input.GetKey(KeyCode.LeftShift))
            GameController.Instance.RemoveCoins((int)upgrades[upgrade].Cost);
        upgrades[upgrade].level++;
        LevelUI.Instance.UpdateUpgradeButtons(upgrades);
        if (upgrade == UpgradeType.health)
            GameObject.Find("Player")?.GetComponent<PlayerController>().RefillHealth();

        return true;
    }

    public void UnlockUpgrade(UpgradeType upgrade)
    {
        upgrades[upgrade].unlocked = true;
        LevelUI.Instance.UpdateUpgradeButtons(upgrades);
        TutorialController.Instance.PlayUpgradeTutorial(upgrade);
    }
}

public abstract class Price
{
    public bool unlocked = true;
    public int level = 0;
    public int maxLevel = 5;
    public int basePrice = 1;
    public int priceMod = 1;
    //public float priceModScale = 1;

    public int Cost { get { return basePrice + priceMod * level; } }
    public bool CanUpgrade { get { return unlocked && level < maxLevel; } }

}

public abstract class Upgrade : Price
{
    public abstract float Mod();

    public static implicit operator float(Upgrade obj)
    {
        return obj.Mod();
    }
}

[System.Serializable]
public class LinearUpgrade : Upgrade
{
    public float baseVal = 0;
    public float mod = 0;

    [HideInInspector] public override float Mod() { return baseVal + mod * level; }
}

[System.Serializable]
public class SetUpgrade : Upgrade
{
    public float baseVal = 0;
    public float[] mod;

    public override float Mod() { return baseVal + mod[level]; }
}