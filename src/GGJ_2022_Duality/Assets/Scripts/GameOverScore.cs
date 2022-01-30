using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameOverScore : MonoBehaviour
{
    private TMP_Text text;

    private void Awake()
    {
        text = GetComponent<TMP_Text>();
        UpdateScore(0);
    }

    private void Start()
    {
        TempLevelData tld = FindObjectOfType<TempLevelData>();
        if (tld && tld.stringData.Equals(string.Empty))
        {
            UpdateScore(tld.intData);
            Destroy(tld.gameObject);
        }
    }

    private void UpdateScore(int score)
    {
        text.text = "Score: " + score;
    }
}
