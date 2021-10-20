using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchToSpider : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private GameObject spider;
    [SerializeField] private GameObject spiderCamera;
    [SerializeField] private GameObject batPlayable;
    [SerializeField] private GameObject batPlayableMesh;
    [SerializeField] private GameObject batTargetPoint;
    [SerializeField] private GameObject batFlyRig;
    [SerializeField] private GameObject batFlyUI;
    private bool atachToPlayer = true;

    private void Update()
    {
        if(atachToPlayer)
        {
            batPlayable.transform.position = batTargetPoint.transform.position;
            batPlayable.transform.rotation = batTargetPoint.transform.rotation;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            this.gameObject.GetComponent<AudioSource>().Play();
            atachToPlayer = false;
            spider.gameObject.GetComponent<PlayerController>().enabled = true;
            batPlayable.GetComponent<FlyController>().enabled = false;
            spider.GetComponent<SwitchToBat>().enabled = true;
            batFlyRig.GetComponent<FlyCameraController>().enabled = false;
            spiderCamera.SetActive(true);
            batFlyRig.SetActive(false);
            batFlyUI.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            this.gameObject.GetComponent<AudioSource>().Play();
            atachToPlayer = true;
            spider.gameObject.GetComponent<PlayerController>().enabled = true;
            batPlayable.GetComponent<FlyController>().enabled = false;
            spider.GetComponent<SwitchToBat>().enabled = true;
            batFlyRig.GetComponent<FlyCameraController>().enabled = false;
            batPlayableMesh.SetActive(false);
            spiderCamera.SetActive(true);
            batFlyRig.SetActive(false);
            batFlyUI.SetActive(false);
            batTargetPoint.SetActive(true);
        }
    }
}
