using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SwordCollision : MonoBehaviour
{
    public UnityEvent<GameObject> CollisionDetected;

    public void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Ennemies"))
        {
            CollisionDetected.Invoke(collision.gameObject);
        }
    }
}
