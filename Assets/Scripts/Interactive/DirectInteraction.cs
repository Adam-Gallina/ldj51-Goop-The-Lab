using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DirectInteraction : Interactive
{
    public bool requiresHand = false;

    public abstract bool CanInteract();

    public abstract bool Interact(Transform source);
}
