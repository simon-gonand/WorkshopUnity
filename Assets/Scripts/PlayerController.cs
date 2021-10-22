using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Stats")]
    public float speed;
    public AnimationCurve groundAcceleration;
    public int maxAllowedJumps;
    public float jumpForce;

    [Header("References")]
    public Transform self;
    public Transform selfModel;
    public Transform selfHips;
    public Rigidbody selfRigidbody;
    public Animator selfAnimator;
    public Transform cameraPivot;

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
        selfAnimator.SetBool("IsGrounded", false);        
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

        // Get forward and right vector of the camera without the Y axis
        Vector3 forwardWithoutY = new Vector3(cameraPivot.forward.x, 0.0f, cameraPivot.forward.z);
        Vector3 rightWithoutY = new Vector3(cameraPivot.right.x, 0.0f, cameraPivot.right.z);
        
        // Apply movement
        self?.Translate(rightWithoutY * acceleration * currentMove.x * Time.deltaTime * speed, Space.World);
        self?.Translate(forwardWithoutY * acceleration * currentMove.z * Time.deltaTime * speed, Space.World);

        // Rotate the direction where the player move
        if (currentMove != Vector3.zero)
        {
            // Calculate the direction of the player movement
            Vector3 targetDirection = forwardWithoutY * currentMove.z;
            targetDirection += rightWithoutY * currentMove.x;
            targetDirection = targetDirection.normalized;
            // Rotate player model
            selfModel.forward = targetDirection;
        }

        // Update animations
        selfAnimator.SetFloat("Speed", currentMove.x + currentMove.z);
    }

    private void VerticalUpdate()
    {
        // Jump
        if (Input.GetButtonDown("Jump") && remainingJumps > 0)
        {
            --remainingJumps;
            isGrounded = false;
            selfAnimator.SetBool("IsGrounded", false);
            ++currentMove.y;
            selfRigidbody?.AddForce((selfModel.forward + Vector3.up)* currentMove.y * jumpForce, ForceMode.Impulse);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // If player is touching the ground
        if (collision.transform.position.y < selfHips.position.y)
        {
            isGrounded = true;
            selfAnimator.SetBool("IsGrounded", true);
            remainingJumps = maxAllowedJumps;
        }
    }

    // Update is called once per frame
    void Update()
    {
        currentMove = Vector3.zero;
        HorizontalUpdate();
        VerticalUpdate();
        if (Input.GetKeyDown(KeyCode.R))
        {
            //Reload scene from 0
        }
    }
}
