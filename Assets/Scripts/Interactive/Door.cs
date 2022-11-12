using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Door : DirectInteraction
{
    [SerializeField] private DirectInteraction lever;

    [SerializeField] private GameObject closed;
    [SerializeField] private GameObject open;

    private void Update()
    {
        closed.SetActive(!lever.GetStatus());
        open.SetActive(lever.GetStatus());
    }

    public override bool CanInteract()
    {
        return lever.GetStatus();
    }

    public override bool Interact(Transform source)
    {
        if (!CanInteract())
            return false;

        GameController.Instance.Win();
        return true;
    }


}
