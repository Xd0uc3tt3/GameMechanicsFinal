using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private PlayerScript player;
    public Transform spawnDestination;

    void Start()
    {
        player = FindObjectOfType<PlayerScript>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player.SetCheckpoint(spawnDestination);
        }
    }
}
