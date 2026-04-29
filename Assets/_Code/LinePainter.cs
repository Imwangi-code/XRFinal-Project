using UnityEngine;
using System.Collections.Generic;

public class LinePainter : MonoBehaviour
{
    readonly float minDistance = 0.01f;

    public bool right;
    List<Vector3> points = new();
    LineRenderer line;
    bool lastTrigger = true;
    bool active = true;
    PinchTest pinchTest;

    void Start()
    {
        line = GetComponent<LineRenderer>();
        line.positionCount = 0;
        pinchTest = FindAnyObjectByType<PinchTest>();
    }

    void Update()
    {
        if (!active) return;

        bool trigger = right ? pinchTest.activeRightPinch : pinchTest.activeLeftPinch;
        Vector3 pinchPoint = right ? pinchTest.pinchPointRight : pinchTest.pinchPointLeft;

        if (trigger)
        {
            MakeLine(pinchPoint);
        }
        else if (!trigger && lastTrigger)
        {
            active = false;
        }
        lastTrigger = trigger;
    }

    void MakeLine(Vector3 pinchPoint)
    {
        Vector3 currentPos = transform.InverseTransformPoint(pinchPoint);
        if (points.Count == 0 || Vector3.Distance(points[points.Count - 1], currentPos) > minDistance)
        {
            points.Add(currentPos);
            line.positionCount = points.Count;
            line.SetPosition(points.Count - 1, currentPos);
        }
    }
}