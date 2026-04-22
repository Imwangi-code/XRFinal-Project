using UnityEngine;
using UnityEngine.InputSystem;

public class HandDrawer : MonoBehaviour
{
    [Header("Setup")]
    public GameObject linePrefab;
    public Transform drawPoint; 
    public InputActionReference pinchAction; 

    [Header("Settings")]
    public float pointSpacing = 0.01f; 

    private LineRenderer currentLine;
    private Vector3 previousPoint;

    // --- NEW: We MUST enable the action for it to listen ---
    private void OnEnable()
    {
        if (pinchAction != null) pinchAction.action.Enable();
    }

    private void OnDisable()
    {
        if (pinchAction != null) pinchAction.action.Disable();
    }
    // -------------------------------------------------------

    void Update()
    {
        if (pinchAction == null || drawPoint == null) return;

        // NEW: IsPressed() is much safer than checking a float value
        bool isPinching = pinchAction.action.IsPressed();

        if (isPinching)
        {
            if (currentLine == null)
            {
                StartNewLine();
            }
            UpdateLine();
        }
        else
        {
            currentLine = null; 
        }
    }

    void StartNewLine()
    {
        GameObject newLineObject = Instantiate(linePrefab);
        currentLine = newLineObject.GetComponent<LineRenderer>();
        
        currentLine.positionCount = 1;
        currentLine.SetPosition(0, drawPoint.position);
        previousPoint = drawPoint.position;
    }

    void UpdateLine()
    {
        if (Vector3.Distance(drawPoint.position, previousPoint) > pointSpacing)
        {
            currentLine.positionCount++;
            currentLine.SetPosition(currentLine.positionCount - 1, drawPoint.position);
            previousPoint = drawPoint.position;
        }
    }
}