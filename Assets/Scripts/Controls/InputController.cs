using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public Key left = new Key(KeyCode.A);
    public Key right = new Key(KeyCode.D);
    public Key up = new Key(KeyCode.W);
    public Key down = new Key(KeyCode.S);

    public Key interact = new Key(KeyCode.E);

    public Key pause = new Key(KeyCode.Escape);


    private Key[] keys;

    private void Awake()
    {
        keys = new Key[]
        {
            left,
            right,
            up,
            down,
            interact,
            pause
        };
    }

    public void Update()
    {
        foreach (Key k in keys)
        {
            k.UpdateKey();
        }
    }
}

public class Key
{
    private KeyCode targetKey;
    private KeyCode alternateKey;
    public bool down;
    public bool held;
    public bool up;

    public Key(KeyCode targetKey)
    {
        this.targetKey = targetKey;
    }

    public Key(KeyCode targetKey, KeyCode alternateKey)
    {
        this.targetKey = targetKey;
        this.alternateKey = alternateKey;
    }

    public void UpdateKey()
    {
        down = Input.GetKeyDown(targetKey) || Input.GetKeyDown(alternateKey);
        held = Input.GetKey(targetKey) || Input.GetKey(alternateKey);
        up = Input.GetKeyUp(targetKey) || Input.GetKeyUp(alternateKey);
    }

    public static implicit operator bool(Key obj)
    {
        return obj.down || obj.held || obj.up;
    }
}