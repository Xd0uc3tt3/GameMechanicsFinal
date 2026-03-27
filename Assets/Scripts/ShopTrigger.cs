using UnityEngine;
using TMPro;

public class ShopTrigger : MonoBehaviour
{
    public GameObject shopUI;
    public GameObject interactPromptUI;
    private bool playerInRange = false;

    private PlayerInput playerInput;

    public bool PlayerInRange => playerInRange;

    public Camera shopCamera;

    public CameraScript cameraScript;

    private void Start()
    {
        playerInput = FindObjectOfType<PlayerInput>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            interactPromptUI.SetActive(true);
            playerInput.SetShopTrigger(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            shopUI.SetActive(false);
            interactPromptUI.SetActive(false);
            cameraScript.SetCameraLocked(true);
            shopCamera.gameObject.SetActive(false);
            playerInput.ClearShopTrigger();
        }
    }

    public void ToggleShop()
    {
        if (playerInRange)
        {
            bool isOpen = !shopUI.activeSelf;
            shopCamera.gameObject.SetActive(isOpen);
            shopUI.SetActive(!shopUI.activeSelf);
            cameraScript.SetCameraLocked(!isOpen);
        }
    }


}
