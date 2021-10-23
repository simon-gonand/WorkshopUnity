using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam_Follow : MonoBehaviour
{
    public GameObject Target;

    void FixedUpdate()
    {
        gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, Target.transform.position,
            0.9f);
    }
    /*
    // If cam is jittering try using late update instead
    private void LateUpdate()
    {
        gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, Target.transform.position,
    0.9f);
    }
    */
}
