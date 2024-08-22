using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class TutorialScript : MonoBehaviour {
   public MenuScript m;
   public GameObject tutorialCanvas;

   void Start() {
        if (PlayerPrefs.GetInt("FirstTime", 1) == 1) {
            PlayerPrefs.SetInt("FirstTime", 0);
            PlayerPrefs.Save();

            m.moveUI();
        } else {
            Destroy(tutorialCanvas);
        }
   }

   public void OnClicked(GameObject nextObject) {
        nextObject.SetActive(true);
   }

    public void destroyButton(GameObject previousObject) {
        previousObject.SetActive(false);

        if(previousObject.name == "Final") {
            print("End Tutorial");

            m.resetUI();

            Destroy(tutorialCanvas);
        }
    }
}
