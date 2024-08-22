using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PullInScript : MonoBehaviour, IPointerExitHandler {
    private Vector3 position;

    private float tweenDuration = 0.3f;

    private void Start() {
        position = transform.position;
    }

    public void OnPointerExit(PointerEventData data) {
        StartCoroutine(AnimateUIElement());
    }

    IEnumerator AnimateUIElement() {
        Vector3 initialPosition = gameObject.transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < tweenDuration)
        {
            gameObject.transform.position = Vector3.Lerp(initialPosition, position, elapsedTime / tweenDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        gameObject.transform.position = position;
    }
}
