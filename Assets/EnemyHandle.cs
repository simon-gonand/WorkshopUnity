using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHandle : MonoBehaviour
{
    public Transform selfHips;
    public Animator selfAnimator;
    public NavMeshAgent selfNavMesh;
    public ParticleSystem bloodDropFX;

    private bool isDie;
    private Transform player;

    // Start is called before the first frame update
    void Start()
    {
        isDie = false;
        player = PlayerController.instance.self;
    }

    public void Die()
    {
        selfAnimator.SetBool("IsDead", true);
        bloodDropFX.Play();
        isDie = true;
        --EnemyManager.instance.nbEnemies;
    }

    private void IsDieAnimationFinished()
    {
        if (selfAnimator.GetBehaviours<AnimationFinishedBehaviour>()[0].animationIsFinished && isDie)
        {
            Destroy(gameObject);
        }
    }

    private void AIBehaviour()
    {
        if (isDie) return;

        selfNavMesh.SetDestination(player.position);
        if (Vector3.Distance(player.position, selfHips.position) < 1.5f)
        {
            int randomAttackIndex = Random.Range(0, 6);
            selfAnimator.SetFloat("RandomAttack", randomAttackIndex);
            selfAnimator.SetTrigger("Attack");
            selfNavMesh.speed = 0.0f;
            selfAnimator.SetFloat("Speed", selfNavMesh.speed);
            Animator playerAnimator = player.GetComponent<Animator>();
            if (!playerAnimator.GetBool("IsDead")) {
                playerAnimator.SetTrigger("Die");
                playerAnimator.SetBool("IsDead", true);
            }
        }
        else
        {
            selfNavMesh.speed = 5.0f;
            selfAnimator.SetFloat("Speed", selfNavMesh.speed);
        }
    }

    // Update is called once per frame
    void Update()
    {
        AIBehaviour();
        IsDieAnimationFinished();
    }
}
