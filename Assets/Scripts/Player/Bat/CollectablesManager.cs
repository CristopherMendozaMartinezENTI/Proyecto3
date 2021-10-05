using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectablesManager : MonoBehaviour
{
    public GameObject hudElement;

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            hudElement.SetActive(true);
            this.gameObject.SetActive(false);
        }
    }
}
