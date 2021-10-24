using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FistCollision : MonoBehaviour
{
    [SerializeField]
    private EnemyHandle enemyHandle;

    // Detect a collision on one of the fists of the enemy
    private void OnTriggerEnter(Collider other)
    {
        // if the fist is entering in collision with the player during an attack
        if (other.CompareTag("Player") && enemyHandle.isAttacking)
        {
            // Update the animator of the player
            Animator playerAnimator = other.GetComponent<Animator>();
            playerAnimator.SetTrigger("Die");
            playerAnimator.SetBool("IsDead", true);

            // The player has died, the game is finished
            EnemyManager.instance.endGame = true;
        }
    }
}
