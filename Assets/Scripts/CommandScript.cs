using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using UnityEngine;

public class CommandScript : MonoBehaviour {
    private GameObject[] unlocks;

    public String tagName;

    void Start() {
        unlocks = GameObject.FindGameObjectsWithTag(tagName);

        for(int i = 0; i < unlocks.Length; i++) {
            LockScript m = unlocks[i].GetComponent<LockScript>();
            m.Unlock();
        }
    }
}
