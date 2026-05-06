using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro; // Required for TextMeshPro Dropdowns

public class MeditationManager : MonoBehaviour
{
    [Header("Core References")]
    public MeshRenderer skyboxRenderer;
    public GameObject drawManager;
    public GameObject doorPromptUI;
    public GameObject exitButton; // Change 1: Added reference variable
    public AudioSource voiceCoach;
    public AudioSource ambientLoop;

    public TMP_Dropdown moodDropdown; // Reference to the Mood Dropdown in the scene
    public TMP_Dropdown timeDropdown; // Reference to the Time Dropdown in the scene

    [Header("Transition Settings")]
    [Tooltip("The exact name of the scene to load when finished")]
    public string nextSceneName;

    [Header("Anxious Settings")]
    public Material anxiousSky;
    public AudioClip anxiousVoice;
    public AudioClip anxiousAmbient;

    [Header("Tired Settings")]
    public Material tiredSky;
    public AudioClip tiredVoice;
    public AudioClip tiredAmbient;

    [Header("Stressed Settings")]
    public Material stressedSky;
    public AudioClip stressedVoice;
    public AudioClip stressedAmbient;

    [Header("Creative Settings")]
    public Material creativeSky;
    public AudioClip creativeVoice;
    public AudioClip creativeAmbient;

    void Start()
    {
        // 1. Turn off drawing initially
        if (drawManager != null)
        {
            drawManager.SetActive(false);
            // Grab the script and tell it it's officially allowed to draw now
            drawManager.GetComponent<HandDrawer>().canDraw = true; 
        }

        if (doorPromptUI != null) doorPromptUI.SetActive(false);

        // 2. Read the "backpack" and set the environment
        SetupEnvironment(SessionData.chosenMood);

        // 3. Start the timer logic
        StartCoroutine(MeditationRoutine(SessionData.chosenTime));
    }

    private void SetupEnvironment(string mood)
    {
        if (mood == "Anxious")
        {
            skyboxRenderer.material = anxiousSky;
            voiceCoach.clip = anxiousVoice;
            ambientLoop.clip = anxiousAmbient;
        }
        else if (mood == "Tired")
        {
            skyboxRenderer.material = tiredSky;
            voiceCoach.clip = tiredVoice;
            ambientLoop.clip = tiredAmbient;
        }
        else if (mood == "Stressed")
        {
            skyboxRenderer.material = stressedSky;
            voiceCoach.clip = stressedVoice;
            ambientLoop.clip = stressedAmbient;
        }
        else if (mood == "Creative")
        {
            skyboxRenderer.material = creativeSky;
            voiceCoach.clip = creativeVoice;
            ambientLoop.clip = creativeAmbient;
        }
    }

    private IEnumerator MeditationRoutine(float totalMinutes)
    {
        // Play audio
        if (voiceCoach != null) voiceCoach.Play();
        if (ambientLoop != null) ambientLoop.Play();

        // Wait 1 minute (60 seconds) for intro to finish
        yield return new WaitForSeconds(60f);

        // Turn ON drawing tools for the magic door
        if (drawManager != null) drawManager.SetActive(true);
        if (doorPromptUI != null) doorPromptUI.SetActive(true);

        // Wait the remaining time (Total Minutes - 1 minute)
        float remainingMinutes = totalMinutes - 1f;
        
        // If they chose 2 mins, it waits 1 more minute... If 5 minutes, it waits 4 more minutes.
        if (remainingMinutes > 0)
        {
            yield return new WaitForSeconds(remainingMinutes * 60f);
        }

        // Time is up! 
        if (ambientLoop != null) ambientLoop.Stop();

        //Switch to next scene
        SwitchToNextScene();
    }

    // Change 2: Changed 'private' to 'public'
    public void SwitchToNextScene() 
    {
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogError("Next scene name is not set in MeditationManager.");
        }
    }
}