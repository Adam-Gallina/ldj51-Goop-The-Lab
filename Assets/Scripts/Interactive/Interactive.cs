using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactive : MonoBehaviour
{
    protected bool activated;

    public virtual bool GetStatus()
    {
        return activated;
    }
}
