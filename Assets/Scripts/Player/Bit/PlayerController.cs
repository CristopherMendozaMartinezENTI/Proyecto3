using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Options")]
    [SerializeField] private float speed = 8.0f;
    [SerializeField] private float smoothness = 5.0f;
    [SerializeField] private int numberOfRays = 8;
    [SerializeField] private float raysEccentricity = 0.5f;
    [SerializeField] private float outerRaysOffset = 42.0f;
    [SerializeField] private float innerRaysOffset = 20.0f;
    private bool onHook = false;
    private float speedMultiplier = 1.0f;
    private Vector3 velocity;
    private Vector3 lastVelocity;
    private Vector3 lastPosition;
    private Vector3 forward;
    private Vector3 upward;
    private Quaternion lastRot;     
    private Vector3[] pn;
    private GameObject camera;

    private void Start()
    {
        forward = transform.forward;
        upward = transform.up;
        lastRot = transform.rotation;
        lastPosition = transform.position;
        camera = GameObject.Find("Main Camera");
    }

    private void FixedUpdate()
    {
        velocity = (smoothness * velocity + (transform.position - lastPosition)) / (1.0f + smoothness);
        if (velocity.magnitude < 0.00025f)
            velocity = lastVelocity;
        lastPosition = transform.position;
        lastVelocity = velocity;
        speedMultiplier = 1.0f;

        if (onHook)
            speedMultiplier = 0.5f;

        //Slow Down
        if (Input.GetKey(KeyCode.LeftShift))
            speedMultiplier = 0.5f;

        //Move Around
        float valueY = Input.GetAxis("Vertical");
        if (valueY != 0)
            transform.position += transform.forward * valueY * speed * speedMultiplier * Time.fixedDeltaTime;

        float valueX = Input.GetAxis("Horizontal");
        if (valueX != 0)
            transform.position += Vector3.Cross(transform.up, transform.forward) * valueX * speed * speedMultiplier * Time.fixedDeltaTime;
        
        if(camera.transform.localRotation.y > 0.3f)
            transform.position += Vector3.Cross(transform.up, transform.forward) * 0.1f * speed * Time.fixedDeltaTime;

        if (camera.transform.localRotation.y < -0.3f)
            transform.position += Vector3.Cross(transform.up, transform.forward) * -0.1f * speed * Time.fixedDeltaTime;

        /*
        pn = getClosestPoint(transform.position, transform.forward, transform.up, 0.5f, 0.2f, 30, -30, 4);
        upward = pn[1];
        Vector3[] nextPos = getClosestPoint(transform.position, transform.forward, transform.up, 0.5f, raysEccentricity, innerRaysOffset, outerRaysOffset, numberOfRays);
        transform.position = Vector3.Lerp(lastPosition, nextPos[0], 1.0f / (1.0f + smoothness));
        forward = velocity.normalized;
        Quaternion lookAtRotation = Quaternion.LookRotation(forward, upward);
        transform.rotation = Quaternion.Lerp(lastRot, lookAtRotation, 1.0f / (1.0f + smoothness));
        lastRot = transform.rotation;
        */
    }

    //This function allows the player to find the closest point near by and move towards it. This basically allows the player to walk on any surface posible
    private Vector3[] getClosestPoint(Vector3 point, Vector3 forward, Vector3 up, float halfRange, float eccentricity, float offset1, float offset2, int rayAmount)
    {
        Vector3[] res = new Vector3[2] { point, up };
        Vector3 right = Vector3.Cross(up, forward);
        float normalAmount = 1.0f;
        float positionAmount = 1.0f;

        Vector3[] dirs = new Vector3[rayAmount];
        float angularStep = 2.0f * Mathf.PI / (float)rayAmount;
        float currentAngle = angularStep / 2.0f;
        for (int i = 0; i < rayAmount; ++i)
        {
            dirs[i] = -up + (right * Mathf.Cos(currentAngle) + forward * Mathf.Sin(currentAngle)) * eccentricity;
            currentAngle += angularStep;
        }

        foreach (Vector3 dir in dirs)
        {
            RaycastHit hit;
            Vector3 largener = Vector3.ProjectOnPlane(dir, up);
            Ray ray = new Ray(point - (dir + largener) * halfRange + largener.normalized * offset1 / 100.0f, dir);
            
            if (Physics.SphereCast(ray, 0.01f, out hit, 2f * halfRange))
            {
                Debug.DrawRay(ray.origin, ray.direction);
                if (hit.transform.gameObject.tag == "Obstacle")
                {
                    res[0] += hit.point;
                    res[1] += hit.normal;
                    normalAmount += 1;
                    positionAmount += 1;
                }
            }
            ray = new Ray(point - (dir + largener) * halfRange + largener.normalized * offset2 / 100.0f, dir);
            if (Physics.SphereCast(ray, 0.01f, out hit, 2f * halfRange))
            {
                Debug.DrawRay(ray.origin, ray.direction, Color.green);
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

    public void onHookTrue()
    {
        onHook = true; 
    }

    public void onHookFalse()
    {
        onHook = false;
    }
    
    public bool getOnHook()
    {
        return onHook;
    }
}
