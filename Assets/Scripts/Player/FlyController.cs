using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    private Transform target = null;
    [SerializeField]
    private Transform mouseAim = null;
    [SerializeField] 
    private Transform cameraRig = null;
    [SerializeField]
    private Transform cam = null;

    [Header("Options")]
    [SerializeField] 
    private bool useFixed = true;
    [SerializeField]
    private float camSmoothSpeed = 5f;
    [SerializeField] 
    private float mouseSensitivity = 3f;

    private float aimDistance = 500f;
    private Vector3 frozenDirection = Vector3.forward;
    private bool isMouseAimFrozen = false;

    public Vector3 MouseAimPos
    {
        get
        {
            if (mouseAim != null)
            {
                return isMouseAimFrozen
                    ? GetFrozenMouseAimPos()
                    : mouseAim.position + (mouseAim.forward * aimDistance);
            }
            else
            {
                return transform.forward * aimDistance;
            }
        }
    }

    private void Awake()
    {
        transform.parent = null;
    }

    private void Update()
    {
        if (useFixed == false)
            UpdateCameraPos();

        RotateRig();
    }

    private void FixedUpdate()
    {
        if (useFixed == true)
            UpdateCameraPos();
    }

    private void RotateRig()
    {
        if (mouseAim == null || cam == null || cameraRig == null)
            return;

        if (Input.GetKeyDown(KeyCode.C))
        {
            isMouseAimFrozen = true;
            frozenDirection = mouseAim.forward;
        }
        else if  (Input.GetKeyUp(KeyCode.C))
        {
            isMouseAimFrozen = false;
            mouseAim.forward = frozenDirection;
        }

        // Mouse input.
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = -Input.GetAxis("Mouse Y") * mouseSensitivity;

        mouseAim.Rotate(cam.right, mouseY, Space.World);
        mouseAim.Rotate(cam.up, mouseX, Space.World);

        Vector3 upVec = (Mathf.Abs(mouseAim.forward.y) > 0.9f) ? cameraRig.up : Vector3.up;

        cameraRig.rotation = Damp(cameraRig.rotation, Quaternion.LookRotation(mouseAim.forward, upVec),camSmoothSpeed, Time.deltaTime);
    }

    private Vector3 GetFrozenMouseAimPos()
    {
        if (mouseAim != null)
            return mouseAim.position + (frozenDirection * aimDistance);
        else
            return transform.forward * aimDistance;
    }

    private void UpdateCameraPos()
    {
        if (target != null)
        {
            transform.position = target.position;
        }
    }

    private Quaternion Damp(Quaternion a, Quaternion b, float lambda, float dt)
    {
        return Quaternion.Slerp(a, b, 1 - Mathf.Exp(-lambda * dt));
    }
}
