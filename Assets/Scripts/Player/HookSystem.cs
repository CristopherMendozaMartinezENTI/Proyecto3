using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookSystem : MonoBehaviour
{
    public GameObject player;
    public float maxGrabDistance = 5.0f;

    private float currentGrabDistance;
    private Vector2 rotationInput;
    private Vector3 hitOffsetLocal;
    private Vector3 rotationDifferenceEuler;
    private new Rigidbody rigidbody;
    private RigidbodyInterpolation initialInterpolationSetting;
    private LineRenderer lineRenderer;

    private void Start()
    {
        lineRenderer = this.gameObject.GetComponent<LineRenderer>();
        lineRenderer.startWidth = 0.01f;
        lineRenderer.endWidth = 0.01f;
        lineRenderer.positionCount = 2;
        lineRenderer.useWorldSpace = true;
    }

    private void Update()
    {
        if (!Input.GetMouseButton(0))
        {
            if (rigidbody != null)
            {
                // Reset the rigidbody to how it was before we grabbed it
                rigidbody.interpolation = initialInterpolationSetting;

                rigidbody = null;
            }
            return;
        }

        // We are not holding an object
        if (rigidbody == null)
        {
            Ray ray = Camera.main.ViewportPointToRay(Vector3.one * 0.5f);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, maxGrabDistance))
            {
                if (hit.rigidbody != null && !hit.rigidbody.isKinematic)
                {
                    rigidbody = hit.rigidbody;
                    initialInterpolationSetting = rigidbody.interpolation;
                    rotationDifferenceEuler = hit.transform.rotation.eulerAngles - transform.rotation.eulerAngles;
                    hitOffsetLocal = hit.transform.InverseTransformVector(hit.point - hit.transform.position);
                    currentGrabDistance = Vector3.Distance(ray.origin, hit.point);
                    rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
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
        if (rigidbody)
        {
            //We place the object in the space in relation with the camera position 
            Ray ray = Camera.main.ViewportPointToRay(Vector3.one * 0.5f);
            rigidbody.MoveRotation(Quaternion.Euler(rotationDifferenceEuler + transform.rotation.eulerAngles));
            Vector3 holdPoint = ray.GetPoint(currentGrabDistance);
            Vector3 currentEuler = rigidbody.rotation.eulerAngles;
            rigidbody.transform.RotateAround(holdPoint, transform.right, rotationInput.y);
            rigidbody.transform.RotateAround(holdPoint, transform.up, -rotationInput.x);
            rigidbody.angularVelocity = Vector3.zero;
            rotationInput = Vector2.zero;
            rotationDifferenceEuler = rigidbody.transform.rotation.eulerAngles - transform.rotation.eulerAngles;
            Vector3 centerDestination = holdPoint - rigidbody.transform.TransformVector(hitOffsetLocal);
            Vector3 toDestination = centerDestination - rigidbody.transform.position;
            Vector3 force = toDestination / Time.fixedDeltaTime;
            rigidbody.velocity = Vector3.zero;
            rigidbody.AddForce(force, ForceMode.VelocityChange);

            //Now we draw the web
            lineRenderer.SetPosition(0, new Vector3(player.transform.position.x, player.transform.position.y + 0.1f, player.transform.position.z)); //x,y and z position of the starting point of the line
            lineRenderer.SetPosition(1, new Vector3(rigidbody.gameObject.transform.position.x, rigidbody.gameObject.transform.position.y, rigidbody.gameObject.transform.position.z)); //x,y and z position of the end point of the line
        }
    }
}
