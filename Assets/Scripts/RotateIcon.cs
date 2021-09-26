using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateIcon : MonoBehaviour
{

    public float speed;
    float realRotation = 0;

    private void Update()
    {
        realRotation++;

        if (realRotation == 360)
            realRotation = 0;

        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, -realRotation, 0), speed);
    }
}
