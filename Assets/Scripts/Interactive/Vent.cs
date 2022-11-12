using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vent : DirectInteraction
{
    [SerializeField] private float moveSpeed;
    [SerializeField] protected Vector2[] path;
    [SerializeField] private float offset = 0.1f;
    private bool updatedPath = false;

    [SerializeField] private Transform placeholder;

    private AudioSource aus;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;

        for (int i = 0; i < path.Length-1; i++)
        {
            if (!updatedPath)
                Gizmos.DrawLine((Vector2)transform.position + path[i], (Vector2)transform.position + path[i + 1]);
            else
                Gizmos.DrawLine(path[i], path[i + 1]);
        }
    }

    private void Awake()
    {
        for (int i = 0; i < path.Length; i++)
        {
            path[i] += (Vector2)transform.position;
        }
        updatedPath = true;

        aus = GetComponent<AudioSource>();
    }

    public override bool CanInteract()
    {
        return PlayerManager.Instance.upgrades[UpgradeType.ventSpeed].unlocked;
    }

    public override bool Interact(Transform source)
    {
        if (!CanInteract())
            return false;

        StartCoroutine(Transport(source));
        return true;
    }

    private IEnumerator Transport(Transform target)
    {
        aus.Play();
        placeholder.position = target.position;
        CameraController.instance.SetFollowTarget(placeholder);
        target.gameObject.SetActive(false);

        int dir = Vector2.Distance(placeholder.position, path[0]) < Vector2.Distance(placeholder.position, path[path.Length - 1]) ? 1 : -1;
        int currPos = dir == 1 ? 1 : path.Length - 2;
        int remaining = path.Length - 1;

        do
        {
            Vector2 currDir = (path[currPos] - (Vector2)placeholder.position).normalized;
            placeholder.Translate(currDir * PlayerManager.Instance.upgrades[UpgradeType.ventSpeed] * Time.deltaTime);

            yield return new WaitForEndOfFrame();

            if (Vector2.Distance(placeholder.position, path[currPos]) < offset)
            {
                currPos += dir;
                remaining--;
            }
        } while (remaining > 0);

        aus.Play();
        target.position = placeholder.position;
        CameraController.instance.SetFollowTarget(target);
        target.gameObject.SetActive(true);
    }
}
