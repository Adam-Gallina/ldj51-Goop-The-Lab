using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Lever : DirectInteraction
{
    [SerializeField] private Transform handle;
    [SerializeField] private float offAng;
    [SerializeField] private float onAng;

    public override bool CanInteract()
    {
        return true;
    }

    public override bool Interact(Transform source)
    {
        if (!activated)
            TutorialController.Instance.PlayEndDoor();

        activated = true;
        handle.transform.localEulerAngles = new Vector3(0, 0, onAng);

        return true;
    }
}
