using UnityEngine;
using UnityEngine.XR.Hands;
using System.Collections.Generic;

public class WristTracker : MonoBehaviour
{
    public Transform leftHandTrans;
    public Transform rightHandTrans;

    XRHandSubsystem handSubsystem;
    static readonly List<XRHandSubsystem> subsystems = new();

    void Start()
    {
        //Get subsystems
        SubsystemManager.GetSubsystems(subsystems);
        if (subsystems.Count > 0)
        {
            handSubsystem = subsystems[0];
        }
        else
        {
            Debug.LogWarning("XRHandSubsystem not found.");
        }
    }

    void Update()
    {
        //make sure the hand subsystem is running
        if (handSubsystem == null || !handSubsystem.running) return;

        ProcessHand(handSubsystem.leftHand, leftHandTrans);
        ProcessHand(handSubsystem.rightHand, rightHandTrans);
    }

    void ProcessHand(XRHand hand, Transform handTrans)
    {
        //make sure hand tracking is happening
        if (!hand.isTracked) return;

        var joint = hand.GetJoint(XRHandJointID.IndexTip); 
    
    if (joint.TryGetPose(out Pose pose))
    {
        // Get the position and rotation and set it to the prefab
        handTrans.SetPositionAndRotation(pose.position, pose.rotation);
    }
    }
}