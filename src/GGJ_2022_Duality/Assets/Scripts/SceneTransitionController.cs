using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class SceneTransitionController : MonoBehaviour
{
    private static SceneTransitionController instance;
    public static SceneTransitionController Instance {
        get {
            if (instance == null)
            {
                instance = FindObjectOfType<SceneTransitionController>();
            }
            return instance;
        }
    }

    public const string MENU_NAME = "menu";
    public const string GAME_NAME = "game";

    private CanvasGroup faderCanvasGroup;
    private float fadeDuration = 1f;

    public string startScene;
    private Scene activeScene;

    private bool isFading;
    public bool IsFading { get { return isFading; } }

    public event Action StartTransition = delegate { };
    public event Action EndTransition = delegate { };

    private void Awake()
    {
        if (instance != null && instance != this) { Destroy(this); }
        else { instance = this; }

        faderCanvasGroup = GetComponent<CanvasGroup>();        
    }

    private void Start()
    {
        TempLevelData tld = FindObjectOfType<TempLevelData>();
        if (tld)
        {
            startScene = tld.stringData;
            Destroy(tld.gameObject);
        }

        StartCoroutine(StartPersistent());
    }

    public void LoadGame()
    {
        SwitchScenes(GAME_NAME);
    }

    public void LoadMenu()
    {
        SwitchScenes(MENU_NAME);
    }

    public void SwitchScenes(string sceneName)
    {
        StartCoroutine(FadeAndSwitchScenes(sceneName));
    }

    private IEnumerator StartPersistent()
    {
        Time.timeScale = 0;
        faderCanvasGroup.alpha = 1f;
        GC.Collect();
        yield return SceneManager.LoadSceneAsync(startScene, LoadSceneMode.Additive);

        activeScene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);

        SceneManager.SetActiveScene(activeScene);

        Time.timeScale = 1;

        yield return StartCoroutine(Fade(0f));
    }

    // This is the main coroutine where it goes through the entire game transition
    private IEnumerator FadeAndSwitchScenes(string sceneName)
    {
        //!TODO: Tell game manager that player can't input here

        Time.timeScale = 0;

        yield return StartCoroutine(Fade(1f));
        faderCanvasGroup.alpha = 1f;

        GC.Collect();

        yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);

        yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        activeScene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);

        SceneManager.SetActiveScene(activeScene);

        Time.timeScale = 1;

        yield return StartCoroutine(Fade(0f));

        //!TODO: Tell game manager that player can input here
    }


    private IEnumerator RestartSameScene()
    {
        Time.timeScale = 0;
        yield return StartCoroutine(Fade(1f));
        faderCanvasGroup.alpha = 1f;
        Time.timeScale = 1;
        yield return StartCoroutine(Fade(0f));
    }

    //Coroutine to fade the loading screen in and out
    public IEnumerator Fade(float finalAlpha)
    {
        isFading = true;

        if (finalAlpha == 1f)
        {
            StartTransition?.Invoke();
        }

        faderCanvasGroup.blocksRaycasts = true;

        float fadeSpeed = Mathf.Abs(faderCanvasGroup.alpha - finalAlpha) / fadeDuration;

        while (!Mathf.Approximately(faderCanvasGroup.alpha, finalAlpha))
        {
            faderCanvasGroup.alpha = Mathf.MoveTowards(faderCanvasGroup.alpha, finalAlpha, fadeSpeed * Time.unscaledDeltaTime);
            yield return null;
        }

        isFading = false;

        if (finalAlpha == 0f)
        {
            EndTransition?.Invoke();
        }

        faderCanvasGroup.blocksRaycasts = false;
    }
}
