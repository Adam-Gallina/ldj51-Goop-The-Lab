using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Goal : DirectInteraction
{
    public override void Interact()
    {
        GameController.instance.EndLevel();
    }
}
