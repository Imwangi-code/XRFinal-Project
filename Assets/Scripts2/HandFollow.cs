using UnityEngine;
using UnityEngine.XR.Hands;
using UnityEngine.XR.Management;

public class HandFollow : MonoBehaviour
{
    private XRHandSubsystem handSubsystem;
    [SerializeField] private bool useRightHand = true;
    [SerializeField] private XRHandJointID jointId = XRHandJointID.IndexTip;

    private void OnEnable()
    {
        handSubsystem = XRGeneralSettings.Instance?.Manager?.activeLoader
            ?.GetLoadedSubsystem<XRHandSubsystem>();

        if (handSubsystem != null)
            handSubsystem.updatedHands += OnHandsUpdated;
    }

    private void OnDisable()
    {
        if (handSubsystem != null)
            handSubsystem.updatedHands -= OnHandsUpdated;
    }

    private void OnHandsUpdated(XRHandSubsystem subsystem,
        XRHandSubsystem.UpdateSuccessFlags flags,
        XRHandSubsystem.UpdateType updateType)
    {
        var hand = useRightHand ? subsystem.rightHand : subsystem.leftHand;
        var joint = hand.GetJoint(jointId);

        if (joint.TryGetPose(out Pose pose))
        {
            transform.SetPositionAndRotation(pose.position, pose.rotation);
        }
    }
}