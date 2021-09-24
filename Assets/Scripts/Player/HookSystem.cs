using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookSystem : MonoBehaviour
{
    private new Rigidbody rigidbody;
    private Vector3 hitOffsetLocal;
    private float currentGrabDistance;
    private RigidbodyInterpolation initialInterpolationSetting;
    private Vector3 rotationDifferenceEuler;
    private Vector2 rotationInput;
    public  float maxGrabDistance = 30.0f;

    void Update()
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
            Ray ray = CenterRay();
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
            Ray ray = CenterRay();
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
        }
    }

    private Ray CenterRay()
    {
        return Camera.main.ViewportPointToRay(Vector3.one * 0.5f);
    }
}
