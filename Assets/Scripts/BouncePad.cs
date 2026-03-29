using UnityEngine;

public class BouncePad : MonoBehaviour
{
    public float launchForce = 20f;
    public int cost = 10;

    public AudioClip spendSound;
    public AudioClip failSound;

    private PlayerScript player;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.GetComponent<PlayerScript>();

            if (player.coins >= cost)
            {
                player.coins -= cost;
                player.audioSource.PlayOneShot(spendSound);
                Launch(other);
            }
            else
            {
                //player.audioSource.PlayOneShot(failSound);
            }
        }
    }

    void Launch(Collider player)
    {
        Rigidbody rb = player.GetComponent<Rigidbody>();
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        rb.AddForce(Vector3.up * launchForce, ForceMode.Impulse);
    }
}
