using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTravel : MonoBehaviour
{
    // Load scene by name
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    // Load scene by index
    public void LoadSceneByIndex(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    // Quit game (for builds)
    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }
}