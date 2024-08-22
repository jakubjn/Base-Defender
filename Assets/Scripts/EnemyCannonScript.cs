using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class EnemyCannonScript : MonoBehaviour {
    private GameObject[] defences;
    public float offset;

    public GameObject projectile;
    public float firerate;
    public Transform firePoint;

    public LayerMask ignoreLayer;

    private float fireTime;
    private Vector2 currDirection;

    public float detectionRadius;

    private float tickTime;

    void Start() {
        defences = GameObject.FindGameObjectsWithTag("Defence");

        tickTime = 15;
        tickTime = Time.time+tickTime;
    }

    void Update() {
        defences = GameObject.FindGameObjectsWithTag("Defence");

        if(defences.Length < 1) return;

        GameObject defence = findClosestDefence();

        if(!checkIfColliding(defence.transform)) { 
            return;
        }

        lookAt(defence.transform.position,transform);

        if(Time.time > fireTime) {
            fireTime = Time.time+1/firerate;
            fire(currDirection);
        }

    }
    private GameObject findClosestDefence() {
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

    private void lookAt(Vector3 pos, Transform transform) {
        Vector3 diff = pos - this.transform.position;
        diff.Normalize();

        float rot_z = Mathf.Atan2(diff.y,diff.x) * Mathf.Rad2Deg;
        rot_z = rot_z + offset;

        this.transform.rotation = Quaternion.Euler(0f,0f,rot_z-90);
    }

    private bool checkIfColliding(Transform target) {
        Vector2 Direction = (Vector2)target.position - (Vector2)transform.position;

        RaycastHit2D rayInfo = Physics2D.Raycast(transform.position,Direction , detectionRadius, ~ignoreLayer);

        if(rayInfo && rayInfo.collider.gameObject.tag == "Defence") {
            currDirection = Direction;
            return true;
        }

        return false;
    }

    private void fire(Vector2 direction) {
        GameObject bulletIns = Instantiate(projectile,firePoint.position,transform.rotation * quaternion.RotateZ(90 * Mathf.Deg2Rad));
        bulletIns.GetComponent<Rigidbody2D>().AddForce(direction * 150f);
        bulletIns.name = "EnemyProjectile";

        Destroy(bulletIns,2f);
    }
}
