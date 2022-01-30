using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BaseObject))]
public class BaseObjectAction : MonoBehaviour
{
    protected BaseObject baseObject;
    protected HitObject hitObject;
    protected PickUpObject pickUpObject;

    protected PlayerController pc;
    protected GameController gc => GameController.Instance;

    public bool canAction = true;

    protected virtual void Awake()
    {
        pc = FindObjectOfType<PlayerController>();
        baseObject = GetComponent<BaseObject>();
        hitObject = GetComponent<HitObject>();
        pickUpObject = GetComponent<PickUpObject>();
    }

    protected virtual void OnEnable()
    {
        if (hitObject)
        {
            hitObject.IsHit += OnAction;
        }

        if (pickUpObject)
        {
            pickUpObject.IsTriggered += OnAction;
        }
    }

    protected virtual void OnDisable()
    {
        if (hitObject)
        {
            hitObject.IsHit -= OnAction;
        }

        if (pickUpObject)
        {
            pickUpObject.IsTriggered -= OnAction;
        }
    }

    protected virtual void OnAction()
    {

    }
}
