using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AwakeObjectAction : NegativeObjectAction
{
    protected override void OnAction()
    {
        Debug.Log("awake action hit");
        if (!canAction) { return; }

        base.OnAction();
        gc.TriggerPlayerLostNighttime();
    }
}
