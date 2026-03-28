using UnityEngine;
using TMPro;

public class ShopUIManager : MonoBehaviour
{
    public PlayerScript player;

    public TMP_Text coinsText;

    public int item1Price = 10;
    public int item2Price = 25;
    public int item3Price = 10;
    public int item4Price = 25;
    public int item5Price = 10;

    private void OnEnable()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        if (coinsText != null && player != null)
        {
            coinsText.text = "Coins: " + player.coins;
        }
    }

    public void BuyItem1()
    {
        PurchaseItem(item1Price, "Item 1");
    }

    public void BuyItem2()
    {
        PurchaseItem(item2Price, "Item 2");
    }

    public void BuyItem3()
    {
        PurchaseItem(item3Price, "Item 3");
    }

    public void BuyItem4()
    {
        PurchaseItem(item4Price, "Item 4");
    }

    public void BuyItem5()
    {
        PurchaseItem(item5Price, "Item 5");
    }

    private void PurchaseItem(int price, string itemName)
    {
        if (player.coins >= price)
        {
            player.coins -= price;
            Debug.Log(itemName + " purchased!");
            UpdateUI();
        }
        else
        {
            Debug.Log("Not enough coins to buy " + itemName);
        }
    }

    public void CloseShop()
    {
        ShopTrigger shopTrigger = FindObjectOfType<ShopTrigger>();
        if (shopTrigger != null)
        {
            shopTrigger.ToggleShop();
        }
    }
}
