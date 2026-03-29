using UnityEngine;
using TMPro;

public class BouncePad : MonoBehaviour
{
    public float launchForce = 20f;
    public int cost = 10;

    public AudioClip spendSound;
    public AudioClip failSound;
    public TextMeshPro costText;

    private PlayerScript player;
    private PlayerInput playerInput;

    void Start()
    {
        costText.text = $"Price: {cost}";
    }

    void Update()
    {
        costText.transform.LookAt(Camera.main.transform);
        costText.transform.Rotate(0, 180, 0);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerScript player = other.GetComponent<PlayerScript>();
            PlayerInput playerInput = other.GetComponent<PlayerInput>();

            if (player.coins >= cost)
            {
                player.coins -= cost;
                player.audioSource.PlayOneShot(spendSound);
                Launch(other.GetComponent<Rigidbody>(), playerInput);
            }
            else
            {
                player.audioSource.PlayOneShot(failSound);
            }
        }
    }

    void Launch(Rigidbody rb, PlayerInput playerInput)
    {
        rb.linearVelocity = Vector3.zero;
        rb.AddForce(transform.up * launchForce, ForceMode.Impulse);
        rb.GetComponent<PlayerInput>().isGrounded = false;
        playerInput.jumpCount = 1;
    }
}