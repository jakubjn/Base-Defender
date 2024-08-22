using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.Mathematics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class CanonScript : MonoBehaviour {
    private GameObject[] enemies;
    public float offset;

    public GameObject projectile;
    public float firerate;
    public Transform firePoint;

    private float fireTime;
    private Vector2 currDirection;

    public float detectionRadius;

    private float tickTime;
    public float maintainanceCost;

    private Animator anim;

    private Vector3 savedPos;

    private bool Animating;

    private bool Switch = false;
    private bool Loading = false;

    private LayerMask ignoreLayer;

    void Awake() {
        savedPos = transform.localPosition;
        ignoreLayer = LayerMask.GetMask("UserLayer6","CanonIgnore");
    }

    void Start() {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");

        tickTime = 15;
        tickTime = Time.time+tickTime;
    }

    void Update() {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");

        if(enemies.Length < 1) {
            if(Animating) StopCoroutine(GunAnim()); Animating = false;
            return;
        }

        GameObject enemy = findClosestEnemy();

        if(!checkIfColliding(enemy.transform)) { 
            if(Animating) StopCoroutine(GunAnim()); Animating = false;
            return;
        }

        if(Animating == false) Animating = true; StartCoroutine(GunAnim());

        lookAt(enemy.transform.position,transform);

        if(Time.time > fireTime) {
            fireTime = Time.time+1/firerate;
            fire(currDirection);
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
        rot_z = rot_z + offset;

        this.transform.rotation = Quaternion.Euler(0f,0f,rot_z-90);
    }

    private bool checkIfColliding(Transform target) {
        Vector2 Direction = (Vector2)target.position - (Vector2)transform.position;

        RaycastHit2D rayInfo = Physics2D.Raycast(transform.position,Direction,detectionRadius, ~ignoreLayer);

        if(rayInfo && rayInfo.collider.gameObject.tag == "Enemy") {
            currDirection = Direction;
            return true;
        }

        return false;
    }

    private IEnumerator GunAnim() {
        if(Loading) yield break;

        while(Animating) {
            Loading = true;

            if(Switch == false) {
                Switch = true;
                Vector3 localMovement = new Vector3(0, -0.5f, 0);
                Vector3 moveVector = transform.rotation * localMovement;

                transform.Translate(moveVector , Space.World);
            } else {
                Switch = false;
                transform.localPosition = savedPos;
            }

            yield return new WaitForSeconds(0.05f);
        }

        Loading = false;
    }

    private void fire(Vector2 direction) {
        GameObject bulletIns = Instantiate(projectile,firePoint.position,transform.rotation * quaternion.RotateZ(90 * Mathf.Deg2Rad));
        bulletIns.GetComponent<Rigidbody2D>().AddForce(direction * 150f);

        Destroy(bulletIns,2f);
    }
}
