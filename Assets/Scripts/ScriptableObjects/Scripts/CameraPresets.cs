using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Camera Presets", menuName = "ScriptableObject/Camera Preset", order = 2)]
public class CameraPresets : ScriptableObject
{
    [Header("Speed")]
    public float speed;
    [Header("Limit on Y-Axis")]
    public float minLimitYCam;
    public float maxLimitYCam;
}
