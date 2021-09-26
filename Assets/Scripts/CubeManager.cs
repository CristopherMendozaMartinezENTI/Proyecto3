using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeManager : MonoBehaviour
{
    public string defaultMaterial;
    public string onHookMaterial;
    public bool worksBackwards;
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
        if (worksBackwards)
        {
            this.gameObject.GetComponent<Rigidbody>().useGravity = false;
            this.gameObject.GetComponent<Rigidbody>().AddForce(-Physics.gravity, ForceMode.Acceleration);
        }
        else
        {
            this.gameObject.GetComponent<Rigidbody>().useGravity = true;
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
