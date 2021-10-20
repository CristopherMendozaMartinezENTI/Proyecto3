using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleportSystem : MonoBehaviour
{
    [SerializeField] private string sceneName;
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if(Input.GetKeyDown(KeyCode.Return))
            {
                this.gameObject.GetComponent<AudioSource>().Play();
                StartCoroutine(GameObject.FindObjectOfType<SceneFader>().FadeAndLoadScene(SceneFader.FadeDirection.In, sceneName));
            }
        }
    }
}
