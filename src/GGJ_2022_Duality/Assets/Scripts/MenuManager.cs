using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    private SceneTransitionController stc;

    private void Awake()
    {
        stc = SceneTransitionController.Instance;
    }

    public void TransitionToMenu()
    {
        stc.LoadMenu();
    }

    public void TransitionToGame()
    {
        stc.LoadGame();
    }

    public void TransitionToGameOver()
    {
        stc.LoadGameOver();
    }

    public void TransitionToInstruction()
    {
        stc.LoadInstruction();
    }
}
