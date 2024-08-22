using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class EnemyHandler : EnemyMainHandler {
    private Vector3 direction;
    private Animator anim;

    void Start() {
        main = GameObject.Find("Main");
        defences = GameObject.FindGameObjectsWithTag("Defence");
        projectile = GameObject.Find("Projectile");

        anim = transform.GetComponent<Animator>();

        firerate = 5;
    }

    
    void Update() {
        defences = GameObject.FindGameObjectsWithTag("Defence");

        if(defences.Length < 1) return;

        if(!checkDetectionRadius(transform)) {
            var step =  movementSpeed * Time.deltaTime; 
            transform.position = Vector3.MoveTowards(transform.position, main.transform.position, step);
            lookAt(main.transform.position,transform);

            anim.SetBool("Firing",false);
        } else {
            GameObject defence = findClosestDefence(transform);
            lookAt(defence.transform.position,transform);
            direction = (Vector2)defence.transform.position - (Vector2)transform.position;

            anim.SetBool("Firing",true);

            if(Time.time < fireTime) return;

            fireTime = Time.time+1/firerate;

            fire();
        }
    }

    override public void fire() {
        GameObject bulletIns = Instantiate(projectile,firePoint.position,transform.rotation * quaternion.RotateZ(90 * Mathf.Deg2Rad));
        bulletIns.GetComponent<Rigidbody2D>().AddForce(direction * 150f);
        bulletIns.name = "EnemyProjectile";

        Destroy(bulletIns,5f);
    }
}
