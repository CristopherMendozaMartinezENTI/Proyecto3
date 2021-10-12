using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchToBat : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private GameObject spider;
    [SerializeField] private GameObject spiderCamera;
    [SerializeField] private GameObject batPlayable;
    [SerializeField] private GameObject batPlayableMesh;
    [SerializeField] private GameObject batTargetPoint;
    [SerializeField] private GameObject batFlyRig;
    [SerializeField] private GameObject batFlyUI;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && !spider.GetComponent<PlayerController>().getOnHook())
        {
            spider.gameObject.GetComponent<PlayerController>().enabled = false;
            batPlayable.GetComponent<FlyController>().enabled = true;
            batPlayable.GetComponent<SwitchToSpider>().enabled = true;
            batFlyRig.GetComponent<FlyCameraController>().enabled = true;
            spiderCamera.SetActive(false);
            batPlayableMesh.SetActive(true);
            batFlyRig.SetActive(true);
            batFlyUI.SetActive(true);
            batPlayable.SetActive(true);
            batTargetPoint.SetActive(false);
            spider.GetComponent<SwitchToBat>().enabled = false;
        }
    }
}
