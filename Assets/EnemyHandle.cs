using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHandle : MonoBehaviour
{
    public Animator selfAnimator;
    public ParticleSystem bloodDropFX;

    private bool isDie;

    // Start is called before the first frame update
    void Start()
    {
        isDie = false;
    }

    public void Die()
    {
        selfAnimator.SetBool("IsDead", true);
        bloodDropFX.Play();
        isDie = true;
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
        IsDieAnimationFinished();
    }
}
