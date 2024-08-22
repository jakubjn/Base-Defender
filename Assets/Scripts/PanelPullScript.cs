using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.EventSystems;

public class PanelPullScript : MonoBehaviour, IPointerEnterHandler {
    private GameObject frame;
    private Vector3 newPosition;

    private float tweenDuration = 0.3f;

    private void Start() {
        frame = gameObject.transform.parent.gameObject;
        newPosition = frame.transform.position + new Vector3(0,23,0);
    }

    public void OnPointerEnter(PointerEventData eventData) {
        StartCoroutine(AnimateUIElement());
    }

     IEnumerator AnimateUIElement() {
        Vector3 initialPosition = frame.transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < tweenDuration)
        {
            frame.transform.position = Vector3.Lerp(initialPosition, newPosition, elapsedTime / tweenDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        frame.transform.position = newPosition;
    }
}
