using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Player Presets", menuName ="ScriptableObject/Player Presets", order = 1)]
public class PlayerPresets : ScriptableObject
{
    [Header("Movements")]
    public float walkSpeed;
    public float sprintSpeed;
    public AnimationCurve groundAcceleration;

    [Header("Jump")]
    public int maxAllowedJumps;
    public float jumpForce;
}
