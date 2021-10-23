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

    // Update is called once per frame
    void Update()
    {
        if(!isDie)
            selfNavMesh.SetDestination(player.position);
        IsDieAnimationFinished();
    }
}
