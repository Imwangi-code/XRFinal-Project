using UnityEngine;

public class BreathVisualizer : MonoBehaviour
{
    public HeadBreathTracker tracker; // Drag Main Camera here
    
    [Header("Scale Settings")]
    public float minScale = 0.01f;
    public float maxScale = 0.05f;
    public float growSpeed = 2.0f;

    private Vector3 _targetScale;

    void Update()
    {
        if (tracker == null) return;

        // 1. Determine the target size based on breath state
        float size = tracker.isInhaling ? maxScale : minScale;
        _targetScale = new Vector3(size, size, size);

        // 2. Smoothly transition to that size
        transform.localScale = Vector3.Lerp(transform.localScale, _targetScale, Time.deltaTime * growSpeed);

        //3. Changes color of object
        GetComponent<Renderer>().material.color = Color.Lerp(Color.green, Color.blue, (transform.localScale.x - minScale) / (maxScale - minScale));
   
    }
}

