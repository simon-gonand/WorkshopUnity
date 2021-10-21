using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    public Transform CameraAttached;
    public Transform self;

    private Vector3 offset;
    private Vector3 cameraRotation;
    private Vector3 initCameraRotation;
    private float speed = 4.0f;   

    // Start is called before the first frame update
    void Start()
    {
        offset = self.position - player.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 movement = Vector3.zero;
        movement.x = Mathf.Lerp(self.position.x, player.position.x + offset.x, speed * Time.deltaTime);
        movement.y = Mathf.Lerp(self.position.y, player.position.y + offset.y, speed * Time.deltaTime);
        movement.z = Mathf.Lerp(self.position.z, player.position.z + offset.z, speed * Time.deltaTime);
        self.position = movement;

        cameraRotation.x = Input.GetAxis("RHorizontal");
        cameraRotation.y = Input.GetAxis("RVertical");
        Vector3 temp = cameraRotation;
        if (cameraRotation.x == 0)
            temp.x = initCameraRotation.x;
        if (cameraRotation.z == 0)
            temp.y = initCameraRotation.y;

        self.rotation = Quaternion.Euler(temp);
        self?.Rotate(player.position, cameraRotation.x * Time.deltaTime * speed);
        self?.Rotate(player.position, cameraRotation.y * Time.deltaTime);
    }
}
