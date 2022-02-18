using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public Messages messages;
    public Items.Item[] items = new Items.Item[5];

    public bool AddItem(Items.Item item)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == item)
            {
                messages.GetSubtitle("AlreadyHave");
                return false;
            }
        }

        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == Items.Item.Empty)
            {
                items[i] = item;
                messages.GetSubtitle("Added");
                return true;
            }
        }
        messages.GetSubtitle("InventoryFull");
        return false;
    }

    public bool UseItem(Items.Item item, bool willErase = true)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == item)
            {
                if (willErase) items[i] = Items.Item.Empty;
                return true;
            }
        }
        return false;
    }
}
