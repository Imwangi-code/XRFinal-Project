using UnityEngine;
using UnityEngine.SceneManagement;

public class OpeningMenuManager : MonoBehaviour
{
    public GameObject moodPanel;
    public GameObject timePanel;

    // --- MOOD BUTTONS ---
    public void SelectAnxious() { SessionData.chosenMood = "Anxious"; NextPanel(); }
    public void SelectTired() { SessionData.chosenMood = "Tired"; NextPanel(); }
    public void SelectStressed() { SessionData.chosenMood = "Stressed"; NextPanel(); }
    public void SelectCreative() { SessionData.chosenMood = "Creative"; NextPanel(); }

    private void NextPanel()
    {
        moodPanel.SetActive(false);
        timePanel.SetActive(true);
    }

    // --- TIME BUTTONS ---
    public void StartTime2Min() { SessionData.chosenTime = 2f; LoadMeditation(); }
    public void StartTime5Min() { SessionData.chosenTime = 5f; LoadMeditation(); }
    public void StartTime10Min() { SessionData.chosenTime = 10f; LoadMeditation(); }
    public void StartTime15Min() { SessionData.chosenTime = 15f; LoadMeditation(); }

    private void LoadMeditation()
    {
        // Loads Scene 2. Make sure the name matches your scene EXACTLY!
        SceneManager.LoadScene("EnterMeditation"); 
    }
}