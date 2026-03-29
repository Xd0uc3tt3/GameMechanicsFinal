using UnityEngine;
using TMPro;

public class TeleportPad : MonoBehaviour
{
    public Transform destination;
    public TeleportPad destinationPad;

    public int cost = 0;
    public AudioClip spendSound;
    public AudioClip failSound;
    public TextMeshPro costText;

    private static TeleportPad lastUsedPad;

    void Start()
    {
        costText.text = $"Price: {cost}";
    }

    void Update()
    {
        costText.transform.LookAt(Camera.main.transform);
        costText.transform.Rotate(0, 180, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        if (lastUsedPad == this)
        {
            return;
        }

        PlayerScript player = other.GetComponent<PlayerScript>();

        if (player == null)
        {
            return;
        }

        if (player.coins >= cost)
        {
            player.coins -= cost;

            player.audioSource.PlayOneShot(spendSound);

            lastUsedPad = destinationPad;
            other.transform.position = destination.position;
        }
        else
        {
            if (failSound != null)
                player.audioSource.PlayOneShot(failSound);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (lastUsedPad == this)
        {
            lastUsedPad = null;
        }
    }
}
