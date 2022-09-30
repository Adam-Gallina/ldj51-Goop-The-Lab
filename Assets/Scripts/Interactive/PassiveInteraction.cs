using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PassiveInteraction : Interactive
{
    [SerializeField] public ActiveRequirement[] required;

    void Update()
    {
        CheckRequirements();
    }

    protected virtual void CheckRequirements()
    {
        bool activationChange = true;

        foreach (ActiveRequirement req in required)
        {
            if (!req.IsActive())
            {
                activationChange= false;
                break;
            }
        }

        if (activationChange != activated)
        {
            activated = activationChange;
            
            if (activated) Activate();
            else Deactivate();
        }
    }

    protected abstract void Activate();
    protected abstract void Deactivate();

}

[System.Serializable]
public class ActiveRequirement
{
    public Interactive target;
    public bool activeState = true;

    public bool IsActive()
    {
        return target.GetStatus() == activeState;
    }
}
