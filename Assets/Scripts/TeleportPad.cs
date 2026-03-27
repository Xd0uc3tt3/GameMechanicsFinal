using UnityEngine;

public class TeleportPad : MonoBehaviour
{
    public Transform destination;
    public TeleportPad destinationPad;

    private static TeleportPad lastUsedPad;

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

        lastUsedPad = destinationPad;

        other.transform.position = destination.position;
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
