using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class MainScript : HealthScript {
    public GameObject healthDisplay;

    public MenuScript menu;

    public override void OnTriggerEnter2D(Collider2D collider) {
       if(collider.gameObject.tag != "Projectile") return;

        health = health - 5;

        updateHealthDisplay();

        Destroy(collider.gameObject);

        if(health > 0) return;

        RestartGame();
    }

    private void RestartGame() {
        print("GAME OVER!");

        health = totalHealth;
        
        f.reset();
        menu.resetUI();
        m.reset();
    }

    public void updateHealthDisplay() {
        TextMeshProUGUI text = healthDisplay.GetComponent<TextMeshProUGUI>();
        text.text = "" + health;
    }
}
