using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHandle : MonoBehaviour
{
    [Header("References")]
    public Transform selfHips;
    public Animator selfAnimator;
    public NavMeshAgent selfNavMesh;
    public ParticleSystem bloodDropFX;

    private bool isDie;
    private bool _isAttacking;
    public bool isAttacking { get => _isAttacking; }
    private Transform player;

    // Start is called before the first frame update
    void Start()
    {
        isDie = false;
        // Get player transform
        player = PlayerController.instance.self;
    }

    // When the enemy has been killed
    public void Die()
    {
        // Update animation
        selfAnimator.SetBool("IsDead", true);
        // Process blood drop particle system
        bloodDropFX.Play();
        isDie = true;
        // Remove one enemy from the scene update counter
        EnemyManager.instance.nbEnemies = -1;
    }

    // Test if the dying animations is ended in order to destroy the object
    private void IsDieAnimationFinished()
    {
        if (selfAnimator.GetBehaviours<AnimationFinishedBehaviour>()[0].animationIsFinished && isDie)
        {
            Destroy(gameObject);
        }
    }

    // Test if the animations of attacks are ended
    // If they are ended then the enemy's fist cannot touch the player
    private void IsAttackAnimationFinished()
    {
        if (selfAnimator.GetBehaviours<AnimationFinishedBehaviour>()[1].animationIsFinished)
        {
            _isAttacking = false;
        }
    }

    // Behaviour of the enemy
    private void AIBehaviour()
    {
        Animator playerAnimator = player.GetComponent<Animator>();
        bool isPlayerDead = playerAnimator.GetBool("IsDead");
        // If the enemy is dead or if the player is dead, there is no need to anything for the enemy
        if (isDie || isPlayerDead)
        {
            // Stop moving
            selfNavMesh.speed = 0.0f;
            // Stop moving animation
            selfAnimator.SetFloat("Speed", selfNavMesh.speed);

            // Disable the controls of the character if the player is dead
            if (isPlayerDead)
                PlayerController.instance.DisableControls();
            return;
        }

        // Update the destination of the enemy according to the player position
        selfNavMesh.SetDestination(player.position);
        
        // If the player is close enough then attack
        if (Vector3.Distance(player.position, selfHips.position) < 1.5f)
        {
            // Select a random attack animation
            int randomAttackIndex = Random.Range(0, 6);
            selfAnimator.SetFloat("RandomAttack", randomAttackIndex);

            // Update animator
            selfAnimator.SetTrigger("Attack");
            _isAttacking = true;

            // Enemy cannot move when he's attacking
            selfNavMesh.speed = 0.0f;
        }
        else
        {
            // Enemy is not attacking so he has to get close to the player
            selfNavMesh.speed = 5.0f;
        }

        // Update movements animations
        selfAnimator.SetFloat("Speed", selfNavMesh.speed);
    }

    // Update is called once per frame
    void Update()
    {
        AIBehaviour();
        IsDieAnimationFinished();
    }
}
