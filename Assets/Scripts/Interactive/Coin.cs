using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : Collectible
{
    [SerializeField] protected int value;

    [SerializeField] protected float bounceSpeed;
    [SerializeField] protected float bounceAmount;

    protected int bounceDir;
    protected float currBounce;
    protected float startBounce;

    [SerializeField] protected Transform bounceObj;

    [Header("Collect anim")]
    [SerializeField] protected float collectTime;
    [SerializeField] protected float driftSpeed;
    protected AudioSource audio;

    private void Awake()
    {
        startBounce = bounceObj.localPosition.y;
        currBounce = Random.Range((int)(-bounceAmount / 2 * 1000), (int)(bounceAmount / 2 * 1000)) / 1000f;
        bounceDir = Random.Range(0, 2) == 0 ? 1 : -1;
        audio = GetComponent<AudioSource>();
    }

    private void Update()
    {
        currBounce += bounceDir * bounceSpeed * Time.deltaTime;

        if (currBounce <= -bounceAmount / 2) 
        {
            currBounce = -bounceAmount / 2;
            bounceDir *= -1;
        }
        else if (currBounce >= bounceAmount / 2)
        {
            currBounce = bounceAmount / 2;
            bounceDir *= -1;
        }

        bounceObj.localPosition = new Vector3(bounceObj.localPosition.x, startBounce + currBounce, bounceObj.localPosition.z);
    }

    protected override void Collect()
    {
        GameController.Instance.AddCoins(value + (int)PlayerManager.Instance.upgrades[UpgradeType.coinValue]);
        audio.Play();
        StartCoroutine(CollectAnim());
    }

    private IEnumerator CollectAnim()
    {
        float startTime = Time.time;

        GameObject player = GameObject.Find("Player");

        while (Time.time < startTime + collectTime)
        {
            float t = (Time.time - startTime) / collectTime;

            float size = 1 - t;
            if (!player)
                break;
            Vector3 dir = player.transform.position + new Vector3(0, 0.1f) - transform.position;

            transform.Translate(t * dir);
            transform.localScale = new Vector3(size, size, size);

            yield return new WaitForEndOfFrame();
        }

        base.Collect();
    }
}
