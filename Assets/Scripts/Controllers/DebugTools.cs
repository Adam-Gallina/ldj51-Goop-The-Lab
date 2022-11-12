using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugTools : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKey(KeyCode.Equals))
        {
            GameController.Instance.AddCoins(10);
            LevelUI.Instance.UpdateUpgradeButtons(PlayerManager.Instance.upgrades);
        }
        if (Input.GetKeyDown(KeyCode.Minus))
        {
            GameController.Instance.AddCoins(10);
            LevelUI.Instance.UpdateUpgradeButtons(PlayerManager.Instance.upgrades);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            PlayerManager.Instance.UnlockUpgrade(UpgradeType.hand);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            PlayerManager.Instance.UnlockUpgrade(UpgradeType.jumpCount);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            PlayerManager.Instance.UnlockUpgrade(UpgradeType.spitballCount);
            PlayerManager.Instance.UnlockUpgrade(UpgradeType.spitballDamage);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            PlayerManager.Instance.UnlockUpgrade(UpgradeType.ventSpeed);
        }

        if (Input.GetKeyDown(KeyCode.P))
            GameController.Instance.TogglePause();
    }
}
