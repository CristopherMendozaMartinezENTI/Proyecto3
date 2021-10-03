using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraManager : MonoBehaviour
{
    [Header("Options")]
    //Variables of the Camera Movement
    public float sensitivityX = 5.0f;
    public float sensitivityY = 5.0f;
    public float minimumX = -30.0f;
    public float maximumX = 30.0f;
    public float minimumY = 0.0f;
    public float maximumY = 620.0f;
    public float frameCounter = 5.0f;

    private float rotationX = 0.0f;
    private float rotationY = 0.0f;
    private float rotAverageX = 0.0f;
    private float rotAverageY = 0.0f;
    private List<float> rotArrayX = new List<float>();
    private List<float> rotArrayY = new List<float>();
    private Quaternion originalRotation;

    //Variables of the Camera Zooming
    public float zoomMultiplier = 2.0f;
    public float defaultFov = 80.0f;
    public float zoomDuration = 0.5f;
    
    private Camera cam;

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        cam = this.gameObject.GetComponent<Camera>();
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb)
            rb.freezeRotation = true;
        originalRotation = transform.localRotation;
    }

    private void Update()
    {
        if (Input.GetMouseButton(1))
        {
            zoomCamera(defaultFov / zoomMultiplier);
        }
        else if (cam.fieldOfView != defaultFov)
        {
            zoomCamera(defaultFov);
        }

        if (!Input.GetKey(KeyCode.R))
        {
            rotAverageY = 0f;
            rotAverageX = 0f;
            rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
            rotationX += Input.GetAxis("Mouse X") * sensitivityX;
            rotArrayY.Add(rotationY);
            rotArrayX.Add(rotationX);

            if (rotArrayY.Count >= frameCounter)
            {
                rotArrayY.RemoveAt(0);
            }
            if (rotArrayX.Count >= frameCounter)
            {
                rotArrayX.RemoveAt(0);
            }

            for (int j = 0; j < rotArrayY.Count; j++)
            {
                rotAverageY += rotArrayY[j];
            }
            for (int i = 0; i < rotArrayX.Count; i++)
            {
                rotAverageX += rotArrayX[i];
            }

            rotAverageY /= rotArrayY.Count;
            rotAverageX /= rotArrayX.Count;
            rotAverageY = clampAngle(rotAverageY, minimumY, maximumY);
            rotAverageX = clampAngle(rotAverageX, minimumX, maximumX);
            Quaternion yQuaternion = Quaternion.AngleAxis(rotAverageY, Vector3.left);
            Quaternion xQuaternion = Quaternion.AngleAxis(rotAverageX, Vector3.up);
            transform.localRotation = originalRotation * xQuaternion * yQuaternion;
        }
    }

    public float clampAngle(float angle, float min, float max)
    {
        angle = angle % 360;
        if ((angle >= -360.0f) && (angle <= 360.0f))
        {
            if (angle < -360.0)
            {
                angle += 360.0f;
            }
            if (angle > 360.0f)
            {
                angle -= 360.0f;
            }
        }
        return Mathf.Clamp(angle, min, max);
    }

    private void zoomCamera(float target)
    {
        float angle = Mathf.Abs((defaultFov / zoomMultiplier) - defaultFov);
        cam.fieldOfView = Mathf.MoveTowards(cam.fieldOfView, target, angle / zoomDuration * Time.deltaTime);
    }
}