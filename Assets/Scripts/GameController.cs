using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    [HideInInspector] public int activeLayer = 0;

    private int currLevel = 0;

    public bool paused = false;
    public bool movementPaused = false;

    void Awake()
    {
        instance = this;

        currLevel = SceneManager.GetActiveScene().buildIndex;
    }


    public void EndLevel()
    {
        Debug.Log("Level end");
        //SceneManager.LoadScene("Level " + (++currLevel).ToString());
    }

    #region Pausing
    public void TogglePause()
    {
        SetPause(!paused);
    }
    public void SetPause(bool pauseState)
    {
        paused = pauseState;
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
}
