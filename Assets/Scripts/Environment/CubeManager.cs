using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GravitationalForce { Up, Down, Left, Right }

public class CubeManager : MonoBehaviour
{
    [SerializeField] private string defaultMaterial;
    [SerializeField] private string onHookMaterial;
    [SerializeField] private GravitationalForce gravityOrientation; 
    private MeshRenderer mesh;
    private Outline objectOutline;
    private Material defaultM;
    private Material onHookM;
    private bool isGrabbed;
    private bool outlineIsEnable;

    private void Start()
    {
        mesh = this.gameObject.GetComponent<MeshRenderer>();
        objectOutline = this.gameObject.GetComponent<Outline>();
        defaultM = Resources.Load<Material>(@"Materials/" + defaultMaterial);
        onHookM = Resources.Load<Material>(@"Materials/" + onHookMaterial);
        mesh.material = defaultM;
        isGrabbed = false;
    }

    private void FixedUpdate()
    {
        switch(gravityOrientation)
        {
            case GravitationalForce.Up:
                gameObject.GetComponent<Rigidbody>().useGravity = false;
                gameObject.GetComponent<Rigidbody>().AddForce(-Physics.gravity, ForceMode.Acceleration);
                return;
            case GravitationalForce.Down:
                gameObject.GetComponent<Rigidbody>().useGravity = true;
                return;
            case GravitationalForce.Left:
                gameObject.GetComponent<Rigidbody>().useGravity = false;
                gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(9.81f, 0.0f, 0.0f), ForceMode.Acceleration);
                return;
            case GravitationalForce.Right:
                gameObject.GetComponent<Rigidbody>().useGravity = false;
                gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(-9.81f, 0.0f, 0.0f), ForceMode.Acceleration);
                return;
        }
    }

    private void Update()
    {
        if(isGrabbed)
        {
            mesh.material = onHookM;
        }
        else
        {
            mesh.material = defaultM;
        }
    }

    public void isGrabbedNow()
    { isGrabbed = true; }

    public void isNotGrabbedNow()
    { isGrabbed = false; }

    public void enableOutline() { objectOutline.enabled = true;}

    public void disableOutline() { objectOutline.enabled = false; }

    public bool outlineState() { return objectOutline.enabled; }
}
