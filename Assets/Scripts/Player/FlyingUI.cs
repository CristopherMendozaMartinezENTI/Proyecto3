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

    private void Awake()
    {
        if (mouseFlight == null)
            Debug.LogError(name + ": Hud - Mouse Flight Controller not assigned!");

        playerCam = mouseFlight.GetComponentInChildren<Camera>();

        if (playerCam == null)
            Debug.LogError(name + ": Hud - No camera found on assigned Mouse Flight Controller!");

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false; 
    }

    private void Update()
    {
        if (mouseFlight == null || playerCam == null)
            return;

        UpdateGraphics(mouseFlight);
    }

    private void UpdateGraphics(FlyController controller)
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
