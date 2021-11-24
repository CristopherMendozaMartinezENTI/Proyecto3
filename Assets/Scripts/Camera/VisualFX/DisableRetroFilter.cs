using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableRetroFilter : MonoBehaviour
{
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F5))
        {
            this.gameObject.GetComponent<ScanlinesEffect>().enabled = false;
            this.gameObject.GetComponent<Assets.Scripts.Cam.Effects.RetroSize>().enabled = false;
        }

        if(Input.GetKeyDown(KeyCode.F6))
        {
            this.gameObject.GetComponent<ScanlinesEffect>().enabled = true;
            this.gameObject.GetComponent<Assets.Scripts.Cam.Effects.RetroSize>().enabled = true;
        }
    }
}
