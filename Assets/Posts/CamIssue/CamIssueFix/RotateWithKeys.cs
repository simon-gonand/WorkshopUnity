using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateWithKeys : MonoBehaviour
{
    public KeyCode RotateLeftKey = KeyCode.Z;
    public KeyCode RotateRightKey = KeyCode.X;
    public float speedOfRotation = 1f;
    public float RotateAnglePerPress = 45f;

    private Quaternion targetRotation;

    private void Start()
    {
        targetRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(RotateLeftKey))
        {
            ChangeTargetRotation(1);
        }
        else if (Input.GetKeyDown(RotateRightKey))
        {
            ChangeTargetRotation(-1);
        }

        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, (10 * speedOfRotation * Time.deltaTime));

    }

    private void ChangeTargetRotation(float direction)
    {
        targetRotation *= Quaternion.AngleAxis(RotateAnglePerPress*direction, Vector3.up);
    }

  
}
