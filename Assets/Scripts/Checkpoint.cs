using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private PlayerScript player;
    public Transform spawnDestination;

    public Renderer checkpoint;
    public Material inactiveVisual;
    public Material activeVisual;

    public AudioSource audioSource;
    public AudioClip activateSound;

    void Start()
    {
        player = FindObjectOfType<PlayerScript>();

        if (checkpoint != null && inactiveVisual != null)
        {
            checkpoint.material = inactiveVisual;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player.SetCheckpoint(spawnDestination);

            if (checkpoint != null && activeVisual != null)
            {
                checkpoint.material = activeVisual;
                audioSource.PlayOneShot(activateSound);
            }
        }
    }
}
