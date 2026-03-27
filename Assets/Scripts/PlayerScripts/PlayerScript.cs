using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour
{
    public Transform spawnPoint;

    public int coins = 0;
    public int lives = 3;

    private Rigidbody rb;
    public bool isRespawning = false;

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

        StartCoroutine(EndRespawnFrame());
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
        if (isRespawning)
        {
            return;
        }

        isRespawning = true;

        if (lives > 0)
        {
            lives--;
            Respawn();
        }
        else
        {
            Debug.Log("Game Over");
            isRespawning = false;
        }
    }

    private IEnumerator EndRespawnFrame()
    {
        yield return new WaitForSeconds(0.5f);
        isRespawning = false;
    }
}
