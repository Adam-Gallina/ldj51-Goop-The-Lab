using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBase : MonoBehaviour
{
    [SerializeField] protected int maxHealth;
    protected int currHealth;
    [SerializeField] private float invincibilityTime;
    private float nextHit;

    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private Color flashCol = Color.red;
    [SerializeField] private float flashDur = 0.1f;

    protected virtual void Awake()
    {
        currHealth = maxHealth;
    }

    protected virtual bool OnHit(int damage)
    {
        return true;
    }

    public bool Damage(int damage)
    {
        if (Time.time < nextHit)
            return false;

        if (OnHit(damage))
        {
            currHealth -= damage;
            nextHit = Time.time + invincibilityTime;

            if (currHealth <= 0)
                Death();
            else if (sprite)
                StartCoroutine(FlashColor());

            return true;
        }

        return false;
    }

    protected virtual void Death()
    {
        Destroy(gameObject);
    }

    private IEnumerator FlashColor()
    {
        Color spriteCol = sprite.color;
        sprite.color = flashCol;

        yield return new WaitForSeconds(flashDur);

        sprite.color = spriteCol;
    }
}
