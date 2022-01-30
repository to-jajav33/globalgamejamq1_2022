using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealObjectAction : PositiveObjectAction
{
    protected override void OnAction()
    {
        if (!canAction) { return; }

        base.OnAction();
        pc.HealPlayer();
    }
}
