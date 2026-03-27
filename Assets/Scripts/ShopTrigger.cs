using UnityEngine;
using TMPro;

public class ShopTrigger : MonoBehaviour
{
    public GameObject shopUI;
    public GameObject interactPromptUI;
    private bool playerInRange = false;

    private PlayerInput playerInput;

    public bool PlayerInRange => playerInRange;

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
            playerInput.ClearShopTrigger();
        }
    }

    public void ToggleShop()
    {
        if (playerInRange)
        {
            shopUI.SetActive(!shopUI.activeSelf);
        }
    }


}
