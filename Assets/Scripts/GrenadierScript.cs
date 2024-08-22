using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class GrenadierScript : EnemyMainHandler {
    private Animator anim;
    private Vector3 direction;

    public GameObject explosion;

    public float damageAmount;
    public float damageRadius;

    void Start() {
        main = GameObject.Find("Main");
        defences = GameObject.FindGameObjectsWithTag("Defence");
        projectile = GameObject.Find("GrenadeProjectile");

        anim = transform.GetComponent<Animator>();

        firerate = 3;
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

            anim.SetBool("Firing",true);

            if(Time.time < fireTime) return;

            fireTime = Time.time+firerate;
            direction = (Vector2)defence.transform.position - (Vector2)transform.position;

            fire();
        }
    }

    private void damageInRadius(Transform transform) {
        for (int i = 0; i < defences.Length; i++) {
            GameObject defence = defences[i];

            if ((defence.transform.position - transform.position).magnitude < damageRadius) {
                HealthScript m = defence.GetComponent<HealthScript>();

                if(m == null) return;

                m.Damage(damageAmount);

                print("Damaging");
            }
         }
    }

    private IEnumerator AnimationWait() {
        yield return new WaitForSeconds(0.5f); 

        GameObject bulletIns = Instantiate(projectile,firePoint.position,transform.rotation * quaternion.RotateZ(90 * Mathf.Deg2Rad));
        bulletIns.GetComponent<Rigidbody2D>().AddForce(direction * 90f);
        bulletIns.name = "EnemyGrenade";

        yield return new WaitForSeconds(1.5f); 

        GameObject explosionC = Instantiate(explosion);
        explosionC.transform.position = bulletIns.transform.position;

        Destroy(explosionC,1.5f);

        damageInRadius(bulletIns.transform);

        Destroy(bulletIns);
    }

    override public void fire() {
        anim.SetTrigger("ThrowGrenade");

        StartCoroutine(AnimationWait());
    }
}
