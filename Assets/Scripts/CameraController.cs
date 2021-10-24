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
    public Transform rig;
    public Transform pivot;
    public Transform self;

    private Vector2 cameraRotation;
    private Vector3 offset;
    private bool isInTexture;

    // Start is called before the first frame update
    void Start()
    {
        isInTexture = false;
    }

    // When the camera is entering inside an object
   /* public void OnTriggerExit(Collider other)
    {
        float newAlpha = 0.0f;
        // If it is in texture and there is a collision with the camera, then it means that the camera is going out from the object
        if (isInTexture)
        {
            newAlpha = 1.0f;
        }

        // Update alpha transparency
        Color[] meshColor = other.gameObject.GetComponent<MeshRenderer>().material.GetColorArray("_Color");
        Color newAlphaColor = new Color(meshColor[0].r, meshColor[0].g, meshColor[0].b, newAlpha);
        Color[] newAlphaColorArray = { newAlphaColor };
        other.gameObject.GetComponent<MeshRenderer>().material.SetColorArray("_Color", newAlphaColorArray);

        // Update if the camera is inside the object or not
        isInTexture = !isInTexture;
    }*/

    private void TranslationUpdate()
    {
        // Camera translate with the player movements
        rig.position = player.position + offset;
    }

    // Rotate camera with the right stick around the player
    private void RotationUpdate()
    {
        // Get inputs from the right stick and clamp
        cameraRotation.y -= Input.GetAxis("RHorizontal") * speed;
        cameraRotation.x -= Input.GetAxis("RVertical") * speed;
        cameraRotation.x = Mathf.Clamp(cameraRotation.x, minLimitYCam, maxLimitYCam);

        // Process rotations on the two axis
        rig.rotation = Quaternion.Euler(0.0f, cameraRotation.y, 0.0f);
        pivot.localRotation = Quaternion.Euler(cameraRotation.x, 0.0f, 0.0f);

        // Camera is looking at the player so it can rotate around it
        self.LookAt(player.position);
    }

    // Update is called once per frame
    void Update()
    {
        TranslationUpdate();
        RotationUpdate();
        // Update the distance between the camera and the player to prevent that the camera is zooming when rotating around the players
        offset = rig.position - player.position;
    }
}
