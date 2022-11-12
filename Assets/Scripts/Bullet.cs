using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] protected int damage;
    [SerializeField] private float speed;
    [SerializeField] private LayerMask targets;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & targets.value) > 0)
        {
            HealthBase target = collision.gameObject.GetComponent<HealthBase>();
            if (target && target.Damage(CalcDamage()))
            {
                Destroy(gameObject);
            }
        }
        else if (collision.gameObject.layer == Constants.GroundLayer ||
                 collision.gameObject.layer == Constants.EnvironmentLayer)
        {
            Destroy(gameObject);
        }
    }

    protected virtual int CalcDamage()
    {
        return damage;
    }

    public void Fire(Vector2 dir)
    {
        GetComponent<Rigidbody2D>().velocity = dir.normalized * speed;
        transform.localEulerAngles = new Vector3(0, 0, -Vector2.SignedAngle(dir, Vector2.right));
    }
}
