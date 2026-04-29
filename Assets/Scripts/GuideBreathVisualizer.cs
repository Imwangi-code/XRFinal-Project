using UnityEngine;

public class GuidedBreathSphere : MonoBehaviour
{
    [Header("BPM / Timing Settings")]
    [Tooltip("Duration of one full inhale or exhale in seconds.")]
    public float breathDuration = 4.0f; 
    
    [Header("Scale Settings")]
    public float minScale = 0.5f;
    public float maxScale = 2.0f;

    [Header("Visual Settings")]
    public Color inhaleColor = Color.cyan;
    public Color exhaleColor = Color.magenta;

    private float _timer;
    private bool _isExpanding = true;
    private Renderer _renderer;

    void Start()
    {
        _renderer = GetComponent<Renderer>();
    }

    void Update()
    {
        // 1. Advance the timer
        _timer += Time.deltaTime;

        // 2. Determine the progress (0 to 1) of the current breath phase
        float progress = _timer / breathDuration;

        // 3. Calculate the current scale and color
        if (_isExpanding)
        {
            UpdateVisuals(minScale, maxScale, progress, inhaleColor);
        }
        else
        {
            UpdateVisuals(maxScale, minScale, progress, exhaleColor);
        }

        // 4. Switch phases when timer hits the duration
        if (_timer >= breathDuration)
        {
            _timer = 0f;
            _isExpanding = !_isExpanding;
        }
    }

    void UpdateVisuals(float startSize, float endSize, float t, Color targetColor)
    {
        // Use LerpStep or SmoothStep for a more "organic" lung-like movement
        float currentSize = Mathf.SmoothStep(startSize, endSize, t);
        transform.localScale = new Vector3(currentSize, currentSize, currentSize);

        // Update color
        _renderer.material.color = Color.Lerp(_renderer.material.color, targetColor, t);
    }
}