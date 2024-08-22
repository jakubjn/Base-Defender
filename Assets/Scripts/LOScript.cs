using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LOScript : MonoBehaviour
{
    public Transform trans;
    public float range;
    public float angle;

    public Material LOSMaterial;

    private LineRenderer line;

    private void Start() {
        createLOS();

        line.enabled = false;
    }

    private void createLOS() {
        AnimationCurve curve = new AnimationCurve();
        curve.AddKey(1f,1f);

        LineRenderer lr = gameObject.AddComponent<LineRenderer>();
        lr.widthCurve = curve;
        
        lr.positionCount = 3;

        Vector3[] gotPositions = getDirection();

        Vector3[] positions = new Vector3[3];
        positions[0] = trans.position;
        positions[1] = gotPositions[0];
        positions[2] = gotPositions[1];

        lr.SetPositions(positions);
        lr.material = LOSMaterial;
        lr.loop = true;

        line = lr;
    }

    private Vector3[] getDirection() {
        float rayRange = range;
        float halfFOV = angle / 2.0f;
        float coneDirection = 90;

        Quaternion upRayRotation = Quaternion.AngleAxis(-halfFOV + coneDirection, Vector3.forward);
        Quaternion downRayRotation = Quaternion.AngleAxis(halfFOV + coneDirection, Vector3.forward);

        Vector3 upRayDirection = upRayRotation * trans.right * rayRange;
        Vector3 downRayDirection = downRayRotation * trans.right * rayRange;

        Vector3[] positions = new Vector3[3];
        positions[0] = trans.position + upRayDirection;
        positions[1] = trans.position + downRayDirection;

        return positions;
    }

    public void showLOS() {
        line.enabled = true;
    }

    public void hideLOS() {
        line.enabled = false;
    }
}
