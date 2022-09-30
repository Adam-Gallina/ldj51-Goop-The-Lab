using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Door : PassiveInteraction
{
    public Color activeColor = new Color(1, 1, 1, 0.5f);
    public Color inactiveColor = new Color(1, 1, 1, 1);

    private BoxCollider2D coll;
    private SpriteRenderer image;

    private void Awake()
    {
        coll = GetComponent<BoxCollider2D>();
        image = GetComponentInChildren<SpriteRenderer>();
    }

    protected override void Activate()
    {
        coll.isTrigger = true;
        image.color = activeColor;
    }

    protected override void Deactivate()
    {
        coll.isTrigger = false;
        image.color = inactiveColor;
    }
}
