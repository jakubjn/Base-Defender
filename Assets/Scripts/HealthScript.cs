using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthScript : MonoBehaviour
{
    public float totalHealth;

    [HideInInspector]
    public BuildingHandler m;
    [HideInInspector]
    public EnemySpawner f;
    [HideInInspector]
    public float health;

    void Start() {
        health = totalHealth;
        m = GameObject.Find("BuildingManager").GetComponent<BuildingHandler>();
        f = GameObject.Find("EnemySpawner").GetComponent<EnemySpawner>();
    }

    public void Damage(float amount) {
        health = health - amount;

        if(health > 0) return;

        if(gameObject.tag == "Enemy") {
            f.waitForEnemies(gameObject);
        }

        Destroy(gameObject);
    }
    
    public virtual void OnTriggerEnter2D(Collider2D collider) {
        if(collider.gameObject.tag != "Projectile") return;

        if(collider.gameObject.name == "EnemyProjectile" && gameObject.tag == "Enemy") return;

        health = health - 5;

        Destroy(collider.gameObject);

        if(health > 0) return;

        if(gameObject.tag == "Enemy") {
            f.waitForEnemies(gameObject);
        }

        Destroy(gameObject);
    }
}
