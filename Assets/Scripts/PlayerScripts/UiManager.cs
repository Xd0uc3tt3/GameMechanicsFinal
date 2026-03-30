using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI coinText;
    public TextMeshProUGUI livesText;
    public PlayerScript player;

    void Update()
    {
        coinText.text = "x " + player.coins;
        livesText.text = "x " + player.lives;
    }
}
