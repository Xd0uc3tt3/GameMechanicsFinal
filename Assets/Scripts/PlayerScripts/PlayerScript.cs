using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour
{
    public Transform spawnPoint;

    public int coins = 0;
    public int lives = 3;

    public int RespawnCoinReward;

    private Rigidbody rb;
    public bool isRespawning = false;

    public AudioSource audioSource;
    public AudioClip coinSound;

    void Start()
    {
        RespawnCoinReward = coins;
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
            PlaySound(coinSound);

            CoinSpin coin = other.GetComponent<CoinSpin>();
            if (coin != null)
            {
                coin.Collect(transform);
            }
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

    void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.pitch = Random.Range(0.97f, 1.03f);
            audioSource.PlayOneShot(clip);
        }
    }
}
