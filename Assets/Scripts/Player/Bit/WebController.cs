using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebController : MonoBehaviour
{
    private Transform startPos;
    private Transform endPos;
    private LineRenderer lineRenderer;
    private List<Vector3> allWebSections = new List<Vector3>();

    [Header("Options")]
    [SerializeField] private float offset1 = 0.5f;
    [SerializeField] private float offset2 = 0.5f;

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
        Vector3 B = A + endPos.up * (-(A - D).magnitude * offset1);
        Vector3 C = D + startPos.up * ((A - D).magnitude * offset2);
        BezierCurve.GetBezierCurve(A, B, C, D, allWebSections);

        Vector3[] positions = new Vector3[allWebSections.Count];

        /*
        positions[10].x = startPos.transform.position.y;
        positions[10].y = startPos.transform.position.y + 0.1f;
        positions[10].z = startPos.transform.position.z;
        */

        for (int i = 0; i < allWebSections.Count; i++)
        {
            positions[i] = allWebSections[i];
        }

        lineRenderer.positionCount = positions.Length;
        lineRenderer.SetPositions(positions);
    }

    public void setStartPos(Transform pos)
    {
        startPos = pos;
    }

    public void setEndPos(Transform pos)
    {
        endPos = pos;
    }
}