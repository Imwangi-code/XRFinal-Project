using UnityEngine;
using UnityEngine.XR;

public class HandFollow : MonoBehaviour
{
    void Update()
    {
        var devices = new System.Collections.Generic.List<InputDevice>();

        InputDevices.GetDevicesWithCharacteristics(
            InputDeviceCharacteristics.HandTracking |
            InputDeviceCharacteristics.Right,
            devices
        );

        foreach (var device in devices)
        {
            if (device.TryGetFeatureValue(CommonUsages.devicePosition, out Vector3 pos))
            {
                transform.position = pos;
            }
        }
    }
}