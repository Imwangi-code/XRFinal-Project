using UnityEngine;
using System.Collections;
using System;

public class MeditationVoiceover : MonoBehaviour
{
    [Header("Voiceover Clips")]
    public AudioClip[] meditationSteps;

    [Header("Breath Cue Clips (optional)")]
    public AudioClip inhalePrompt;   // e.g. a soft "breathe in" tone
    public AudioClip exhalePrompt;   // e.g. a soft "breathe out" tone

    [Header("Ambient Background")]
    public AudioClip ambientLoop;
    public float ambientVolume = 0.4f;
    public float fadeOutDuration = 3f;

    [Header("Breath Sync")]
    public HeadBreathTracker breathTracker; // Assign in Inspector
    public bool waitForBreathCycle = true;  // Wait for a full breath before advancing

    private AudioSource voiceSource;
    private AudioSource ambientSource;
    private AudioSource cueSource;  // Separate source for breath cue tones

    // Other scripts can subscribe to these
    public event Action OnInhalePrompt;
    public event Action OnExhalePrompt;
    public event Action OnSessionComplete;

    void Start()
    {
        voiceSource = gameObject.AddComponent<AudioSource>();
        voiceSource.spatialBlend = 0f;
        voiceSource.volume = 1f;
        voiceSource.loop = false;

        ambientSource = gameObject.AddComponent<AudioSource>();
        ambientSource.clip = ambientLoop;
        ambientSource.spatialBlend = 0.2f;
        ambientSource.loop = true;
        ambientSource.volume = ambientVolume;
        ambientSource.Play();

        cueSource = gameObject.AddComponent<AudioSource>();
        cueSource.spatialBlend = 0f;
        cueSource.volume = 0.6f;
        cueSource.loop = false;

        StartCoroutine(PlayMeditationSequence());
    }

    IEnumerator PlayMeditationSequence()
    {
        yield return new WaitForSeconds(2f);

        foreach (AudioClip step in meditationSteps)
        {
            voiceSource.clip = step;
            voiceSource.Play();
            yield return new WaitForSeconds(step.length);

            // After narration ends: wait for one full breath cycle if enabled
            if (waitForBreathCycle && breathTracker != null)
                yield return StartCoroutine(WaitForBreathCycle());
            else
                yield return new WaitForSeconds(3f);
        }

        OnSessionComplete?.Invoke();
        StartCoroutine(FadeOut(fadeOutDuration));
        Debug.Log("Meditation session complete.");
    }

    // Waits until the user completes one full inhale → exhale cycle
    IEnumerator WaitForBreathCycle()
    {
        // Wait for inhale to start
        yield return new WaitUntil(() => breathTracker.isInhaling);
        PlayCue(inhalePrompt);
        OnInhalePrompt?.Invoke();

        // Wait for exhale to start
        yield return new WaitUntil(() => !breathTracker.isInhaling);
        PlayCue(exhalePrompt);
        OnExhalePrompt?.Invoke();

        // Wait for exhale to finish (next inhale begins)
        yield return new WaitUntil(() => breathTracker.isInhaling);
    }

    void PlayCue(AudioClip clip)
    {
        if (clip == null) return;
        cueSource.clip = clip;
        cueSource.Play();
    }

    // Pauses/resumes both voice and ambient together
    public void TogglePause()
    {
        if (voiceSource.isPlaying)
        {
            voiceSource.Pause();
            ambientSource.Pause();
            cueSource.Pause();
        }
        else
        {
            voiceSource.UnPause();
            ambientSource.UnPause();
            cueSource.UnPause();
        }
    }

    IEnumerator FadeOut(float duration)
    {
        float startVolume = ambientSource.volume;
        while (ambientSource.volume > 0)
        {
            ambientSource.volume -= startVolume * Time.deltaTime / duration;
            yield return null;
        }
        ambientSource.Stop();
    }
}
