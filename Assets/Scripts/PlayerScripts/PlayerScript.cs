using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public Transform spawnPoint;

    public int coins = 0;
    public int lives = 3;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (spawnPoint != null)
        {
            transform.position = spawnPoint.position;
            transform.rotation = spawnPoint.rotation;
        }
    }

    public void Respawn()
    {
        if (spawnPoint != null)
        {
            transform.position = spawnPoint.position;
            transform.rotation = spawnPoint.rotation;

            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            coins += 1;
            Destroy(other.gameObject);
        }

        if (other.CompareTag("TestHazard"))
        {
            LoseLife();
        }
    }

    public void LoseLife()
    {
        lives--;
        Debug.Log("LoseLife called, lives now: " + lives);

        if (lives > 0)
        {
            Respawn();
        }
        else
        {
            lives = 0;
            Debug.Log("Game Over");
        }
    }
}
