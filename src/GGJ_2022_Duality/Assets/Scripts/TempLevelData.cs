using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempLevelData : MonoBehaviour
{
    public string stringData = string.Empty;
    public int intData = -1;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
}
