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
    public Transform CameraAttached;
    public Transform Pivot;
    public Transform self;

    private Vector3 offset;
    private Vector3 cameraRotation;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Translate camera according to player position
        Vector3 movement = Vector3.zero;
        movement.x = Mathf.Lerp(CameraAttached.localPosition.x, player.position.x + offset.x, speed * Time.deltaTime);
        movement.y = Mathf.Lerp(CameraAttached.localPosition.y, player.position.y + offset.y, speed * Time.deltaTime);
        movement.z = Mathf.Lerp(CameraAttached.localPosition.z, player.position.z + offset.z, speed * Time.deltaTime);
        CameraAttached.localPosition = player.position;

        // Rotate camera with the right stick around the player
        offset = CameraAttached.position - player.position;
        cameraRotation.y -= Input.GetAxis("RHorizontal") * speed;
        cameraRotation.x -= Input.GetAxis("RVertical") * speed;
        cameraRotation.x = Mathf.Clamp(cameraRotation.x, minLimitYCam, maxLimitYCam);
        CameraAttached.localRotation = Quaternion.Euler(0.0f, cameraRotation.y, 0.0f);
        Pivot.localRotation = Quaternion.Euler(cameraRotation.x, 0.0f, 0.0f);
        self.LookAt(player.position);
    }
}
