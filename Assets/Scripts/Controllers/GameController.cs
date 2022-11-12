using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    [HideInInspector] public int activeLayer = 0;

    private int currCoins = 0;

    public bool paused = false;
    public bool movementPaused = false;

    private bool roundOver;
    [HideInInspector] public bool playedIntro = false;
    private bool pauseLocked = false;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        if (!playedIntro)
            StartLevel();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 1)
        {
            SetPause(true, false);
            pauseLocked = true;

        }
    }

    public void StartLevel()
    {
        LevelUI.Instance.ShowUpgradeUI(false);
        pauseLocked = false;
        SetPause(false, false);
        SetMovement(false);
        roundOver = false;

        if (!playedIntro)
        {
            TutorialController.Instance.PlayIntro();
        }
        else
        {
            LevelUI.Instance.ShowTimer(true);
            TimerController.Instance.RestartTimer();
        }
    }

    public void EndLevel()
    {
        if (!roundOver)
        {
            SetPause(true, false);
            LevelUI.Instance.SetEndMenu(true);
            roundOver = true;
        }
    }

    public void ReloadLevel()
    {
        SetPause(false, false);
        SetMovement(false);
        SceneManager.LoadScene("Level");
    }

    public void ReturnToMain()
    {
        SetPause(false, false);
        SetMovement(false);
        SceneManager.LoadScene(0);
    }

    public void Win()
    {
        SetPause(true, false);
        LevelUI.Instance.SetWinMenu(true);
        SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetActiveScene());
        SceneManager.MoveGameObjectToScene(PlayerManager.Instance.gameObject, SceneManager.GetActiveScene());
    }

    private void Update()
    {
        LevelUI.Instance.SetCoinCount(currCoins);
    }

    #region Pausing
    public void TogglePause(bool toggleMenu=true)
    {
        SetPause(!paused,toggleMenu);
    }
    public void SetPause(bool pauseState, bool toggleMenu=true)
    {
        if (pauseLocked)
            return;

        paused = pauseState;
        Time.timeScale = paused ? 0 : 1;

        if (toggleMenu)
            LevelUI.Instance.SetPauseMenu(paused);
    }

    public void ToggleMovement()
    {
        SetMovement(!movementPaused);
    }
    public void SetMovement(bool movementState)
    {
        movementPaused = movementState;
    }
    #endregion

    #region Money
    public void AddCoins(int value)
    {
        currCoins += value;
    }
    
    public bool TryRemoveCoins(int value) 
    {
        return value <= currCoins;
    }

    public bool RemoveCoins(int value)
    {
        if (!TryRemoveCoins(value))
            return false;

        currCoins -= value;
        return true;
    }
    #endregion
}
