using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI coinText;
    public PlayerScript player;

    void Update()
    {
        coinText.text = "Coins: " + player.coins;
    }
}
