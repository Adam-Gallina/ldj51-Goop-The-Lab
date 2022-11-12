using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerController : MonoBehaviour
{
    public static TimerController Instance;

    [SerializeField] private float maxTimer;
    [HideInInspector] public float currTimer;
    private bool timerActive = false;

    [SerializeField] private Image timer;

    [SerializeField] private GameObject lasers;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (timerActive)
        {
            currTimer -= Time.deltaTime;

            //LevelUI.Instance.SetTimer(currTimer / maxTimer);
            LevelUI.Instance.SetTimer(currTimer);

            if (currTimer <= 0)
            {
                currTimer = 0;
                //GameController.Instance.EndLevel();
                lasers.SetActive(true);
                GameObject.Find("Player").GetComponent<HealthBase>().Damage(1);
                //timerActive = false;
            }
        }
    }

    public void RestartTimer()
    {
        currTimer = maxTimer;
        timerActive = true;
        lasers.SetActive(false);
    }
}
