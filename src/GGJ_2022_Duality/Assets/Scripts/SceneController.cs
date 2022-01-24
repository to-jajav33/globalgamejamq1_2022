using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    SceneTransitionController stc;
    string sceneName;

    protected void Awake()
    {
        sceneName = gameObject.scene.name;
        CheckIfLoadPersistent();
    }

    protected void Start()
    {
        stc = SceneTransitionController.Instance;
    }

    //Used to check if we are in persistent, and if we are not
    //Persistent is loaded with the info from the scene we are in
    protected void CheckIfLoadPersistent()
    {
        if (SceneTransitionController.Instance == null)
        {
            GameObject tempData = new GameObject();
            TempLevelData tld = tempData.AddComponent<TempLevelData>();
            tld.stringData = sceneName;
            SceneManager.LoadScene("persistent");
            this.gameObject.SetActive(false);
        }
    }
}
