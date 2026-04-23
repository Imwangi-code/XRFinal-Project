using UnityEngine;
using System.Collections;

public class MeditationVoiceover : MonoBehaviour
{
    [Header("Voiceover Clips")]
    public AudioClip[] meditationSteps; // Drag clips in Inspector

    [Header("Ambient Background")]
    public AudioClip ambientLoop;

    private AudioSource voiceSource;
    private AudioSource ambientSource;

    void Start()
    {
        // Voice source — non-spatial (2D) so it's heard equally in both ears
        voiceSource = gameObject.AddComponent<AudioSource>();
        voiceSource.spatialBlend = 0f;       // 0 = full 2D, ideal for narration
        voiceSource.volume = 1f;
        voiceSource.loop = false;

        // Ambient source — slightly spatial for immersion
        ambientSource = gameObject.AddComponent<AudioSource>();
        ambientSource.clip = ambientLoop;
        ambientSource.spatialBlend = 0.2f;   // Slight 3D for atmosphere
        ambientSource.loop = true;
        ambientSource.volume = 0.4f;
        ambientSource.Play();

        // Begin the meditation sequence
        StartCoroutine(PlayMeditationSequence());
    }

    IEnumerator PlayMeditationSequence()
    {
        // Brief pause before starting
        yield return new WaitForSeconds(2f);

        foreach (AudioClip step in meditationSteps)
        {
            voiceSource.clip = step;
            voiceSource.Play();

            // Wait for clip to finish, then pause before next step
            yield return new WaitForSeconds(step.length + 3f);
        }

        Debug.Log("Meditation session complete.");
    }

    // Call this to pause/resume (e.g., from a UI button)
    public void TogglePause()
    {
        if (voiceSource.isPlaying)
            voiceSource.Pause();
        else
            voiceSource.UnPause();
    }

    // Fade ambient out at end of session
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
