using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScript : MonoBehaviour
{
    public GameObject UI;

    private Vector3 savedPosition;

    private EnemySpawner m;

    void Awake() {
        savedPosition = UI.transform.position;
    }

    private void Start() {
        m = GameObject.Find("EnemySpawner").GetComponent<EnemySpawner>();
    }

    public void OnClicked() {
        moveUI();

        m.RespawnWave();
    }

    public void moveUI() {

        UI.transform.position = UI.transform.position + new Vector3(300,0,0);
    }

    public void resetUI() {
        UI.transform.position = savedPosition;
    }
}
