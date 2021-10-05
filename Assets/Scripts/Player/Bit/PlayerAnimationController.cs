using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Transform[] legTargets;
    [Header("Options")]
    [SerializeField] private int smoothness = 3;
    [SerializeField] private int numberOfLegs;
    [SerializeField] private float stepSize = 0.5f;
    [SerializeField] private float stepHeight = 0.015f;
    [SerializeField] private float sphereCastRadius = 0.125f;
    [SerializeField] private float raycastRange = 1.5f;
    [SerializeField] private float velocityMultiplier = 15.0f;
    private bool[] legMoving;
    private Vector3[] defaultLegPositions;
    private Vector3[] lastLegPositions;
    private Vector3 velocity;
    private Vector3 lastVelocity;
    private Vector3 lastBodyPos;

    private void Start()
    {
        numberOfLegs = legTargets.Length;
        defaultLegPositions = new Vector3[numberOfLegs];
        lastLegPositions = new Vector3[numberOfLegs];
        legMoving = new bool[numberOfLegs];
        for (int i = 0; i < numberOfLegs; ++i)
        {
            defaultLegPositions[i] = legTargets[i].localPosition;
            lastLegPositions[i] = legTargets[i].position;
            legMoving[i] = false;
        }
        lastBodyPos = transform.position;
    }

    private void FixedUpdate()
    {
        velocity = transform.position - lastBodyPos;
        velocity = (velocity + smoothness * lastVelocity) / (smoothness + 1f);

        if (velocity.magnitude < 0.000025f)
            velocity = lastVelocity;
        else
            lastVelocity = velocity;
        
        Vector3[] desiredPositions = new Vector3[numberOfLegs];
        int indexToMove = -1;
        float maxDistance = stepSize;
        for (int i = 0; i < numberOfLegs; ++i)
        {
            desiredPositions[i] = transform.TransformPoint(defaultLegPositions[i]);

            float distance = Vector3.ProjectOnPlane(desiredPositions[i] + velocity * velocityMultiplier - lastLegPositions[i], transform.up).magnitude;
            if (distance > maxDistance)
            {
                maxDistance = distance;
                indexToMove = i;
            }
        }
        for (int i = 0; i < numberOfLegs; ++i)
            if (i != indexToMove)
                legTargets[i].position = lastLegPositions[i];

        if (indexToMove != -1 && !legMoving[0])
        {
            Vector3 targetPoint = desiredPositions[indexToMove] + Mathf.Clamp(velocity.magnitude * velocityMultiplier, 0.0f, 1.5f) * (desiredPositions[indexToMove] - legTargets[indexToMove].position) + velocity * velocityMultiplier;

            Vector3[] positionAndNormalFwd = alignWithSurface(targetPoint + velocity * velocityMultiplier, raycastRange, (transform.parent.up - velocity * 100).normalized);
            Vector3[] positionAndNormalBwd = alignWithSurface(targetPoint + velocity * velocityMultiplier, raycastRange*(1f + velocity.magnitude), (transform.parent.up + velocity * 75).normalized);
            
            legMoving[0] = true;
            
            if (positionAndNormalFwd[1] == Vector3.zero)
            {
                StartCoroutine(performStep(indexToMove, positionAndNormalBwd[0]));
            }
            else
            {
                StartCoroutine(performStep(indexToMove, positionAndNormalFwd[0]));
            }
        }
        lastBodyPos = transform.position;
    }

    //This funtions allows the legs to align with the surface above
    private Vector3[] alignWithSurface(Vector3 point, float halfRange, Vector3 up)
    {
        Vector3[] res = new Vector3[2];
        res[1] = Vector3.zero;
        RaycastHit hit;
        Ray ray = new Ray(point + halfRange * up / 2.0f, -up);

        if (Physics.SphereCast(ray, sphereCastRadius, out hit, 2.0f * halfRange))
        {
            res[0] = hit.point;
            res[1] = hit.normal;
        }
        else
        {
            res[0] = point;
        }
        return res;
    }

    //This funtion allows leg movement
    private IEnumerator performStep(int index, Vector3 targetPoint)
    {
        Vector3 startPos = lastLegPositions[index];
        for (int i = 1; i <= smoothness; ++i)
        {
            legTargets[index].position = Vector3.Lerp(startPos, targetPoint, i / (float)(smoothness + 1.0f));
            legTargets[index].position += transform.up * Mathf.Sin(i / (float)(smoothness + 1.0f) * Mathf.PI) * stepHeight;
            yield return new WaitForFixedUpdate();
        }
        legTargets[index].position = targetPoint;
        lastLegPositions[index] = legTargets[index].position;
        legMoving[0] = false;
    }

    //HAY TENER EL GIZMO SIEMPRE ACTIVO AL DEBUGGAR POR FAVOR QUE NO SE TE OLVIDE!!!!!!!!!!!
    private void OnDrawGizmos()
    {
        for (int i = 0; i < numberOfLegs; ++i)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(legTargets[i].position, sphereCastRadius);
        }
    }
}
