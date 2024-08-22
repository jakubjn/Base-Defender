using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

public class DeletionTool : MonoBehaviour
{
    public Sprite cross;

    private Sprite current; 
    private bool selected;

    private Image render;

    private GameObject[] defences;

    void Awake() {
        selected = false;
        render = gameObject.GetComponent<Image>();
        current = render.sprite;

        defences = GameObject.FindGameObjectsWithTag("Defence");
    }
    
    public void OnClick() {
       if(selected) return;

       render.sprite = cross;
       gameObject.transform.localScale = new Vector3(0.9f,gameObject.transform.localScale.y,gameObject.transform.localScale.z);

       selected = true;
    }

    void Update() {
        if(!selected) return;

        defences = GameObject.FindGameObjectsWithTag("Defence");

        if(Input.GetMouseButtonDown(0)) {
            render.sprite = current;
            gameObject.transform.localScale = new Vector3(0.45f,gameObject.transform.localScale.y,gameObject.transform.localScale.z);

            for(int i = 0;i < defences.Length;i++) {
                if(!IsTouchingMouse(defences[i])) continue;

                if(defences[i].name == "Main") continue;

                Destroy(defences[i]);
            }

            selected = false;
        }
    }

    private bool IsTouchingMouse(GameObject g) {
        Vector2 mouseInput = Input.mousePosition;
        Vector3 point = Camera.main.ScreenToWorldPoint(new Vector3(mouseInput.x,mouseInput.y,45.4f));
        point.z = -5f;
        return g.GetComponent<Collider2D>().OverlapPoint(point);
    }
}
