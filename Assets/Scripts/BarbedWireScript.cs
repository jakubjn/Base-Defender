using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarbedWireScript : MonoBehaviour
{
    public float movementDecrease;

    private float savedSpeed;

    private void OnTriggerEnter2D(Collider2D collider) {
        if(collider.gameObject.tag != "Enemy") return;

        EnemyMainHandler m = collider.gameObject.GetComponent<EnemyMainHandler>();

        savedSpeed = m.movementSpeed;

        m.movementSpeed = m.movementSpeed/movementDecrease;
    }

    private void OnTriggerExit2D(Collider2D collider) {
        if(collider.gameObject.tag != "Enemy") return;

        EnemyMainHandler m = collider.gameObject.GetComponent<EnemyMainHandler>();

        m.movementSpeed = savedSpeed;
    }
}
