using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class OilWellScript : MonoBehaviour
{
    public float tickTime;
    public float amountAdded;

    private BuildingHandler m;

    void Start() {
        m = GameObject.Find("BuildingManager").GetComponent<BuildingHandler>();
        tickTime = Time.time+tickTime;
    }

    void Update() {
       if(Time.time > tickTime) {
            tickTime = Time.time+tickTime;

            m.currentBalance = m.currentBalance + amountAdded;
            m.updateFundsDisplay();
        } 
    }
}
