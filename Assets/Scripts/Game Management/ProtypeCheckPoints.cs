using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ProtypeCheckPoints : MonoBehaviour
{
    [SerializeField] private string checkPointScene1;
    [SerializeField] private string checkPointScene2;
    [SerializeField] private string checkPointScene3;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
            SceneManager.LoadScene(checkPointScene1);

        if (Input.GetKeyDown(KeyCode.F2))
            SceneManager.LoadScene(checkPointScene2);

        if (Input.GetKeyDown(KeyCode.F3))
            SceneManager.LoadScene(checkPointScene3);
    }
}
