using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuildingHandler : MonoBehaviour
{
    private GameObject buildingPrefab;
    private GameObject toBuild;

    public float currentBalance;
    public GameObject balanceDisplay;

    private void Awake() {
        buildingPrefab = null;
        currentBalance = 1000f;

        updateFundsDisplay();
    }

    private void Update() {
        if(buildingPrefab == null) return;

        if(Input.GetMouseButtonDown(1)) {
            Destroy(toBuild);
            toBuild = null;
            buildingPrefab = null;
            return;
        }

        Vector2 mouseInput = Input.mousePosition;
        Vector3 mouse = Camera.main.ScreenToWorldPoint(new Vector3(mouseInput.x,mouseInput.y,45.4f));
        mouse.z = -0f;

        if(!toBuild.activeSelf) toBuild.SetActive(true);

        toBuild.transform.position = mouse;

        if(EventSystem.current.IsPointerOverGameObject()) {
            if(toBuild.activeSelf) toBuild.SetActive(false);
        }

        if(Input.GetKey(KeyCode.R)) {
            toBuild.transform.Rotate(Vector3.forward,1f);
        }

        if(Input.GetKey(KeyCode.Q) && !Input.GetKey(KeyCode.R)) {
            toBuild.transform.Rotate(Vector3.forward,-1f);
        }

        if(Input.GetMouseButtonDown(0)) {
            BuildingManager m = toBuild.GetComponent<BuildingManager>();
            if(!m.hasValidPlacement) return;

            GameObject main = Instantiate(m.mainObject);
            main.transform.position = toBuild.transform.position;
            main.transform.rotation = toBuild.transform.rotation;

            currentBalance = currentBalance - m.buildingCost;
            updateFundsDisplay();

            m.SetPlacementMode(PlacementMode.Fixed);
            Destroy(toBuild);

            buildingPrefab = null;
            toBuild = null;
        }
    }

    public void SetBuildingPrefab(GameObject prefab) {
        BuildingManager m = prefab.GetComponent<BuildingManager>();

        if(currentBalance < m.buildingCost) return;

        buildingPrefab = prefab;
        prepareBuilding();
    }

    private void prepareBuilding() {
        if(toBuild) Destroy(toBuild);

        toBuild = Instantiate(buildingPrefab);
        toBuild.SetActive(false);

        BuildingManager m = toBuild.GetComponent<BuildingManager>();
        m.isFixed = false;
        m.SetPlacementMode(PlacementMode.Valid);
    }

    public void updateFundsDisplay() {
        TextMeshProUGUI text = balanceDisplay.GetComponent<TextMeshProUGUI>();
        text.text = "$" + currentBalance;
    }

    public void reset() {
        GameObject[] buildings = GameObject.FindGameObjectsWithTag("Defence");

        for(int i = 0; i < buildings.Length; i++) {
            GameObject building = buildings[i];

            if(building.name == "Main") continue;

            Destroy(building);
        }

        currentBalance = 500f;
        updateFundsDisplay();

        if(buildingPrefab) {
           Destroy(toBuild);
           toBuild = null;
           buildingPrefab = null;
        }
    }
}
