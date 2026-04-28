using UnityEngine;
using UnityEngine.InputSystem;

public class HandFollow : MonoBehaviour
{
    public float followDistance = 0.5f;   // how far in front of you
    public Vector3 offset = new Vector3(0, -0.05f, 0.1f); // tweak this
    public float smoothSpeed = 10f;

    private Camera cam;
    private bool isHolding = false;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        if (Pointer.current == null || cam == null)
            return;

        // Detect pinch (press)
        if (Pointer.current.press.wasPressedThisFrame)
        {
            isHolding = true;
        }

        if (Pointer.current.press.wasReleasedThisFrame)
        {
            isHolding = false;
        }

        if (!isHolding)
            return;

        Vector2 screenPos = Pointer.current.position.ReadValue();

        Vector3 targetPos = cam.ScreenToWorldPoint(
            new Vector3(screenPos.x, screenPos.y, followDistance)
        );

        targetPos += cam.transform.TransformDirection(offset);

        // Smooth movement
        transform.position = Vector3.Lerp(
            transform.position,
            targetPos,
            Time.deltaTime * smoothSpeed
        );
    }
}