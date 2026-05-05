using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // Required for TextMeshPro Dropdowns

public class OpeningMenuManager : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Dropdown moodDropdown;
    public TMP_Dropdown timeDropdown;

    // This single function will be attached to your "Start" button
    public void OnStartClicked()
    {
        // 1. Read the Mood Dropdown (0 = Anxious, 1 = Stressed, 2 = Tired, 3 = Creative)
        int moodIndex = moodDropdown.value;
        
        if (moodIndex == 0) SessionData.chosenMood = "Anxious";
        else if (moodIndex == 1) SessionData.chosenMood = "Stressed";
        else if (moodIndex == 2) SessionData.chosenMood = "Tired";
        else if (moodIndex == 3) SessionData.chosenMood = "Creative";

        // 2. Read the Time Dropdown (0 = 2min, 1 = 5min, 2 = 10min, 3 = 15min)
        int timeIndex = timeDropdown.value;
        
        if (timeIndex == 0) SessionData.chosenTime = 2f;
        else if (timeIndex == 1) SessionData.chosenTime = 5f;
        else if (timeIndex == 2) SessionData.chosenTime = 10f;
        else if (timeIndex == 3) SessionData.chosenTime = 15f;

        // 3. Load the Meditation Scene
        SceneManager.LoadScene("EnterMeditation");
    }
}