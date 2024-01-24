/*
 * Class for changing scenes.
 * 
 * @author Richard
 * @version January 23
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;

    private void Awake()
    {
        Instance = this;
    }
    /**
     * Loads the next scene in the build index.
     */
    public void LoadNextScene()
    {
        DataManager.Instance.SaveToFiles();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    /**
     * Loads the start scene (scene at build index 0).
     */
    public void LoadStartScene()
    {
        DataManager.Instance.ResetTimer();
        DataManager.Instance.SaveToFiles();
        SceneManager.LoadScene(0);
    }
    /**
     * Quits the game application.
     */
    public void QuitGame()
    {
        Application.Quit();
    }
    /**
     * Loads the ending scene.
     */
    public void LoadEndScene()
    {
        
        DataManager.Instance.StopTimer();
        DataManager.Instance.CompareBestTimes();
        DataManager.Instance.SaveToFiles();
        SceneManager.LoadScene(2);

    }
    /**
     * Loads the losing scene.
     */
    public void LoadDeathScene()
    {
        DataManager.Instance.StopTimer();
        DataManager.Instance.SaveToFiles();
        SceneManager.LoadScene(3);
    }

    /*
     * Loads the losing scene after a specified amount of time
     */
    public void LoadDeathScene(float time)
    {
        Invoke("LoadDeathScene", time);
    }
}
