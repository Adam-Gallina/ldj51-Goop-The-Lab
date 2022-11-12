using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlarmButton : DirectInteraction
{
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private Sprite btnUp;
    [SerializeField] private Sprite btnDown;
    [SerializeField] private float pressTime;

    private AudioSource aud;

    private void Awake()
    {
        aud = GetComponent<AudioSource>();
    }

    public override bool CanInteract()
    {
        return requiresHand && PlayerManager.Instance.upgrades[UpgradeType.hand].unlocked 
            && GameObject.Find("Player").GetComponent<PlayerController>().totalInteractions < PlayerManager.Instance.upgrades[UpgradeType.hand];
    }

    public override bool Interact(Transform source)
    {
        if (!CanInteract())
            return false;

        if (TimerController.Instance.currTimer >= 9.5f)
            return false;

        sprite.sprite = btnDown;
        TimerController.Instance.RestartTimer();
        Invoke("BtnUp", pressTime);
        aud.Play();

        return true;
    }

    private void BtnUp()
    {
        sprite.sprite = btnUp;
    }
}
