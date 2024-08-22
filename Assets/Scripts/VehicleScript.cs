using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class VehicleScript : MonoBehaviour {
    private GameObject[] defences;
    private Transform closestDefence;

    private float timeCount = 40;
    private Vector3 randomTransform;

    private bool crashed = false;

    public float crashDamage;
    public float speed;

    public GameObject explosion;

    void Start() {
        defences = GameObject.FindGameObjectsWithTag("Defence");

        closestDefence = findClosestDefence(transform).transform;

        InstantlookAt(closestDefence.transform.position,transform);
    }

    void Update() {
        if(crashed) return;

        moveTowardDefence(closestDefence);
    }

    void OnCollisionEnter2D(Collision2D collider) {
        if(collider.gameObject.tag != "Defence" || crashed) return;

        crashed = true;

        GameObject explosionC = Instantiate(explosion);
        explosionC.transform.position = collider.gameObject.transform.position;

        HealthScript m = collider.gameObject.GetComponent<HealthScript>();
        m.Damage(crashDamage);

        Destroy(explosionC,3f);
    }

    private void moveTowardDefence(Transform defence) {
        timeCount = timeCount + 1;

        var step =  speed * Time.deltaTime; 

        if(timeCount > 30) {
            randomTransform = defence.position + new Vector3(Random.Range(-3,3),Random.Range(-3,3),0);
            timeCount = 0;
        } 

        transform.position = Vector3.MoveTowards(transform.position, randomTransform, step);
        lookAt(randomTransform,transform);
    }

    private GameObject findClosestDefence(Transform transform) {
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
        Vector3 diff = pos - transform.position;
        diff.Normalize();

        float rot_z = Mathf.Atan2(diff.y,diff.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0f,0f,rot_z+90),0.005f);
    }

    private void InstantlookAt(Vector3 pos, Transform transform) {
        Vector3 diff = pos - transform.position;
        diff.Normalize();

        float rot_z = Mathf.Atan2(diff.y,diff.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0f,0f,rot_z+90);
    }
}
