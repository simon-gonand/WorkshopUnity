using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Stat")]
    public float speed;
    public float minLimitYCam, maxLimitYCam;

    [Header("References")]
    public Transform player;
    public Transform Rig;
    public Transform Pivot;
    public Transform self;

    private Vector2 cameraRotation;
    private Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        offset = Rig.position - player.position;
    }

    private void HorizontalUpdate()
    {
        
        Rig.position = player.position + offset;
    }

    private void VerticalUpdate()
    {
        // Rotate camera with the right stick around the player
        cameraRotation.y -= Input.GetAxis("RHorizontal") * speed;
        cameraRotation.x -= Input.GetAxis("RVertical") * speed;
        cameraRotation.x = Mathf.Clamp(cameraRotation.x, minLimitYCam, maxLimitYCam);

        Rig.rotation = Quaternion.Euler(0.0f, cameraRotation.y, 0.0f);
        Pivot.localRotation = Quaternion.Euler(cameraRotation.x, 0.0f, 0.0f);
        self.LookAt(player.position);
    }

    // Update is called once per frame
    void Update()
    {
        HorizontalUpdate();
        VerticalUpdate();
    }
}
