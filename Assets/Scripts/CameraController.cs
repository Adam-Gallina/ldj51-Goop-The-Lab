using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;

    [Header("Movement")]
    public float moveSpeed;
    public float minDistance = 0.1f;
    public float maxDistance = 2f;
    private Transform target = null;
    private bool locked = false;

    private void Awake()
    {
        instance = this;
    }

    private void FixedUpdate()
    {
        if (target != null && !locked)
        {
            float dist = Vector2.Distance(transform.position, target.position);
            Vector2 newPos = transform.position;

            if (dist > maxDistance)
            {
                newPos = target.position - (target.position - transform.position).normalized * maxDistance;
            }
            else if (dist > minDistance)
            {
                newPos = Vector2.MoveTowards(transform.position, target.position, moveSpeed);
            }

            transform.position = newPos;
        }
    }

    public void SetFollowTarget(Transform t)
    {
        target = t;
    }

    public void ClearFollowTarget()
    {
        target = null;
    }
}
