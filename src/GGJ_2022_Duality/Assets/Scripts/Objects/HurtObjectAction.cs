using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtObjectAction : NegativeObjectAction
{
    protected override void OnAction()
    {
        Debug.Log("hurt action hit");
        if (!canAction) { return; }

        base.OnAction();
        pc.HurtPlayer();
    }
}
