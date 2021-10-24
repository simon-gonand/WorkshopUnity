using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    public PlayerPresets playerPreset;
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
    private float speed;
    private bool areControlsEnabled;

    private bool _isAttacking;
    public bool isAttacking { get => _isAttacking; }

    private bool _isAttackingSprint;
    public bool isAttackingSprint { get => _isAttackingSprint; }

    // Player is a singleton
    public static PlayerController instance;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        remainingJumps = playerPreset.maxAllowedJumps;
        _isAttacking = false;
        selfAnimator.SetBool("IsGrounded", false);
        speed = playerPreset.walkSpeed;
        areControlsEnabled = true;
    }

    // Funtion that disable the controls of the player but not the camera
    public void DisableControls()
    {
        areControlsEnabled = false;
    }

    // Test if player is sprinting to adapt the speed and the animations
    private void IsSprinting()
    {
        if (Input.GetButtonDown("Sprint"))
        {
            selfAnimator.SetBool("IsSprinting", true);
            speed = playerPreset.sprintSpeed;
        }
        else if (Input.GetButtonUp("Sprint"))
        {
            selfAnimator.SetBool("IsSprinting", false);
            speed = playerPreset.walkSpeed;
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
        float acceleration = playerPreset.groundAcceleration.Evaluate(accelerationStep);

        // Get forward and right vector of the camera without the Y axis
        Vector3 forwardWithoutY = new Vector3(cameraPivot.forward.x, 0.0f, cameraPivot.forward.z);
        Vector3 rightWithoutY = new Vector3(cameraPivot.right.x, 0.0f, cameraPivot.right.z);
        
        // Apply movement
        self?.Translate(rightWithoutY * acceleration * currentMove.x * Time.deltaTime * speed, Space.World);
        self?.Translate(forwardWithoutY * acceleration * currentMove.z * Time.deltaTime * speed, Space.World);

        // Rotate the direction where the player move according to the position of the camera
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
            selfAnimator.SetTrigger("Jump");
            selfAnimator.SetBool("IsGrounded", false);
            ++currentMove.y;
            selfRigidbody?.AddForce(Vector3.up * currentMove.y * playerPreset.jumpForce, ForceMode.Impulse);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // If player is touching the ground
        if (collision.transform.position.y < selfHips.position.y)
        {
            selfAnimator.SetBool("IsGrounded", true);
            remainingJumps = playerPreset.maxAllowedJumps;
        }
    }

    // Test if the animations of attacks are ended
    // If they are ended then the player is not attacking anymore and the sword cannot kill anyone
    private void IsAnimationFinished()
    {
        if (selfAnimator.GetBehaviours<AnimationFinishedBehaviour>()[1].animationIsFinished)
            _isAttacking = false;
        if (selfAnimator.GetBehaviours<AnimationFinishedBehaviour>()[0].animationIsFinished)
            _isAttackingSprint = false;
    }

    // Process with attack animations
    private void Attack()
    {
        // Check if the player is still attacking or not
        IsAnimationFinished();

        // If the player is not attacking and if he wants to attack
        if (Input.GetButtonDown("Attack") && !isAttacking && !_isAttackingSprint)
        {
            // if player is sprinting then the player will process the Sprint attack animation
            if (playerPreset.sprintSpeed == speed)
            {
                _isAttackingSprint = true;
            }
            else
            {
                _isAttacking = true;
                // Selecting a random attack animation
                int randomAttackIndex = Random.Range(0, 6);
                selfAnimator.SetFloat("RandomAttack", randomAttackIndex);
            }
            // Update animation
            selfAnimator.SetTrigger("Attack");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (areControlsEnabled)
        {
            currentMove = Vector3.zero;
            Attack();
            IsSprinting();
            // When player is attacking (without sprint) he cannot move or jump
            if (!isAttacking)
            {
                HorizontalUpdate();
                VerticalUpdate();
            }
        }

        // Input to reload the game
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(0);
        }
    }
}
