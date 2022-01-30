using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_Text))]
public class MenuScoreText : MonoBehaviour
{
    private TMP_Text text;
    private GameController gc => GameController.Instance;

    private void Awake()
    {
        text = GetComponent<TMP_Text>();
        UpdateScore(0);
    }

    private void OnEnable()
    {
        gc.NewScoreEvent += UpdateScore;
    }

    private void OnDisable()
    {
        gc.NewScoreEvent -= UpdateScore;
    }

    private void UpdateScore(int score)
    {
        text.text = "Score: " + score;
    }
}
