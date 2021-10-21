using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Stats")]
    public float speed;
    public float gravity;

    [Header("References")]
    public Transform self;

    private Vector3 currentMove;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void HorizontalUpdate()
    {
        // Get the inputs
        currentMove.x += Input.GetAxis("Horizontal");
        currentMove.z += Input.GetAxis("Vertical");

        // Apply movement
        self?.Translate(currentMove * Time.deltaTime * speed, Space.World);

        // Rotate the direction where the player move
        if (currentMove != Vector3.zero)
            self.forward = currentMove;
    }

    private void VerticalUpdate()
    {

    }

    // Update is called once per frame
    void Update()
    {
        currentMove = Vector3.zero;
        HorizontalUpdate();
        VerticalUpdate();
    }
}
