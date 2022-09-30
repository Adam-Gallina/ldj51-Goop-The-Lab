using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Lever : DirectInteraction
{
    public override void Interact()
    {
        activated = !activated;
        Debug.Log(name + " is " + activated);
    }
}
