using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MortarScript : MonoBehaviour {
    private GameObject[] enemies;

    public GameObject explosion;
    public float firerate;
    public Transform firePoint;

    private float fireTime;
    private Vector2 currDirection;
    
    private float tickTime;
    public float maintainanceCost;

    public float detectionRadius;
    public float damageRadius;
    public float damageAmount;

    public bool TurningMode;

    private bool rotating = false;
    private bool rotated = false;

    private Vector3 turnDir;
    private Quaternion rotation;
    private Quaternion currRotation;

    private float timeCount = 0.0f;

    void Start() {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");

        tickTime = 15;
        tickTime = Time.time+tickTime;
    }

    void Update() {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");

        if(rotating) {
            transform.rotation = Quaternion.Slerp(currRotation,rotation,timeCount * 0.35f);
            timeCount = timeCount + Time.deltaTime;

            if(timeCount > 3.5f) {
                rotating = false;
                rotated = true;
                timeCount = 0.0f;

                print("Finished");
            }

            return;
        }

        if(enemies.Length < 1) return;

        GameObject enemy = findClosestEnemy();

        if(TurningMode && !rotated) { 
            rotating = true; 
            turnDir = enemy.transform.position - transform.position;

            rotation = Quaternion.LookRotation(turnDir,Vector3.back);
            currRotation = transform.rotation;

            rotation.Set(transform.rotation.x, transform.rotation.y , rotation.z, rotation.w);

            return;
        } else if(!TurningMode) {
            lookAt(enemy.transform.position,transform);
        }

        if(Time.time > fireTime) {
            print("Firing");

            fireTime = Time.time+firerate;
            fire(enemy.transform);

            rotated = false;
        }
    }

    private GameObject findClosestEnemy() {
        if(enemies.Length < 2) return enemies[0];

        float closestDist = 10000;
        GameObject closestEnemy = enemies[0];

        for (int i = 0; i < enemies.Length; i++) {
            GameObject enemy = enemies[i];
            float dist = (transform.position - enemy.transform.position).magnitude;

            if(dist < closestDist) {
                closestDist = dist;
                closestEnemy = enemy;
            }
        }

        return closestEnemy;
    }

    private void lookAt(Vector3 pos, Transform transform) {
        Vector3 diff = pos - this.transform.position;
        diff.Normalize();

        float rot_z = Mathf.Atan2(diff.y,diff.x) * Mathf.Rad2Deg;

        this.transform.rotation = Quaternion.Euler(0f,0f,rot_z-90);
    }

    private void fire(Transform transform) {
        GameObject explosionC = Instantiate(explosion);
        explosionC.transform.position = transform.position + new Vector3(Random.Range(-5,5),Random.Range(-5,5),0);

        Destroy(explosionC,3f);

         for (int i = 0; i < enemies.Length; i++) {
            GameObject enemy = enemies[i];

            if ((enemy.transform.position - transform.position).magnitude < damageRadius) {
                HealthScript m = enemy.GetComponent<HealthScript>();
                m.Damage(damageAmount);
            }
         }
    }
}
