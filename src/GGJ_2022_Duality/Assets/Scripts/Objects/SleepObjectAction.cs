using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleepObjectAction : PositiveObjectAction
{
    protected override void OnAction()
    {
        Debug.Log("sleep action hit");
        if (!canAction) { return; }

        base.OnAction();
        gc.TriggerPlayerCollectedMoreNighttime();
    }
}
