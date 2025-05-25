using UnityEngine;
using UnityEngine.SceneManagement; 

public class MainMenuManager : MonoBehaviour
{

    public string gameSceneName = "GameScene";

    public void StartGame()
    {
        if (!string.IsNullOrEmpty(gameSceneName))
        {
            SceneManager.LoadScene(gameSceneName);
        }
        else
        {
            Debug.LogError("Game Scene Name is not set in MainMenuManager!");
        }
    }
    public void QuitGame()
    {
        Debug.Log("Quit Game Requested!"); 
        Application.Quit();
    }
}