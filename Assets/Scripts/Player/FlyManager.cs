using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FlyManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] 
    private FlyController controller = null;

    private float fowardForce = 10f;
    private Vector3 turnTorque = new Vector3(1.0f, 1.0f, 1.0f);
    private float forceMult = 20f;

    public float sensitivity = 5f;
    public float aggressiveTurnAngle = 10f;

    private float pitch = 0f;
    private float yaw = 0f;
    private float roll = 0f;

    private Rigidbody rigid;
    private bool rollOverride = false;
    private bool pitchOverride = false;

    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            fowardForce = 20;
        }
        else
        {
            fowardForce = 0;
        }

        rigid.AddRelativeForce(Vector3.forward * fowardForce * forceMult, ForceMode.Force);
        rigid.AddRelativeTorque(new Vector3(turnTorque.x * pitch, turnTorque.y * yaw, -turnTorque.z * roll) * forceMult, ForceMode.Force);
    }

    private void Update()
    {
        rollOverride = false;
        pitchOverride = false;

        float keyboardRoll = Input.GetAxis("Horizontal");
        if (Mathf.Abs(keyboardRoll) > .25f)
        {
            rollOverride = true;
        }

        float keyboardPitch = Input.GetAxis("Vertical");
        if (Mathf.Abs(keyboardPitch) > .25f)
        {
            pitchOverride = true;
            rollOverride = true;
        }

        float autoYaw = 0f;
        float autoPitch = 0f;
        float autoRoll = 0f;
        if (controller != null)
            RunAutopilot(controller.MouseAimPos, out autoYaw, out autoPitch, out autoRoll);
        yaw = autoYaw;
        pitch = (pitchOverride) ? keyboardPitch : autoPitch;
        roll = (rollOverride) ? keyboardRoll : autoRoll;
    }

    private void RunAutopilot(Vector3 flyTarget, out float yaw, out float pitch, out float roll)
    {
        Vector3 localFlyTarget = transform.InverseTransformPoint(flyTarget).normalized * sensitivity;
        float angleOffTarget = Vector3.Angle(transform.forward, flyTarget - transform.position);
        yaw = Mathf.Clamp(localFlyTarget.x, -1f, 1f);
        pitch = -Mathf.Clamp(localFlyTarget.y, -1f, 1f);
        float agressiveRoll = Mathf.Clamp(localFlyTarget.x, -1f, 1f);
        float wingsLevelRoll = transform.right.y;
        float wingsLevelInfluence = Mathf.InverseLerp(0f, aggressiveTurnAngle, angleOffTarget);
        roll = Mathf.Lerp(wingsLevelRoll, agressiveRoll, wingsLevelInfluence);
    }
}

