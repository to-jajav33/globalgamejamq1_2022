using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthContainer : MonoBehaviour
{
    private Image[] healthImages;

    public Sprite HealthyKiwi;
    public Sprite DeadKiwi;

    private void Awake()
    {
        healthImages = GetComponentsInChildren<Image>();

    }

    public void SetHealth(int amount)
    {
        for (int i = 1; i < healthImages.Length; i++)
        {
            if (i<amount)
            {
                healthImages[i].sprite = HealthyKiwi;
            }
            else
            {
                healthImages[i].sprite = DeadKiwi;
            }
        }
    }
}
