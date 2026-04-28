using UnityEngine;
using System.Collections; // Required for timers (Coroutines)

public class MeditationManager : MonoBehaviour
{
    [Header("Environment Setup")]
    public MeshRenderer skyCloudShell; 
    public GameObject drawManager;     
    public GameObject doorPromptUI;    // Optional: Text that says "Draw a Door"

    [Header("Audio Setup")]
    public AudioSource voiceCoach;     // Plays the 1-minute intro and the final prompt
    public AudioSource ambientLoop;    // Loops the background noise

    [Header("Mood Materials")]
    public Material anxiousMaterial;
    public Material tiredMaterial;

    [Header("Mood Audio (Voice Intros)")]
    public AudioClip anxiousIntro;
    public AudioClip tiredIntro;

    [Header("UI Cleanup")]
    public GameObject moodPanel;
    public GameObject timePanel;

    // Hidden variables to remember what they picked
    private Material chosenMaterial;
    private AudioClip chosenIntro;

    void Start()
    {
        // Ensure the brush and door prompt are OFF when the app starts
        if (drawManager != null) drawManager.SetActive(false);
        if (doorPromptUI != null) doorPromptUI.SetActive(false);
    }

    // --- MOOD BUTTONS (Connect these to your Panel_Mood buttons) ---
    public void SelectAnxious()
    {
        chosenMaterial = anxiousMaterial;
        chosenIntro = anxiousIntro;
        
        // Hide Mood Panel, Show Time Panel
        moodPanel.SetActive(false);
        timePanel.SetActive(true);
    }

    public void SelectTired()
    {
        chosenMaterial = tiredMaterial;
        chosenIntro = tiredIntro;
        
        moodPanel.SetActive(false);
        timePanel.SetActive(true);
    }

    // --- TIME BUTTONS (Connect these to your Panel_Time buttons) ---
    public void Start2Minute() { StartCoroutine(MeditationRoutine(2f)); }
    public void Start5Minute() { StartCoroutine(MeditationRoutine(5f)); }
    public void Start10Minute() { StartCoroutine(MeditationRoutine(10f)); }
    public void Start15Minute() { StartCoroutine(MeditationRoutine(15f)); }

    // --- THE TIMER LOGIC ---
   // --- THE UPDATED TIMER LOGIC ---
    private IEnumerator MeditationRoutine(float totalMinutes)
    {
        // 1. Hide the Time Menu
        timePanel.SetActive(false);

        // 2. Play the Intro Voice (e.g., "Breathe... now draw a door to your escape.")
        if (voiceCoach != null)
        {
            voiceCoach.clip = chosenIntro;
            voiceCoach.Play();
        }
        
        // Start the looping ambient sound
        if (ambientLoop != null) ambientLoop.Play(); 

        // 3. Wait exactly 1 minute (60 seconds) for the intro to finish
        yield return new WaitForSeconds(60f);

        // 4. Turn ON the drawing tools so they can draw the door!
        if (drawManager != null) drawManager.SetActive(true);
        if (doorPromptUI != null) doorPromptUI.SetActive(true);

        // 5. Calculate the remaining time and wait for it to finish
        // (e.g., If they picked 5 mins: 5 - 1 = 4 mins remaining)
        float remainingMinutes = totalMinutes - 1f;
        yield return new WaitForSeconds(remainingMinutes * 60f);

        // 6. TIME IS UP! (End of the entire meditation)
        // You can fade out audio here, or show a "Welcome Back" menu
        if (ambientLoop != null) ambientLoop.Stop();
    }
}