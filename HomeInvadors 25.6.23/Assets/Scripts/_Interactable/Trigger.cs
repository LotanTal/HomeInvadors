using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
Triggers are object that can trigger the enemy.
For example a turned on TV or radio can make noise and make the enemy come to it
**/
public abstract class Trigger : Interactable
{
    public float radiusOfEffect = 30f;
    public bool isActive = false;

    protected override void Interact()
    {
        if (isActive == false)
        {
            OnTriggerActive();
            isActive = true;
        }
        else
        {
            OnTriggerInactive();
            isActive = false;
        }
    }

    public abstract void OnTriggerActive();

    public abstract void OnTriggerInactive();
}
