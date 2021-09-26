using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float speed = 3.0f;
    private float smoothness = 5.0f;
    private int raysNb = 8;
    private float raysEccentricity = 0.2f;
    private float outerRaysOffset = 2.0f;
    private float innerRaysOffset = 25.0f;
    private Vector3 velocity;
    private Vector3 lastVelocity;
    private Vector3 lastPosition;
    private Vector3 forward;
    private Vector3 upward;
    private Quaternion lastRot;     
    private Vector3[] pn;

    private void Start()
    {
        velocity = new Vector3();
        forward = transform.forward;
        upward = transform.up;
        lastRot = transform.rotation;
    }

    private void FixedUpdate()
    {
        velocity = (smoothness * velocity + (transform.position - lastPosition)) / (1f + smoothness);
        if (velocity.magnitude < 0.00025f)
            velocity = lastVelocity;
        lastPosition = transform.position;
        lastVelocity = velocity;
        float multiplier = 1.0f;
    
        //Run
        if (Input.GetKey(KeyCode.LeftShift))
            multiplier = 2.0f;

        //Move Foward
        bool moveFoward = Input.GetKey(KeyCode.W);
        if (moveFoward != false)
            transform.position += transform.forward * speed * multiplier * Time.fixedDeltaTime;

        //Turn Backwards
        bool moveBackward = Input.GetKey(KeyCode.S);
        if (moveBackward != false)
            transform.position += -transform.forward * speed * multiplier * Time.fixedDeltaTime;

        //Turn left or Right     
        float valueX = Input.GetAxis("Horizontal");
        if (valueX != 0)
           transform.position += Vector3.Cross(transform.up, transform.forward) * valueX * speed * multiplier * Time.fixedDeltaTime;

        if (moveFoward != false || moveBackward != false || valueX != 0)
        {
            pn = getClosestPoint(transform.position, transform.forward, transform.up, 0.5f, 0.1f, 30, -30, 4);
            upward = pn[1];
            Vector3[] pos = getClosestPoint(transform.position, transform.forward, transform.up, 0.5f, raysEccentricity, innerRaysOffset, outerRaysOffset, raysNb);
            transform.position = Vector3.Lerp(lastPosition, pos[0], 1f / (1f + smoothness));
            forward = velocity.normalized;
            Quaternion q = Quaternion.LookRotation(forward, upward);
            transform.rotation = Quaternion.Lerp(lastRot, q, 1f / (1f + smoothness));
        }
        lastRot = transform.rotation;
    }

    //This function allows the player to find the closest point near by and move towards it. This basically allows the player to walk on any surface posible
    private Vector3[] getClosestPoint(Vector3 point, Vector3 forward, Vector3 up, float halfRange, float eccentricity, float offset1, float offset2, int rayAmount)
    {
        Vector3[] res = new Vector3[2] { point, up };
        Vector3 right = Vector3.Cross(up, forward);
        float normalAmount = 1f;
        float positionAmount = 1f;

        Vector3[] dirs = new Vector3[rayAmount];
        float angularStep = 2f * Mathf.PI / (float)rayAmount;
        float currentAngle = angularStep / 2f;
        for (int i = 0; i < rayAmount; ++i)
        {
            dirs[i] = -up + (right * Mathf.Cos(currentAngle) + forward * Mathf.Sin(currentAngle)) * eccentricity;
            currentAngle += angularStep;
        }

        foreach (Vector3 dir in dirs)
        {
            RaycastHit hit;
            Vector3 largener = Vector3.ProjectOnPlane(dir, up);
            Ray ray = new Ray(point - (dir + largener) * halfRange + largener.normalized * offset1 / 100f, dir);
            
            if (Physics.SphereCast(ray, 0.01f, out hit, 2f * halfRange))
            {
                if(hit.transform.gameObject.tag == "Obstacle")
                {
                    res[0] += hit.point;
                    res[1] += hit.normal;
                    normalAmount += 1;
                    positionAmount += 1;
                }
            }
            ray = new Ray(point - (dir + largener) * halfRange + largener.normalized * offset2 / 100f, dir);
            if (Physics.SphereCast(ray, 0.01f, out hit, 2f * halfRange))
            {
                if (hit.transform.gameObject.tag == "Obstacle")
                {
                    res[0] += hit.point;
                    res[1] += hit.normal;
                    normalAmount += 1;
                    positionAmount += 1;
                }

            }
        }
        res[0] /= positionAmount;
        res[1] /= normalAmount;
        return res;
    }
}
