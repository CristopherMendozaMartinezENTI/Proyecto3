using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FlyManager : MonoBehaviour
{
    [Header("Components")]
    public FlyController controller = null;
    public float fowardForce = 0.0f;
    public float forceMult = 0.0f;
    public float turnAngle = 90.0f;
    public float sensitivity = 1.0f;
    private float currentFowardForce = 0.0f;
    private float pitch = 0.0f;
    private float yaw = 0.0f;
    private float roll = 0.0f;
    private bool rollOverride = false;
    private bool pitchOverride = false;
    public Vector3 turnForce = new Vector3(1.0f, 1.0f, 1.0f);
    private Rigidbody rigid;

    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
        currentFowardForce = fowardForce;
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            fowardForce = currentFowardForce;
        }
        else
        {
            fowardForce = 0.0f;
        }

        rigid.AddRelativeForce(Vector3.forward * fowardForce * forceMult, ForceMode.Force);
        rigid.AddRelativeTorque(new Vector3(turnForce.x * pitch, turnForce.y * yaw, -turnForce.z * roll) * forceMult, ForceMode.Force);
    }

    private void Update()
    {
        rollOverride = false;
        pitchOverride = false;

        float keyboardRoll = Input.GetAxis("Horizontal");
        if (Mathf.Abs(keyboardRoll) > 0.25f)
        {
            rollOverride = true;
        }

        float keyboardPitch = Input.GetAxis("Vertical");
        if (Mathf.Abs(keyboardPitch) > 0.25f)
        {
            pitchOverride = true;
            rollOverride = true;
        }

        float autoYaw = 0.0f;
        float autoPitch = 0.0f;
        float autoRoll = 0.0f;
        RunAutopilot(controller.MouseAimPos, out autoYaw, out autoPitch, out autoRoll);
        yaw = autoYaw;
        pitch = (pitchOverride) ? keyboardPitch : autoPitch;
        roll = (rollOverride) ? keyboardRoll : autoRoll;
    }

    private void RunAutopilot(Vector3 flyTarget, out float yaw, out float pitch, out float roll)
    {
        Vector3 localFlyTarget = transform.InverseTransformPoint(flyTarget).normalized * sensitivity;
        float angleOffTarget = Vector3.Angle(transform.forward, flyTarget - transform.position);
        yaw = Mathf.Clamp(localFlyTarget.x, -1.0f, 1.0f);
        pitch = -Mathf.Clamp(localFlyTarget.y, -1.0f, 1.0f);
        float agressiveRoll = Mathf.Clamp(localFlyTarget.x, -1.0f, 1.0f);
        float wingsLevelRoll = transform.right.y;
        float wingsLevelInfluence = Mathf.InverseLerp(0f, turnAngle, angleOffTarget);
        roll = Mathf.Lerp(wingsLevelRoll, agressiveRoll, wingsLevelInfluence);
    }
}

