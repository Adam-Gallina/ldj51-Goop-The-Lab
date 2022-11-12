using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockableUpgrade : Collectible
{
    [SerializeField] protected UpgradeType[] upgrade;
    [SerializeField] protected float rotSpeed = 1;
    [SerializeField] protected float scaleSpeed = 0.1f;
    [SerializeField] protected float maxScale = 1.8f;
    [SerializeField] protected float minScale = 1.25f;

    private void Start()
    {
        if (upgrade.Length > 0 && PlayerManager.Instance.upgrades[upgrade[0]].unlocked)
            Destroy(gameObject);
    }

    private void FixedUpdate()
    {
        transform.GetChild(1).localEulerAngles = new Vector3(0, 0, transform.GetChild(1).localEulerAngles.z + rotSpeed);
        float s = minScale + (maxScale - minScale) / 2 + ((maxScale - minScale) / 2) * Mathf.Sin(Time.time * scaleSpeed); 
        transform.GetChild(1).localScale = new Vector3(s, s, s);
    }

    protected override void Collect()
    {
        foreach (UpgradeType t in upgrade)
            PlayerManager.Instance.UnlockUpgrade(t);

        //TimerController.Instance.currTimer += 5;

        base.Collect();
    }
}
