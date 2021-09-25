using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeManager : MonoBehaviour
{
    public string defaultMaterial;
    public string onHookMaterial;
    private MeshRenderer mesh;
    private Outline objectOutline;
    private Material defaultM;
    private Material onHookM;
    private bool isGrabbed;

    // Start is called before the first frame update
    private void Start()
    {
        mesh = this.gameObject.GetComponent<MeshRenderer>();
        objectOutline = this.gameObject.GetComponent<Outline>();
        defaultM = Resources.Load<Material>(@"Materials/" + defaultMaterial);
        onHookM = Resources.Load<Material>(@"Materials/" + onHookMaterial);
        mesh.material = defaultM;
        isGrabbed = false;
    }

    // Update is called once per frame
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
    {
        isGrabbed = true;
    }

    public void isNotGrabbedNow()
    {
        isGrabbed = false;
    }
}
