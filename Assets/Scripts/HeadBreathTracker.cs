using UnityEngine;
using System.Collections.Generic;

public class HeadBreathTracker : MonoBehaviour
{
    [Header("Settings")]
    public float windowSizeSeconds = 30f; // Time window to calculate BPM
    public float sensitivityThreshold = 0.05f; // Ignore micro-jitters
    
    [Header("Live Data (Read Only)")]
    public float currentBPM;
    public bool isInhaling;

    private float _lastPitch;
    private List<float> _breathTimestamps = new List<float>();
    private bool _wasMovingUp;

    void Update()
    {
        // 1. Get current Head Pitch (X-axis rotation)
        float currentPitch = transform.localEulerAngles.x;
        if (currentPitch > 180) currentPitch -= 360; // Normalize to -180 to 180

        // 2. Determine movement direction
        float delta = currentPitch - _lastPitch;
        
        // 3. Detect "Turnaround" points (The peak of a breath)
        if (Mathf.Abs(delta) > sensitivityThreshold)
        {
            bool isMovingUp = delta > 0;

            // If we just switched from moving down to moving up, that's 1 breath cycle
            if (isMovingUp && !_wasMovingUp)
            {
                RecordBreath();
                isInhaling = true;
            }
            else if (!isMovingUp && _wasMovingUp)
            {
                isInhaling = false;
            }

            _wasMovingUp = isMovingUp;
        }

        _lastPitch = currentPitch;
        CalculateBPM();
    }

    void RecordBreath()
    {
        _breathTimestamps.Add(Time.time);
    }

    void CalculateBPM()
    {
        // Remove timestamps older than our window
        _breathTimestamps.RemoveAll(t => t < Time.time - windowSizeSeconds);

        if (_breathTimestamps.Count > 1)
        {
            // BPM = (Number of breaths / seconds) * 60
            currentBPM = (_breathTimestamps.Count / windowSizeSeconds) * 60f;
        }
    }
}