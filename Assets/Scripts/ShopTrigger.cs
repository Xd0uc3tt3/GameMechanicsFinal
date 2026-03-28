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

    public GameObject animatedShopObject;
    public Transform startPoint;
    public Transform endPoint;
    public float animationSpeed = 5f;

    private bool shopIsOpen = false;
    private int currentWaypointIndex = 0;

    public AudioClip shopOpenSound;
    public AudioClip shopCloseSound;
    private AudioSource audioSource;

    public AudioClip shopMusicClip;
    private AudioSource musicAudioSource;

    private Renderer[] playerRenderers;

    private void Start()
    {
        playerInput = FindObjectOfType<PlayerInput>();

        animatedShopObject.transform.position = startPoint.position;
        animatedShopObject.SetActive(false);

        audioSource = gameObject.AddComponent<AudioSource>();

        musicAudioSource = gameObject.AddComponent<AudioSource>();
        musicAudioSource.loop = true;
        musicAudioSource.playOnAwake = false;
    }

    private void Update()
    {
        if (animatedShopObject == null || !animatedShopObject.activeSelf) return;

        Vector3 target = shopIsOpen ? endPoint.position : startPoint.position;

        animatedShopObject.transform.position = Vector3.Lerp(animatedShopObject.transform.position, target, Time.deltaTime * animationSpeed);

        if (!shopIsOpen && Vector3.Distance(animatedShopObject.transform.position, startPoint.position) < 0.01f)
        {
            animatedShopObject.transform.position = startPoint.position;
            animatedShopObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            interactPromptUI.SetActive(true);
            playerInput.SetShopTrigger(this);

            playerRenderers = other.GetComponentsInChildren<Renderer>();
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
            SetShopObject(false);
            SetPlayerVisibility(true);
            playerInput.enabled = true;
        }
    }

    public void ToggleShop()
    {
        if (playerInRange)
        {
            bool isOpen = !shopUI.activeSelf;
            shopCamera.gameObject.SetActive(isOpen);
            shopUI.SetActive(isOpen);
            interactPromptUI.SetActive(!isOpen);
            cameraScript.SetCameraLocked(!isOpen);

            PlaySound(isOpen ? shopOpenSound : shopCloseSound);
            SetShopObject(isOpen);
            SetPlayerVisibility(!isOpen);

            playerInput.enabled = !isOpen;

            if (isOpen && shopMusicClip != null)
            {
                musicAudioSource.clip = shopMusicClip;
                musicAudioSource.Play();
            }
            else
            {
                musicAudioSource.Stop();
            }
        }
    }

    private void SetShopObject(bool show)
    {
        if (animatedShopObject == null || startPoint == null || endPoint == null)
        {
            return;
        }

        shopIsOpen = show;

        if (show)
        {
            animatedShopObject.SetActive(true);
        }
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.clip = clip;
            audioSource.pitch = Random.Range(0.97f, 1.03f);
            audioSource.Play();
        }
    }

    private void SetPlayerVisibility(bool visible)
    {
        if (playerRenderers == null) return;

        foreach (var rend in playerRenderers)
        {
            rend.enabled = visible;
        }
    }

}
