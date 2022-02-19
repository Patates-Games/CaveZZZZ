using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Items : MonoBehaviour
{
    public GameObject script;
    PlayerController playerController;
    LanguageManager languageManager;
    Inventory inventory;
    Messages messages;

    private void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>(); ;
        languageManager = script.GetComponent<LanguageManager>();
        inventory = script.GetComponent<Inventory>();
        messages = script.GetComponent<Messages>();
    }

    public enum Item
    {
        Empty,
        DoorExit,
        DoorLocked,
        DoorOpen,

        Lockpick,
        Lever,
        KeyExit,
        SafeNote,

        Safe,
        LeverSocket,
    }
    public Item item;
    public int RangeItemStart = 4;
    public int RangeItemEnd = 7;
    public int RangeOtherAfter = 8;

    public void GetInteract()
    {
        if (item == Item.DoorExit) InteractExitDoor();
        else if (item == Item.DoorLocked) InteractUnlockDoor();
        else if (item == Item.DoorOpen) InteractDoor();
        else if (RangeItemEnd >= (int)item && (int)item >= RangeItemStart) InteractItem();
        else if ((int)item >= RangeOtherAfter) InteractOthers();
    }

    void InteractExitDoor()
    {
        if (inventory.UseItem(Item.KeyExit))
        {
            messages.GetSubtitle("ExitDoorSuccess");
            // END GAME SCRIPT
        }
        else messages.GetSubtitle("ExitDoorFailed");
    }

    void InteractUnlockDoor()
    {
        if (inventory.UseItem(Item.Lockpick))
        {
            messages.GetSubtitle("Unlocked");
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
        }
        else messages.GetSubtitle("NoLockpick");
    }

    void InteractDoor()
    {
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
    }

    void InteractItem()
    {
        if (inventory.AddItem(item))
        {
            Destroy(gameObject);
        }
    }

    void InteractOthers()
    {
        if (item == Item.Safe)
        {
            if (inventory.FindItem(Item.SafeNote))
            {
                inventory.UseItem(Item.SafeNote);
                inventory.AddItem(Item.KeyExit);
                playerController.interactText.GetComponent<TextMeshProUGUI>().text = languageManager.GetLabel("KeyExit");
                gameObject.tag = "Untagged";
            }
        }
        else if (item == Item.LeverSocket)
        {
            if (inventory.FindItem(Item.Lever))
            {
                inventory.UseItem(Item.Lever);
                GameObject.FindGameObjectWithTag("Shelf").GetComponent<Animator>().SetTrigger("LeverUsed");
                StartCoroutine(GetLight(GameObject.FindGameObjectWithTag("Shelf").GetComponentInParent<Light2D>()));
                messages.GetSubtitle("LeverUsed");
            }
            else
            {
                messages.GetSubtitle("NoLever");
                gameObject.tag = "Untagged";
            }
        }
    }

    IEnumerator GetLight(Light2D light2D)
    {
        yield return new WaitForSeconds(.5f);
        light2D.enabled = true;
    }
}
