using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicTextures : MonoBehaviour
{
    [Header("Options")]
    [SerializeField] private bool rotate;
    [SerializeField] private bool resize;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float minScale;
    [SerializeField] private float maxScale;
     private float realRotation;
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
