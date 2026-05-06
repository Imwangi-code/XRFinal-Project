using UnityEngine;
using UnityEngine.SceneManagement;

public class SessionExitManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject endButtonObject;
    public GameObject invisibleCatcherObject;

    // This runs when the user pinches the invisible background
    public void ShowEndButton()
    {
        endButtonObject.SetActive(true);
        invisibleCatcherObject.SetActive(false); // Hide the catcher so they don't accidentally click it again
    }

    // This runs when they pinch the actual End button
    public void ReturnToMainMenu()
    {
        // Replace "OpeningMenu" with the EXACT name of your first scene!
        SceneManager.LoadScene("OpeningMenu"); 
    }
}