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
    private float camSmoothSpeed = 5.0f;
    [SerializeField] 
    private float mouseSensitivity = 3.0f;
    private float aimDistance = 500f;
    private Vector3 frozenDirection = Vector3.forward;
    private bool isMouseAimFrozen = false;

    private void Start()
    {
        transform.parent = null;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        transform.position = target.position;
        if (Input.GetKeyDown(KeyCode.C))
        {
            isMouseAimFrozen = true;
            frozenDirection = mouseAim.forward;
        }
        else if (Input.GetKeyUp(KeyCode.C))
        {
            isMouseAimFrozen = false;
            mouseAim.forward = frozenDirection;
        }

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = -Input.GetAxis("Mouse Y") * mouseSensitivity;
        mouseAim.Rotate(cam.right, mouseY, Space.World);
        mouseAim.Rotate(cam.up, mouseX, Space.World);
        Vector3 upVec = (Mathf.Abs(mouseAim.forward.y) > 0.9f) ? cameraRig.up : Vector3.up;
        cameraRig.rotation = damp(cameraRig.rotation, Quaternion.LookRotation(mouseAim.forward, upVec), camSmoothSpeed, Time.deltaTime);
    }
    private Quaternion damp(Quaternion a, Quaternion b, float lambda, float dt)
    {
        return Quaternion.Slerp(a, b, 1 - Mathf.Exp(-lambda * dt));
    }

    private Vector3 getFrozenMouseAimPos()
    {
        if (mouseAim != null)
            return mouseAim.position + (frozenDirection * aimDistance);
        else
            return transform.forward * aimDistance;
    }

    public Vector3 MouseAimPos
    {
        get
        {
            if (mouseAim != null)
            {
                return isMouseAimFrozen
                    ? getFrozenMouseAimPos()
                    : mouseAim.position + (mouseAim.forward * aimDistance);
            }
            else
            {
                return transform.forward * aimDistance;
            }
        }
    }
}
