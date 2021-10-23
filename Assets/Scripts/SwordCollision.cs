using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordCollision : MonoBehaviour
{
    [SerializeField]
    private PlayerController player;

    public void OnTriggerEnter(Collider collision)
    {
        EnemyHandle enemy = collision.gameObject.GetComponent<EnemyHandle>();
        if (enemy != null && (player.isAttacking || player.isAttackingSprint))
        {
            enemy.Die();
        }
    }
}
