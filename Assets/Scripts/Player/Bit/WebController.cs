using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebController : MonoBehaviour
{
    public Transform startPos;
    public Transform endPos;
    private LineRenderer lineRenderer;
    private List<Vector3> allWebSections = new List<Vector3>();

    public float offset1;
    public float offset2;

    void Start()
    {
        lineRenderer = Camera.main.GetComponent<LineRenderer>();
    }

    private void Update()
    {
        if(endPos != null && startPos != null)
        {
            DisplayWeeb();
        }
    }

    private void DisplayWeeb()
    {
        float ropeWidth = 0.01f;
        lineRenderer.startWidth = ropeWidth;
        lineRenderer.endWidth = ropeWidth;
        Vector3 A = endPos.position;
        Vector3 D = startPos.position;
        Vector3 B = A + endPos.up * (-(A - D).magnitude * 0.1f);
        Vector3 C = D + startPos.up * ((A - D).magnitude * 0.5f);
        BezierCurve.GetBezierCurve(A, B, C, D, allWebSections);
        Vector3[] positions = new Vector3[allWebSections.Count];

        for (int i = 0; i < allWebSections.Count; i++)
        {
            positions[i] = allWebSections[i];
        }

        positions[0].y = startPos.transform.position.y  + 0.1f;
        lineRenderer.positionCount = positions.Length;
        lineRenderer.SetPositions(positions);
    }
}