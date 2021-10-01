using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private FlyController mouseFlight = null;

    [Header("HUD Elements")]
    [SerializeField] private RectTransform mousePos = null;

    private Camera playerCam = null;

    private void Start()
    {
        playerCam = mouseFlight.GetComponentInChildren<Camera>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false; 
    }

    private void Update()
    {
        if (mouseFlight == null || playerCam == null)
            return;

        UpdateUI(mouseFlight);
    }

    private void UpdateUI(FlyController controller)
    {
        if (mousePos != null)
        {
            mousePos.position = playerCam.WorldToScreenPoint(controller.MouseAimPos);
            mousePos.gameObject.SetActive(mousePos.position.z > 1f);
        }
    }

    public void SetReferenceMouseFlight(FlyController controller)
    {
        mouseFlight = controller;
    }
}
