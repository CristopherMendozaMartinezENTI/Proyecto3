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
    [SerializeField] private Vector3 webCastPos;

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
        Vector3 D = new Vector3(startPos.position.x + webCastPos.x, startPos.position.y + webCastPos.y, startPos.position.z + webCastPos.z);
        Vector3 B = A + endPos.up * (-(A - D).magnitude * Random.Range(0.1f, offset1));
        Vector3 C = D + startPos.up * ((A - D).magnitude * Random.Range(0.1f, offset2));
        BezierCurve.GetBezierCurve(A, B, C, D, allWebSections);
        Vector3[] positions = new Vector3[allWebSections.Count];
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