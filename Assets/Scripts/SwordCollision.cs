using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordCollision : MonoBehaviour
{
    [SerializeField]
    private PlayerController player;

    // Detect a collision on the sword of the player
    public void OnTriggerEnter(Collider collision)
    {
        // If the collision is on one of the enemies when the player is attacking
        EnemyHandle enemy = collision.gameObject.GetComponent<EnemyHandle>();
        if (enemy != null && (player.isAttacking || player.isAttackingSprint))
        {
            // Enemy dies
            enemy.Die();
        }
    }
}
