using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateIcon : MonoBehaviour
{
    public bool rotate;
    public bool resize;
    public float rotationSpeed;
    private float realRotation;
    public float minScale;
    public float maxScale;
    private Vector3 scaleChange;

    private void Start()
    {
        realRotation = 0;
        scaleChange = new Vector3(-0.01f, -0.01f, -0.01f);
    }

    private void Update()
    {
        if (rotate)
        {
            realRotation++;

            if (realRotation == 360)
                realRotation = 0;

            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, -realRotation, 0), rotationSpeed);
        }

        if(resize)
        {
            transform.localScale += scaleChange;
            if (transform.localScale.y < minScale || transform.localScale.y > maxScale)
            {
                scaleChange = -scaleChange;
            }
        }
    }
}
