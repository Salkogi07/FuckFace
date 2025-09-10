using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public enum ItemType
{
    HpPotion,
    MpPotion,
    CooldownBuff,
    MpCostBuff,
    Revive
}

public class SimpleInventory : MonoBehaviour
{
    public Text[] itemSlotTexts;
    public Image[] cooldownImages;

    public class Item
    {
        public ItemType type;
        public int quantity;
    }

    private List<Item> inventory = new List<Item>();
    private int maxInventorySlots = 5;

    private float itemCooldown = 2.0f;
    private float currentCooldown = 0f;

    void Start()
    {
        foreach (Image img in cooldownImages)
        {
            img.fillAmount = 0;
        }
    }

    void Update()
    {
        if (currentCooldown > 0)
        {
            currentCooldown -= Time.deltaTime;
            float fill = currentCooldown / itemCooldown;
            foreach (Image img in cooldownImages)
            {
                img.fillAmount = fill;
            }
        }
        else
        {
            foreach (Image img in cooldownImages)
            {
                img.fillAmount = 0;
            }
        }

        if (PlayerManager.instance.GetMasterPlayer() != this.gameObject)
            return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (currentCooldown <= 0)
            {
                UseItem();
            }
        }

        UpdateUI();
    }

    public void AddItem(ItemType itemType)
    {
        foreach (Item item in inventory)
        {
            if (item.type == itemType && item.quantity < 10)
            {
                item.quantity++;
                return;
            }
        }

        if (inventory.Count < maxInventorySlots)
        {
            Item newItem = new Item { type = itemType, quantity = 1 };
            inventory.Add(newItem);
        }
    }

    void UseItem()
    {
        if (inventory.Count == 0)
        {
            return;
        }

        Item itemToUse = inventory[0];
        UIManager.Instance.ShowMsg(itemToUse.type.ToString() + " 아이템 사용!");

        itemToUse.quantity--;

        if (itemToUse.quantity <= 0)
        {
            inventory.RemoveAt(0);
        }

        currentCooldown = itemCooldown;
        foreach (Image img in cooldownImages)
        {
            img.fillAmount = 1;
        }
    }

    void UpdateUI()
    {
        for (int i = 0; i < itemSlotTexts.Length; i++)
        {
            if (i < inventory.Count)
            {
                itemSlotTexts[i].text = inventory[i].type.ToString() + " (" + inventory[i].quantity + ")";
            }
            else
            {
                itemSlotTexts[i].text = "- 비어있음 -";
            }
        }
    }
}