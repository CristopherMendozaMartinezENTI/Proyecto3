using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private FlyCameraController mouseFlight;
    [Header("HUD Elements")]
    [SerializeField] private RectTransform mousePos;

    private Camera playerCam = null;

    private void Start()
    {
        playerCam = mouseFlight.GetComponentInChildren<Camera>();
    }

    private void Update()
    {
        UpdateUI(mouseFlight);
    }

    private void UpdateUI(FlyCameraController controller)
    {
        mousePos.position = playerCam.WorldToScreenPoint(controller.MouseAimPos);
        mousePos.gameObject.SetActive(mousePos.position.z > 1.0f);
    }
}
