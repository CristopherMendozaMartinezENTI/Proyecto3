using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookSystem : MonoBehaviour
{
    public float maxGrabDistance = 5.0f;

    private float currentGrabDistance;
    private Vector2 rotationInput;
    private Vector3 hitOffsetLocal;
    private Vector3 rotationDifferenceEuler;
    private RigidbodyInterpolation initialInterpolationSetting;
    private GameObject player;
    private Rigidbody grabbedRigidbody;
    private LineRenderer lineRenderer;

    private void Start()
    {
        player = GameObject.Find("Player");
        lineRenderer = this.gameObject.GetComponent<LineRenderer>();
        lineRenderer.startWidth = 0.01f;
        lineRenderer.endWidth = 0.01f;
        lineRenderer.positionCount = 2;
        lineRenderer.useWorldSpace = true;
    }

    private void Update()
    {
        //Here we are hovering on posible targets
        Ray rayHover = Camera.main.ViewportPointToRay(Vector3.one * 0.5f);
        RaycastHit hitHover;
        if (Physics.Raycast(rayHover, out hitHover, maxGrabDistance))
        {
            if (hitHover.rigidbody != null && !hitHover.rigidbody.isKinematic)
            {
               hitHover.transform.gameObject.GetComponent<CubeManager>().enableOutline();
            }
        }

        if (!Input.GetMouseButton(0))
        {
            // Reset the rigidbody to how it was before we grabbed it
            if (grabbedRigidbody != null)
            {
                lineRenderer.positionCount = 2;
                grabbedRigidbody.interpolation = initialInterpolationSetting;
                grabbedRigidbody.transform.gameObject.GetComponent<CubeManager>().isNotGrabbedNow();
                grabbedRigidbody = null;
            }
            return;
        }

        // We are not holding an object
        if (grabbedRigidbody == null)
        {
            Ray ray = Camera.main.ViewportPointToRay(Vector3.one * 0.5f);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, maxGrabDistance))
            {
                if (hit.rigidbody != null && !hit.rigidbody.isKinematic)
                {
                    grabbedRigidbody = hit.rigidbody;
                    initialInterpolationSetting = grabbedRigidbody.interpolation;
                    rotationDifferenceEuler = hit.transform.rotation.eulerAngles - transform.rotation.eulerAngles;
                    hitOffsetLocal = hit.transform.InverseTransformVector(hit.point - hit.transform.position);
                    currentGrabDistance = Vector3.Distance(ray.origin, hit.point);
                    grabbedRigidbody.interpolation = RigidbodyInterpolation.Interpolate;
                    grabbedRigidbody.gameObject.GetComponent<CubeManager>().isGrabbedNow();
                }  
            }
        }
        else
        {
            //Rotate object
            if (Input.GetKey(KeyCode.R))
            {
                rotationInput += new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
            }
        }
    }

    private void FixedUpdate()
    {
        // We are holding an object
        if (grabbedRigidbody)
        {
            //We place the object in the space in relation with the camera position 
            Ray ray = Camera.main.ViewportPointToRay(Vector3.one * 0.5f);
            grabbedRigidbody.MoveRotation(Quaternion.Euler(rotationDifferenceEuler + transform.rotation.eulerAngles));
            Vector3 holdPoint = ray.GetPoint(currentGrabDistance);
            Vector3 currentEuler = grabbedRigidbody.rotation.eulerAngles;
            grabbedRigidbody.transform.RotateAround(holdPoint, transform.right, rotationInput.y);
            grabbedRigidbody.transform.RotateAround(holdPoint, transform.up, -rotationInput.x);
            grabbedRigidbody.angularVelocity = Vector3.zero;
            rotationInput = Vector2.zero;
            rotationDifferenceEuler = grabbedRigidbody.transform.rotation.eulerAngles - transform.rotation.eulerAngles;
            Vector3 centerDestination = holdPoint - grabbedRigidbody.transform.TransformVector(hitOffsetLocal);
            Vector3 toDestination = centerDestination - grabbedRigidbody.transform.position;
            Vector3 force = toDestination / Time.fixedDeltaTime;
            grabbedRigidbody.velocity = Vector3.zero;
            grabbedRigidbody.AddForce(force, ForceMode.VelocityChange);

            //Now we draw the web
            lineRenderer.SetPosition(0, new Vector3(player.transform.position.x, player.transform.position.y + 0.1f, player.transform.position.z)); 
            lineRenderer.SetPosition(1, new Vector3(grabbedRigidbody.gameObject.transform.position.x, grabbedRigidbody.gameObject.transform.position.y, grabbedRigidbody.gameObject.transform.position.z)); 
        }
        else
        {
            //Erase the web
            lineRenderer.SetPosition(0, new Vector3(0, 0, 0));
            lineRenderer.SetPosition(1, new Vector3(0, 0, 0));
        }
    } 
}
