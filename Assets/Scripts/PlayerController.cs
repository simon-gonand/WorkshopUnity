using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Stats")]
    public float speed;
    public float gravity;
    public AnimationCurve groundAcceleration;
    public int maxAllowedJumps;
    public float jumpForce;

    [Header("References")]
    public Transform self;
    public Transform selfHips;
    public Rigidbody selfRigidbody;

    private Vector3 currentMove;
    private Vector3 lastDirection;
    private float timeStamp;
    private int remainingJumps;
    private bool isGrounded;

    // Start is called before the first frame update
    void Start()
    {
        remainingJumps = maxAllowedJumps;
        isGrounded = false;
    }

    private void HorizontalUpdate()
    {
        // Get the inputs
        currentMove.x += Input.GetAxis("Horizontal");
        currentMove.z += Input.GetAxis("Vertical");

        // Calculate acceleration
        if (Vector3.Dot(currentMove, lastDirection) < 0.0f)
            timeStamp = Time.time;
        float accelerationStep = Time.time - timeStamp;
        float acceleration = groundAcceleration.Evaluate(accelerationStep);

        // Apply movement
        self?.Translate(acceleration * currentMove * Time.deltaTime * speed, Space.World);

        // Rotate the direction where the player move
        if (currentMove != Vector3.zero)
            self.forward = currentMove;
    }

    private void VerticalUpdate()
    {
        if (Input.GetButtonDown("Jump") && remainingJumps > 0)
        {
            --remainingJumps;
            isGrounded = false;
            ++currentMove.y;
            selfRigidbody?.AddForce(currentMove * jumpForce, ForceMode.Impulse);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.position.y < selfHips.position.y)
        {
            isGrounded = true;
            remainingJumps = maxAllowedJumps;
        }
    }

    // Update is called once per frame
    void Update()
    {
        currentMove = Vector3.zero;
        HorizontalUpdate();
        VerticalUpdate();
    }
}
