using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items : MonoBehaviour
{
    public Inventory inventory;
    public Messages messages;

    public enum Item
    {
        Empty,
        RedKey,
        GreenKey,
        BlueKey,
        RedDoor,
        GreenDoor,
        BlueDoor
    }
    public Item item;

    public void GetInteract()
    {
        if(3 >= (int)item && (int)item >= 1)
        {
            inventory.AddItem(item);
            Destroy(gameObject);
            return;
        }
        else if (6 >= (int)item && (int)item >= 4)
        {
            if (item == Item.RedDoor)
            {
                if (inventory.UseItem(Item.RedKey)) messages.GetSubtitle("Unlocked");
                else messages.GetSubtitle("WrongKey");
                return;
            }
            if (item == Item.GreenDoor)
            {
                if (inventory.UseItem(Item.GreenKey)) messages.GetSubtitle("Unlocked");
                else messages.GetSubtitle("WrongKey");
                return;
            }
            if (item == Item.BlueDoor)
            {
                if (inventory.UseItem(Item.BlueKey)) messages.GetSubtitle("Unlocked");
                else messages.GetSubtitle("WrongKey");
                return;
            }
        }
    }
}
