using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCollision : MonoBehaviour
{
    [Header("Options")]
    [SerializeField] private float minDis = 1.0f;
    [SerializeField] private float maxDis = 2.0f;
    [SerializeField] private float smooth = 10.0f;
    private float distance;
    private Vector3 dollyDirection;

    private void Start()
    {
        dollyDirection = transform.localPosition.normalized;
        distance = transform.localPosition.magnitude;
    }

    private void Update()
    {
        Vector3 desiredPos = transform.parent.TransformPoint(dollyDirection * maxDis);
        RaycastHit hit;
        if (Physics.Linecast(transform.parent.position, desiredPos, out hit))
        {
            distance = Mathf.Clamp((hit.distance * 0.9f), minDis, maxDis);
        }
        else distance = maxDis;
        transform.localPosition = Vector3.Lerp(transform.localPosition, dollyDirection * distance, Time.fixedDeltaTime * smooth);
    }
}
