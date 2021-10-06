using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchToSpider : MonoBehaviour
{
    [SerializeField] private GameObject spider;
    [SerializeField] private GameObject spiderCamera;
    [SerializeField] private GameObject batPlayable;
    [SerializeField] private GameObject batPlayableMesh;
    [SerializeField] private GameObject batTargetPoint;
    [SerializeField] private GameObject batFlyRig;
    [SerializeField] private GameObject batFlyUI;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            spider.gameObject.GetComponent<PlayerController>().enabled = true;
            batPlayable.GetComponent<FlyController>().enabled = false;
            batPlayable.GetComponent<SwitchToSpider>().enabled = false;
            batFlyRig.GetComponent<FlyCameraController>().enabled = false;
            spiderCamera.SetActive(true);
            batFlyRig.SetActive(false);
            batFlyUI.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            batPlayable.transform.position = batTargetPoint.transform.position;
            batPlayable.transform.rotation = batTargetPoint.transform.rotation;
            spider.gameObject.GetComponent<PlayerController>().enabled = true;
            batPlayable.GetComponent<FlyController>().enabled = false;
            batPlayable.GetComponent<SwitchToSpider>().enabled = false;
            batFlyRig.GetComponent<FlyCameraController>().enabled = false;
            batPlayableMesh.SetActive(false);
            spiderCamera.SetActive(true);
            batFlyRig.SetActive(false);
            batFlyUI.SetActive(false);
            batTargetPoint.SetActive(true);
        }
    }
}
