using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnArea : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.GetComponent<CubeManager>())
        {
            other.GetComponent<CubeManager>().resetPos();
        }
    }
}
