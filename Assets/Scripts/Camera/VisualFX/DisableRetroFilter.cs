using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableRetroFilter : MonoBehaviour
{
    private ScanlinesEffect[] scanlines;
    private Assets.Scripts.Cam.Effects.RetroSize[] retroFilters;
    private Camera[] cameras;

    private void Start()
    {
        scanlines = Resources.FindObjectsOfTypeAll<ScanlinesEffect>();
        retroFilters = Resources.FindObjectsOfTypeAll<Assets.Scripts.Cam.Effects.RetroSize>();
    }

    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            foreach (ScanlinesEffect scanline in scanlines)
            {
                scanline.enabled = false;
            }

            foreach (Assets.Scripts.Cam.Effects.RetroSize retroFilter in retroFilters)
            {
                retroFilter.enabled = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            foreach (ScanlinesEffect scanline in scanlines)
            {
                scanline.enabled = true;
            }

            foreach (Assets.Scripts.Cam.Effects.RetroSize retroFilter in retroFilters)
            {
                retroFilter.enabled = true;
            }
        }
    }
}
