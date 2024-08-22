using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tooltip : MonoBehaviour {
    public string message;

    private void OnMouseEnter() {
        if(gameObject.GetComponent<LOScript>()) {
            gameObject.GetComponent<LOScript>().showLOS();
        }

        if(message != string.Empty) {
            TooltipManager._instance.SetAndShowTooltip(message);
        }
    }

    private void OnMouseExit() {
        if(gameObject.GetComponent<LOScript>()) {
            gameObject.GetComponent<LOScript>().hideLOS();
        }

        if(message != string.Empty) {
            TooltipManager._instance.HideTooltip();
        }
    }
}
