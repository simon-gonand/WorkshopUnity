using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam_LookAtTarget : MonoBehaviour
{
    public Transform target;

    private void Update()
    {
        transform.LookAt(target);
    }
}
