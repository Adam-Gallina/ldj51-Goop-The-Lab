using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : HealthBase
{
    [Header("Movement")]
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected Vector2[] positions;
    protected int targetPos = 0;
    [SerializeField] protected float minOffset = 0.1f;
    private bool updatedPath = false;

    [Header("Combat")]
    [SerializeField] protected int collisionDamage;

    private AudioSource aus;

    private void OnDrawGizmos()
    {
        if (positions.Length == 0)
            return;

        Gizmos.color = Color.gray;

        for (int i = 0; i < positions.Length - 1; i++)
        {
            if (!updatedPath)
            {
                Gizmos.DrawWireCube((Vector2)transform.position + positions[i] + new Vector2(0, 0.5f), new Vector2(1.1f, 1.1f));
                Gizmos.DrawLine((Vector2)transform.position + positions[i] + new Vector2(0, 0.5f), (Vector2)transform.position + positions[i + 1] + new Vector2(0, 0.5f));
            }
            else
            {
                Gizmos.DrawWireCube(positions[i] + new Vector2(0, 0.5f), new Vector2(1.1f, 1.1f));
                Gizmos.DrawLine(positions[i] + new Vector2(0, 0.5f), positions[i + 1] + new Vector2(0, 0.5f));
            }
        }
        if (updatedPath)
            Gizmos.DrawWireCube(positions[positions.Length-1] + new Vector2(0, 0.5f), new Vector2(1.1f, 1.1f));
        else
            Gizmos.DrawWireCube((Vector2)transform.position + positions[positions.Length-1] + new Vector2(0, 0.5f), new Vector2(1.1f, 1.1f));
    }

    protected override void Awake()
    {
        base.Awake();

        for (int i = 0; i < positions.Length; i++)
            positions[i] += (Vector2)transform.position;
        updatedPath = true;

        aus = GetComponent<AudioSource>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == Constants.PlayerLayer)
        {
            collision.GetComponentInParent<HealthBase>().Damage(collisionDamage);
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.layer == Constants.PlayerLayer)
        {
            collision.gameObject.GetComponentInParent<HealthBase>().Damage(collisionDamage);
        }
    }

    private void Update()
    {
        if (positions.Length == 0)
            return;

        if (Vector2.Distance(transform.position, positions[targetPos]) <= minOffset)
        {
            if (++targetPos >= positions.Length)
                targetPos = 0;
        }

        Vector2 dir = (positions[targetPos] - (Vector2)transform.position).normalized;
        transform.Translate(dir * moveSpeed * Time.deltaTime);
    }

    protected override bool OnHit(int damage)
    {
        aus.Play();

        return base.OnHit(damage);
    }
}
