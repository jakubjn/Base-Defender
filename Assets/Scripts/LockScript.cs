using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LockScript : MonoBehaviour {
    private GameObject Lock;
    private Button button;

    void Awake() {
        Lock = transform.Find("Lock").gameObject;
        button = gameObject.GetComponent<Button>();
    }

    public void Unlock() {
        Destroy(Lock);
        button.interactable = true;
    }
}
