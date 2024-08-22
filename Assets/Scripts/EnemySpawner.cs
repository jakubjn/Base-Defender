using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    private List<GameObject> enemies;
    private List<Vector3> spawns;

    private GameObject main;

    public GameObject enemy1;
    public GameObject enemy2;
    public GameObject enemy3;

    public Sprite arrowSprite;

    public float difficultyScale;

    private int currLevel;
    private int baseAmount;

    void Start() {
        spawns = new List<Vector3>();
        enemies = new List<GameObject>();

        main = GameObject.Find("Main");

        currLevel = 0;
        baseAmount = 3;

        foreach(Transform child in transform) {
            spawns.Add(child.position);
        }
    }

    public void RespawnWave() {
        int amount = baseAmount + currLevel*(int)difficultyScale;

        Vector3 spawn = spawns[Random.Range(1,5)];
        Vector3 spawn2 = new Vector3(0,0,0);
        Vector3 spawn3 = new Vector3(0,0,0);

        createArrow(spawn);

        if(currLevel >= 5) {
            spawn2 = spawns[Random.Range(1,5)];

            createArrow(spawn2);
        } else if(currLevel >= 3) {
            spawn2 = spawns[Random.Range(1,5)];

            createArrow(spawn2);
        }

        StartCoroutine(spawnDelay(spawn,amount,spawn2,spawn3));
    }

    private IEnumerator spawnDelay(Vector3 spawn, float amount, Vector3 spawn2, Vector3 spawn3) {
        yield return new WaitForSecondsRealtime(15f);

        spawnFunction(spawn,amount);
        if(spawn2 != new Vector3(0,0,0)) spawnFunction(spawn2,amount);
        if(spawn3 != new Vector3(0,0,0)) spawnFunction(spawn3,amount);

        currLevel = currLevel + 1;
    }

    private void spawnFunction(Vector3 spawn, float amount) {
        for(int i = 0; i < amount; i++) {
            GameObject enemyC;

            if (currLevel >= 4) {
                float num = Random.Range(1,100);

                if(num <= 10) { 
                    enemyC = Instantiate(enemy3);
                } else if(num <= 40) {
                    enemyC = Instantiate(enemy2);
                } else {
                    enemyC = Instantiate(enemy1);
                }
            } else if(currLevel >= 2) {
                float num = Random.Range(1,100);

                if(num <= 30) { 
                    enemyC = Instantiate(enemy2);
                } else {
                    enemyC = Instantiate(enemy1);
                }
            } else {
                enemyC = Instantiate(enemy1);
            }

            enemyC.transform.position = spawn + new Vector3(Random.Range(-30,30),Random.Range(-20,20),0);
            enemyC.transform.position = new Vector3(enemyC.transform.position.x,enemyC.transform.position.y,0f);
            enemies.Add(enemyC);
        }
    }

    public void waitForEnemies(GameObject enemy) {
        enemies.Remove(enemy);

        if(enemies.Count > 0) return;

        RespawnWave();
    }

    public void reset() {
        foreach(GameObject enemy in enemies) {
            Destroy(enemy);
        }

        currLevel = 0;
    }

    private void createArrow(Vector3 spawn) {
        Vector3 arrowPosition = new Vector3((main.transform.position.x + spawn.x)/2,(main.transform.position.y + spawn.y)/2,-5f);

        GameObject arrow = Instantiate(new GameObject());
        arrow.name = "Arrow";
        arrow.transform.position = arrowPosition;
        arrow.transform.LookAt(main.transform);

        SpriteRenderer spr = arrow.AddComponent<SpriteRenderer>();
        spr.sprite = arrowSprite;

        if(arrow.transform.rotation.y > 0) spr.flipX = true;

        Destroy(arrow,3f);
    }
}
