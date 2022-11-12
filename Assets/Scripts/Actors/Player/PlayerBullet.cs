using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : Bullet
{
    protected override int CalcDamage()
    {
        return (int)PlayerManager.Instance.upgrades[UpgradeType.spitballDamage];
    }
}
