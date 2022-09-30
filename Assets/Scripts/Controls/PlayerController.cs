using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(InputController))]
public class PlayerController : MonoBehaviour
{
    static readonly float DIAGONAL_MODIFIER_CONST = 1 / Mathf.Sqrt(2);

    [Header("Movement")]
    private InputController ic;
    public float movementSpeed;

    [Header("Interactions")]
    private DirectInteraction currInteraction;

    private Rigidbody2D rb;

    void Awake()
    {
        ic = GetComponent<InputController>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        CameraController.instance.SetFollowTarget(transform);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.layer)
        {
            case 9:
                currInteraction = collision.GetComponent<DirectInteraction>();
                break;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        switch (collision.gameObject.layer)
        {
            case 9:
                currInteraction = null;
                break;
        }
    }

    void Update()
    {
        UpdateMovement();
        UpdateInput();
    }

    private void UpdateMovement()
    {
        float x = 0, y = 0;

        if (ic.left) x -= 1;
        if (ic.right) x += 1;

        if (ic.up) y += 1;
        if (ic.down) y -= 1;

        if (x != 0 && y != 0)
        {
            x *= DIAGONAL_MODIFIER_CONST;
            y *= DIAGONAL_MODIFIER_CONST;
        }

        rb.velocity = new Vector2(x, y) * movementSpeed;
    }

    private void UpdateInput()
    {
        if (ic.interact.down && currInteraction)
            currInteraction.Interact();
    }
}
