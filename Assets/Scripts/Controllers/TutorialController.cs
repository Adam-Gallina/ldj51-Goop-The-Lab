using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    public static TutorialController Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void PlayIntro()
    {
        StartCoroutine(StartTutorial());
    }

    private IEnumerator StartTutorial()
    {
        GameController.Instance.SetPause(true, false);

        yield return Textbox.Instance.SetText("Welcome to the game!");
        yield return Textbox.Instance.SetText("Use WASD to move, and spacebar to jump");
        yield return Textbox.Instance.SetText("Try and find the lever, and escape through that big door...");
        Textbox.Instance.HideBox();

        GameController.Instance.SetPause(false, false);
        GameController.Instance.playedIntro = true;
        LevelUI.Instance.ShowTimer(true);
        TimerController.Instance.RestartTimer();
    }

    public void PlayEndDoor()
    {
        StartCoroutine(StartEndDoor());
    }

    private IEnumerator StartEndDoor()
    {
        GameController.Instance.SetPause(true, false);

        yield return Textbox.Instance.SetText("Did you hear that?");
        yield return Textbox.Instance.SetText("It sounded like a large door opened! Maybe you can get out now...");
        Textbox.Instance.HideBox();

        GameController.Instance.SetPause(false, false);
    }

    public void PlayUpgradeTutorial(UpgradeType upgrade)
    {
        switch (upgrade)
        {
            case UpgradeType.hand:
                StartCoroutine(HandTutorial());
                break;
            case UpgradeType.spitballCount:
                StartCoroutine(GunTutorial());
                break;
            case UpgradeType.spitballDamage:
                break;
            case UpgradeType.ventSpeed:
                StartCoroutine(VentTutorial());
                break;
            case UpgradeType.jumpCount:
                StartCoroutine(JumpTutorial());
                break;
            default:
                Debug.LogWarning("Tried to play " + upgrade + " tutorial, but it doesn't exist");
                break;
        }
    }

    private IEnumerator HandTutorial()
    {
        GameController.Instance.SetPause(true, false);

        yield return Textbox.Instance.SetText("Is that...a hand?");
        yield return Textbox.Instance.SetText("If only you had one of those, maybe then you could press that button and delay the alarm");
        yield return Textbox.Instance.SetText("Such a shame, with a hand you could simply use 'E' to press the button");
        Textbox.Instance.HideBox();

        GameController.Instance.SetPause(false, false);
    }

    private IEnumerator GunTutorial()
    {
        GameController.Instance.SetPause(true, false);

        yield return Textbox.Instance.SetText("Hm. Looks like you're stuck down here");
        yield return Textbox.Instance.SetText("That wall looks fragile though, maybe you could Left Click and launch projectiles at high speeds?");
        Textbox.Instance.HideBox();

        GameController.Instance.SetPause(false, false);
    }

    private IEnumerator VentTutorial()
    {
        GameController.Instance.SetPause(true, false);

        yield return Textbox.Instance.SetText("Stuck in another hole??");
        yield return Textbox.Instance.SetText("You could probably fit into that vent, maybe it goes somewhere more useful");
        Textbox.Instance.HideBox();

        GameController.Instance.SetPause(false, false);
    }

    private IEnumerator JumpTutorial()
    {
        GameController.Instance.SetPause(true, false);

        yield return Textbox.Instance.SetText("Now that's interesting");
        yield return Textbox.Instance.SetText("You're pretty sticky, and these walls look like they're covered in primer");
        yield return Textbox.Instance.SetText("I bet you could jump towards the wall and jump off of it");
        Textbox.Instance.HideBox();

        GameController.Instance.SetPause(false, false);
    }
}
