using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : Singleton<SceneManagement>
{
    int currentIndex = 0;

    private void LoadScene()
    {
        SceneManager.LoadScene(currentIndex);
    }

    public void NextScene()
    {
        currentIndex++;
        if (currentIndex > SceneManager.sceneCount)
            currentIndex = 0;

        LoadScene();
    }

    public void LoadFirstScene()
    {
        currentIndex = 0;
        LoadScene();
    }
}
