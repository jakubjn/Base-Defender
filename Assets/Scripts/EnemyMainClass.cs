using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyMainHandler: MonoBehaviour {
    [HideInInspector] public GameObject main;
    [HideInInspector] public GameObject[] defences;
    [HideInInspector] public GameObject projectile;

    [HideInInspector] public float fireTime;
    
    public float firerate;

    public float movementSpeed;
    public float detectionRadius;

    public Transform firePoint;

    public GameObject findClosestDefence(Transform transform) {
        if(defences.Length < 2) return defences[0];

        float closestDist = 10000;
        GameObject closestEnemy = defences[0];

        for (int i = 0; i < defences.Length; i++) {
            GameObject enemy = defences[i];
            float dist = (transform.position - enemy.transform.position).magnitude;

            if(dist < closestDist) {
                closestDist = dist;
                closestEnemy = enemy;
            }
        }

        return closestEnemy;
    }

    public bool checkDetectionRadius(Transform transform) {
        float dist = 1000000;

        for(int i = 0; i < defences.Length; i++) {
            float distance = (defences[i].transform.position - transform.position).magnitude;

            if(distance < dist) dist = distance;
        }

        if(dist < detectionRadius) return true;

        return false;
    }

    public void lookAt(Vector3 pos, Transform transform) {
        Vector3 diff = pos - transform.position;
        diff.Normalize();

        float rot_z = Mathf.Atan2(diff.y,diff.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0f,0f,rot_z-90);
    }

    public abstract void fire();
}
