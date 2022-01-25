using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitObject : MonoBehaviour
{
    private bool _isNightTime = false;

    void setIsNightTime(bool val) {
        this._isNightTime = val;
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (this._isNightTime) {
            Debug.Log("I got picked up at night");
        } else {
            Debug.Log("I got picked up during the day.");
        }
    }
}
