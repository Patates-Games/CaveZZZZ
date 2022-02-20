using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Items : MonoBehaviour
{
    EnvironmentManager environmentManager;
    GameObject script;
    PlayerController playerController;
    LanguageManager languageManager;
    Inventory inventory;
    Messages messages;
    DoorManager doorManager;
    private void Start()
    {
        script = GameObject.FindGameObjectWithTag("Script");
        doorManager = script.GetComponent<DoorManager>();
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        environmentManager = script.GetComponent<EnvironmentManager>();
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
        DoorNote,

        Lockpick,
        Lever,
        KeyExit,
        KeyNote,
        SafeNote,

        Safe,
        LeverSocket,
    }
    public Item item;

    [HideInInspector]
    public int RangeItemStart = 5;

    [HideInInspector]
    public int RangeItemEnd = 9;

    [HideInInspector]
    public int RangeOtherAfter = 10;

    public void GetInteract()
    {
        if (gameObject.name == "FirstDoorPast")
        {
            FirstDoor.interact = true;
            GetComponent<AudioSource>().Play();
        }
        else if (gameObject.name == "FirstDoorNow" || gameObject.name == "FirstDoorFuture")
        {
            if (!FirstDoor.info)
            {
                StartCoroutine(playerController.GetInfo());
                FirstDoor.info = true;
            }
            GetComponent<AudioSource>().Play();
        }
        if (item == Item.DoorExit)
        {
            InteractExitDoor();
            GetComponent<AudioSource>().Play();
        }
        else if (item == Item.DoorLocked)
        {
            InteractUnlockDoor();
            GetComponent<AudioSource>().Play();
        }
        else if (item == Item.DoorOpen)
        {
            InteractDoor();
            GetComponent<AudioSource>().Play();
        }
        else if (item == Item.DoorNote)
        {
            InteractNoteDoor();
            GetComponent<AudioSource>().Play();
        }

        else if (RangeItemEnd >= (int)item && (int)item >= RangeItemStart) InteractItem();
        else if ((int)item >= RangeOtherAfter) InteractOthers();
    }

    void InteractExitDoor()
    {
        if (FirstDoor.canExit)
        {
            if (inventory.UseItem(Item.KeyExit))
            {
                messages.GetSubtitle("ExitDoorSuccess");
                SceneManager.LoadScene("GameOver", LoadSceneMode.Additive);
            }
            else messages.GetSubtitle("ExitDoorFailedWithoutKey");
        }
        else messages.GetSubtitle("ExitDoorFailed");
    }

    void InteractUnlockDoor()
    {
        Door door;
        if (gameObject.TryGetComponent<Door>(out door))
        {
            if (doorManager.IsUnlockedBefore(gameObject.GetComponent<Door>().doorType))
            {
                messages.GetSubtitle("UnlockedBefore");
                if (!FirstDoor.firstTime)
                {
                    FirstDoor.firstTime = true;
                    GameObject[] texts = GameObject.FindGameObjectsWithTag("Text");
                    for(int i = 0; i < texts.Length; i++)
                    {
                        if(texts[i].name == "InfoText")
                        {
                            texts[i].GetComponent<TextMeshProUGUI>().text = languageManager.GetLabel("UnlockedBeforeInfo", languageManager.language);
                            StartCoroutine(playerController.GetInfo(3f));
                        }
                    }
                }
            }
            else if (inventory.UseItem(Item.Lockpick))
            {
                messages.GetSubtitle("Unlocked");
                doorManager.OpenDoor(gameObject.GetComponent<Door>().doorType);
                environmentManager.OpenDoorInTimeline(gameObject);
            }
            else messages.GetSubtitle("NoLockpick");
        }
        else if (inventory.UseItem(Item.Lockpick))
        {
            messages.GetSubtitle("Unlocked");
            environmentManager.OpenDoorInTimeline(gameObject);
        }
        else messages.GetSubtitle("NoLockpick");
    }

    void InteractDoor()
    {
        environmentManager.OpenDoorInTimeline(gameObject);
    }

    void InteractNoteDoor()
    {
        if (script.GetComponent<TimeController>().time == TimeController.Times.Now)
        {
            if (inventory.UseItem(Item.KeyNote))
            {
                environmentManager.OpenDoorInTimeline(gameObject);
            }
            else
            {
                messages.GetSubtitle("NoNote");
            }
        }
        else messages.GetSubtitle("NoNote");
    }

    void InteractItem()
    {
        if (inventory.AddItem(item))
        {
            gameObject.transform.position = new Vector3(1000f, 1000f, 1000f);
            //gameObject.SetActive(false);
            //Destroy(gameObject);
        }
    }

    void InteractOthers()
    {
        if (item == Item.Safe)
        {
            if (inventory.FindItem(Item.SafeNote))
            {
                if(script.GetComponent<TimeController>().time == TimeController.Times.Now)
                {
                    inventory.UseItem(Item.SafeNote);
                    inventory.AddItem(Item.KeyExit);
                    playerController.interactText.GetComponent<TextMeshProUGUI>().text = languageManager.GetLabel("KeyExit", languageManager.language);
                    playerController.interactShown.GetComponent<RectTransform>().sizeDelta = new Vector2(200f, 200f);
                    playerController.interactShown.GetComponent<Image>().sprite = playerController.keyExitSprite;
                    gameObject.GetComponent<Items>().enabled = false;
                    gameObject.GetComponent<BoxCollider2D>().enabled = false;

                    gameObject.tag = "Untagged";
                }
            }
        }
        else if (item == Item.LeverSocket)
        {
            if (gameObject.name != "ExitLeverSocket")
            {
                if (inventory.FindItem(Item.Lever))
                {
                    GetComponent<AudioSource>().Play();

                    inventory.UseItem(Item.Lever, false);
                    GameObject.FindGameObjectWithTag("Shelf").GetComponent<Animator>().SetTrigger("LeverUsed");
                    StartCoroutine(GetLight(GameObject.FindGameObjectWithTag("Shelf").GetComponentInParent<Light2D>()));
                    messages.GetSubtitle("LeverUsed");
                    gameObject.GetComponent<BoxCollider2D>().enabled = false;
                }
                else
                {
                    messages.GetSubtitle("NoLever");
                    gameObject.tag = "Untagged";
                }
            }
            else
            {
                if (inventory.FindItem(Item.Lever))
                {
                    GetComponent<AudioSource>().Play();

                    inventory.UseItem(Item.Lever, false);
                    FirstDoor.canExit = true;
                    GameObject.FindGameObjectWithTag("Grid").GetComponent<Animator>().SetTrigger("ExitOpen");
                    messages.GetSubtitle("ExitLeverUsed");
                    gameObject.GetComponent<BoxCollider2D>().enabled = false;
                }
                else
                {
                    messages.GetSubtitle("NoLever");
                    gameObject.tag = "Untagged";
                }
            }
        }
    }

    IEnumerator GetLight(Light2D light2D)
    {
        yield return new WaitForSeconds(.5f);
        light2D.enabled = true;
    }
}
