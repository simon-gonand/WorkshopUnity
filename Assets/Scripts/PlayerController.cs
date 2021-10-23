using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Stats")]
    public float walkSpeed;
    public float sprintSpeed;
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
    private bool isAttacking;
    private bool isAttackingOnSprint;
    private float speed;

    // Start is called before the first frame update
    void Start()
    {
        remainingJumps = maxAllowedJumps;
        isGrounded = false;
        isAttacking = false;
        selfAnimator.SetBool("IsGrounded", false);
        speed = walkSpeed;
    }

    private void IsSprinting()
    {
        if (Input.GetButtonDown("Sprint"))
        {
            selfAnimator.SetBool("IsSprinting", true);
            speed = sprintSpeed;
        }
        else if (Input.GetButtonUp("Sprint"))
        {
            selfAnimator.SetBool("IsSprinting", false);
            speed = walkSpeed;
        }
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
        selfAnimator.SetFloat("Speed", Mathf.Abs(currentMove.x) + Mathf.Abs(currentMove.z));
    }

    private void VerticalUpdate()
    {
        // Jump
        if (Input.GetButtonDown("Jump") && remainingJumps > 0)
        {
            --remainingJumps;
            isGrounded = false;
            selfAnimator.SetTrigger("Jump");
            selfAnimator.SetBool("IsGrounded", false);
            ++currentMove.y;
            selfRigidbody?.AddForce(Vector3.up * currentMove.y * jumpForce, ForceMode.Impulse);
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

    private void IsAnimationFinished()
    {
        if (selfAnimator.GetBehaviours<OnAttackBehaviour>()[0].animationIsFinished &&
            selfAnimator.GetBehaviours<OnAttackBehaviour>()[1].animationIsFinished)
        {
            isAttacking = false;
            isAttackingOnSprint = false;
        }
    }

    private void Attack()
    {
        IsAnimationFinished();
        if (Input.GetButtonDown("Attack") && !isAttacking && !isAttackingOnSprint)
        {
            if (sprintSpeed == speed)
            {
                isAttackingOnSprint = true;
            }
            else
            {
                isAttacking = true;
                int randomAttackIndex = Random.Range(0, 5);
                selfAnimator.SetFloat("RandomAttack", randomAttackIndex);
            }
            selfAnimator.SetTrigger("Attack");
        }
    }

    // Update is called once per frame
    void Update()
    {
        currentMove = Vector3.zero;
        Attack();
        IsSprinting();
        if (!isAttacking)
        {
            HorizontalUpdate();
            VerticalUpdate();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            //Reload scene from 0
        }
    }
}
