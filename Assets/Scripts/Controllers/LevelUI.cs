using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUI : MonoBehaviour
{
    public static LevelUI Instance;

    [SerializeField] private float parallaxOffset = 0.1f;

    [Header("Level UI")]
    [SerializeField] private GameObject timerObj;
    [SerializeField] private Image timerImage;
    [SerializeField] private Text timerText;
    [SerializeField] private Text coinText;
    [SerializeField] private Text healthText;
    [SerializeField] private GameObject handObj;
    [SerializeField] private Text handText;
    [SerializeField] private GameObject spitballObj;
    [SerializeField] private Text spitballText;

    [Header("Menus")]
    [SerializeField] private GameObject pauseMenuObj;
    [SerializeField] private GameObject endMenuObj;
    [SerializeField] private GameObject winMenuObj;

    [Header("Upgrades")]
    [SerializeField] private GameObject upgradeUiObj;
    [SerializeField] private UpgradeButton[] upgradeBtns;

    [Header("Indicators")]
    [SerializeField] private RectTransform interactIndicator;
    [SerializeField] private Vector2 indicatorOffset;
    private Transform interactTarget;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        ShowLevelUI(false);
        //if (GameController.Instance.playedIntro)
            ShowUpgradeUI(true);
        SetPauseMenu(false);
        //SetEndMenu(false);
    }

    private void Update()
    {
        if (interactTarget)
        {
            interactIndicator.position = Camera.main.WorldToScreenPoint((Vector2)interactTarget.position + indicatorOffset);
        }

        Vector2 playerPos = GameObject.Find("Player").transform.position;
        GameObject.Find("Background").transform.position = new Vector2(playerPos.x * parallaxOffset, 0);
    }

    public void ShowUpgradeUI(bool state)
    {
        ShowHandCount(PlayerManager.Instance.upgrades[UpgradeType.hand].unlocked);
        ShowSpitballCount(PlayerManager.Instance.upgrades[UpgradeType.spitballCount].unlocked);

        upgradeUiObj.SetActive(state);
    }

    public void UpdateUpgradeButtons(Dictionary<UpgradeType, Upgrade> upgrades)
    {
        foreach (UpgradeButton btn in upgradeBtns)
        {
            btn.btn.transform.parent.gameObject.SetActive(upgrades[btn.upgrade].unlocked);
            btn.price.text = upgrades[btn.upgrade].Cost.ToString();
            btn.level.text = "Level " + (upgrades[btn.upgrade].level + 1);
            //btn.btn.interactable = GameController.Instance.TryRemoveCoins(upgrades[btn.upgrade].Cost) && upgrades[btn.upgrade].CanUpgrade;
            btn.btn.GetComponentInChildren<Text>().text = upgrades[btn.upgrade].CanUpgrade ? "Buy" : "Max";

            //btn.btn.onClick.RemoveAllListeners();
            //btn.btn.onClick.AddListener(() => BuyUpgrade((int)btn.upgrade));
        }

        ShowHandCount(upgrades[UpgradeType.hand].unlocked);
        SetHandCount((int)upgrades[UpgradeType.hand]);
        
        ShowSpitballCount(upgrades[UpgradeType.spitballCount].unlocked);
        SetSpitballCount((int)upgrades[UpgradeType.spitballCount]);
    }

    public void ShowLevelUI(bool state)
    {
        ShowTimer(state);
        ShowHandCount(PlayerManager.Instance.upgrades[UpgradeType.hand].unlocked && state);
        ShowSpitballCount(PlayerManager.Instance.upgrades[UpgradeType.spitballCount].unlocked && state);
    }

    public void ShowTimer(bool state)
    {
        timerObj.SetActive(state);
    }
    /*public void SetTimer(float fillAmount)
    {
        timerImage.fillAmount = fillAmount;
    }*/
    public void SetTimer(float time)
    {
        timerText.text = time > 0 ? time.ToString("F3") : "0.000";
    }

    public void SetHealthCount(int count)
    {
        healthText.text = count.ToString();
    }

    public void ShowHandCount(bool status)
    {
        handObj.SetActive(status);
    }
    public void SetHandCount(int count)
    {
        handText.text = count.ToString();
    }

    public void ShowSpitballCount(bool status)
    {
        spitballObj.SetActive(status);
    }
    public void SetSpitballCount(int count)
    {
        spitballText.text = count.ToString();
    }

    public void SetCoinCount(int count)
    {
        coinText.text = count.ToString();
    }

    public void SetPauseMenu(bool status)
    {
        pauseMenuObj.SetActive(status);
    }

    public void SetEndMenu(bool status)
    {
        //endMenuObj.SetActive(status);
        EndRun();
    }

    public void EndRun()
    {
        GameController.Instance.ReloadLevel();
    }

    public void SetWinMenu(bool status)
    {
        winMenuObj.SetActive(status);
    }

    #region Buttons
    public void PressStart()
    {
        GameController.Instance.StartLevel();
    }

    public void BuyUpgrade(int upgrade)
    {
        PlayerManager.Instance.DoUpgrade((UpgradeType)upgrade);
    }
    public void BuyUpgrade(UpgradeType upgrade)
    {
        PlayerManager.Instance.DoUpgrade(upgrade);
    }

    public void PressResume()
    {
        GameController.Instance.SetPause(false);
    }

    public void PressQuit()
    {
        GameController.Instance.ReturnToMain();
    }
    #endregion

    #region Dynamic UI
    public void SetInteractTarget(Transform target=null)
    {
        interactTarget = target;
        interactIndicator.gameObject.SetActive(target != null);
    }
    #endregion
}

[System.Serializable]
public struct UpgradeButton
{
    public UpgradeType upgrade;
    public Text price;
    public Text level;
    public Button btn;

    /*public UpgradeButton(RectTransform t)
    {
        upgrade = UpgradeType.none;
        price = t.GetChild(3).GetComponent<Text>();
        btn = t.GetComponentInChildren<Button>();
    }*/
}