using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FlyManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private FlyController controller = null;

    [Header("Physics")]
    [Tooltip("Force to push plane forwards with")] public float thrust = 100f;
    [Tooltip("Pitch, Yaw, Roll")] public Vector3 turnTorque = new Vector3(90f, 25f, 45f);
    [Tooltip("Multiplier for all forces")] public float forceMult = 1000f;

    [Header("Autopilot")]
    [Tooltip("Sensitivity for autopilot flight.")] public float sensitivity = 5f;
    [Tooltip("Angle at which airplane banks fully into target.")] public float aggressiveTurnAngle = 10f;

    [Header("Input")]
    [SerializeField] [Range(-1f, 1f)] private float pitch = 0f;
    [SerializeField] [Range(-1f, 1f)] private float yaw = 0f;
    [SerializeField] [Range(-1f, 1f)] private float roll = 0f;

    public float Pitch { set { pitch = Mathf.Clamp(value, -1f, 1f); } get { return pitch; } }
    public float Yaw { set { yaw = Mathf.Clamp(value, -1f, 1f); } get { return yaw; } }
    public float Roll { set { roll = Mathf.Clamp(value, -1f, 1f); } get { return roll; } }

    private Rigidbody rigid;

    private bool rollOverride = false;
    private bool pitchOverride = false;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();

        if (controller == null)
            Debug.LogError(name + ": Plane - Missing reference to MouseFlightController!");
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

        // Calculate the autopilot stick inputs.
        float autoYaw = 0f;
        float autoPitch = 0f;
        float autoRoll = 0f;
        if (controller != null)
            RunAutopilot(controller.MouseAimPos, out autoYaw, out autoPitch, out autoRoll);

        // Use either keyboard or autopilot input.
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

    private void FixedUpdate()
    {
        if(Input.GetKey(KeyCode.LeftShift))
        {
            thrust = 10;
        }
        else
        {
            thrust = 0;
        }

        rigid.AddRelativeForce(Vector3.forward * thrust * forceMult, ForceMode.Force);
        rigid.AddRelativeTorque(new Vector3(turnTorque.x * pitch, turnTorque.y * yaw,-turnTorque.z * roll) * forceMult, ForceMode.Force);
    }
}

