using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempLevelData : MonoBehaviour
{
    public string stringData;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
}
