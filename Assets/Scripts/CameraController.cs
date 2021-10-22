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
    private bool isInTexture;

    // Start is called before the first frame update
    void Start()
    {
        offset = Rig.position - player.position;
        isInTexture = false;
    }


    public void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            float newAlpha = 1.0f;
            /*if (isInTexture)
            {
                newAlpha = 1.0f;
            }*/

            Color[] meshColor = other.gameObject.GetComponent<MeshRenderer>().material.GetColorArray("_Color");
            Color newAlphaColor = new Color(meshColor[0].r, meshColor[0].g, meshColor[0].b, newAlpha);
            Color[] newAlphaColorArray = { newAlphaColor };
            other.gameObject.GetComponent<MeshRenderer>().material.SetColorArray("_Color", newAlphaColorArray);

            isInTexture = !isInTexture;
        }
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
