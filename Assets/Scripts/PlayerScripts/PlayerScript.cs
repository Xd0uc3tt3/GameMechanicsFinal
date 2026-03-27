using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public int coins = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            coins += 1;
            Destroy(other.gameObject);
        }
    }
}
