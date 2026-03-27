using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI coinText;
    public TextMeshProUGUI livesText;
    public PlayerScript player;

    void Update()
    {
        coinText.text = "Coins: " + player.coins;
        livesText.text = "Lives: " + player.lives;
    }
}
