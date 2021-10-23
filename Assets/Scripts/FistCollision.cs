using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FistCollision : MonoBehaviour
{
    public EnemyHandle enemyHandle;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && enemyHandle.isAttacking)
        {
            Animator playerAnimator = other.GetComponent<Animator>();
            playerAnimator.SetTrigger("Die");
            playerAnimator.SetBool("IsDead", true);
            EnemyManager.instance.endGame = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
