using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public LanguageManager languageManager;
    public GameObject interactPanel;
    public GameObject interactShown;
    public GameObject interactText;

    public GameObject datePanel;
    public bool canMove = true;
    public bool timePanelOpen = true;

    public Sprite keyExitSprite;
    GameObject interactItem = null;
    Rigidbody2D rbody;
    public float speed = 500f;
    Animator animator;
    TimeController timeController;
    Inventory inventory;
    Messages messages;
    void Start()
    {
        messages = GameObject.FindGameObjectWithTag("Script").GetComponent<Messages>();
        timeController = GameObject.FindGameObjectWithTag("Script").GetComponent<TimeController>();
        inventory = GameObject.FindGameObjectWithTag("Script").GetComponent<Inventory>();
        rbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetButtonDown("TimeKey"))
        {
            if (timePanelOpen)
            {
                if (!datePanel.activeSelf)
                {
                    datePanel.SetActive(true);
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;
                }
                else
                {
                    datePanel.SetActive(false);
                    Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Locked;
                }
            }
        }


        //if (Input.GetButtonUp("Run")) speed = 2500f;
        //if (Input.GetButtonDown("Run")) speed = 4000f;
        if (canMove)
        {
            float walkingLeft = Input.GetAxis("Horizontal");
            float walkingRight = Input.GetAxis("Horizontal");
            float walkingUp = Input.GetAxis("Vertical");
            float walkingDown = Input.GetAxis("Vertical");

            Vector2 move = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * Time.deltaTime * speed;
            if(move != Vector2.zero)
            {
                rbody.velocity = move;
                 
                AnimationScript(walkingLeft, walkingRight, walkingUp, walkingDown);
            }
            else
            {
                rbody.velocity = Vector2.zero;
                animator.SetBool("walkingUp", false);
                animator.SetBool("walkingDown", false);
                animator.SetBool("walkingSide", false);
            }
        }

        if (Input.GetButtonDown("Interact") && interactItem != null)
        {

            if (FirstDoor.info == false)
            {
                if (interactItem.name == "FirstDoorPast" || interactItem.name == "FirstDoorNow" || interactItem.name == "FirstDoorFuture")
                {
                    FirstDoor.interact = true;
                }
            }
            Items interacted = interactItem.gameObject.GetComponent<Items>();

            if (interacted.RangeItemEnd >= (int)interacted.item && (int)interacted.item >= interacted.RangeItemStart)
            {
                CanMove(false);
                interactShown.GetComponent<Image>().sprite = interactItem.GetComponent<SpriteRenderer>().sprite;
                
                if(interacted.item == Items.Item.Lever)
                    interactShown.GetComponent<RectTransform>().sizeDelta = interactItem.GetComponent<SpriteRenderer>().sprite.pivot * 10;
                else
                    interactShown.GetComponent<RectTransform>().sizeDelta = interactItem.GetComponent<SpriteRenderer>().sprite.pivot * 2;
                
                languageManager.SetSubtitle(interactText.GetComponent<TextMeshProUGUI>(), interactItem.GetComponent<Items>().item.ToString());
                interactPanel.SetActive(true);
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            else if (interacted.item == Items.Item.LeverSocket || interacted.item == Items.Item.DoorNote)
            {
            }
            else if ((int)interacted.item >= interacted.RangeOtherAfter)
            {
                CanMove(false);
                interactShown.GetComponent<Image>().sprite = interactItem.GetComponent<SpriteRenderer>().sprite;

                interactText.GetComponent<TextMeshProUGUI>().text = languageManager.GetLabel(interacted.item.ToString());
                if (interacted.item == Items.Item.Safe)
                {
                    if(timeController.time != TimeController.Times.Now)
                    {
                        if(inventory.FindItem(Items.Item.SafeNote))
                            interactText.GetComponent<TextMeshProUGUI>().text = languageManager.GetLabel("SafeEmpty");
                        else
                            interactText.GetComponent<TextMeshProUGUI>().text = languageManager.GetLabel("Safe");
                    }
                    interactShown.GetComponent<RectTransform>().sizeDelta = interactItem.GetComponent<SpriteRenderer>().sprite.pivot * 15;
                }
                else
                {
                    interactShown.GetComponent<RectTransform>().sizeDelta = interactItem.GetComponent<SpriteRenderer>().sprite.pivot * 2;
                }
                
                interactPanel.SetActive(true);
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }

            interactItem.GetComponent<Items>().GetInteract();
        }
    }

    void CanMove(bool canItMove)
    {
        canMove = canItMove;
        if (!canItMove) rbody.velocity = Vector2.zero;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        interactItem = collision.gameObject;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        interactItem = null;
    }

    public void CloseInteractPanel()
    {
        interactPanel.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        CanMove(true);
    }

    void AnimationScript(float walkingLeft, float walkingRight, float walkingUp, float walkingDown)
    {
        bool up, down, left, right;

        if (walkingUp > 0.2f)
        {
            up = true;
            down = false;
        }
        else if (walkingDown < -0.2f)
        {
            up = false;
            down = true;
        }
        else
        {
            up = false;
            down = false;
        }
        //
        if (walkingRight > 0.2f)
        {
            right = true;
            left = false;
            Vector3 characterScale = transform.localScale;
            characterScale.x = -0.25f;

            transform.localScale = characterScale;
        }
        else if (walkingLeft < -0.2f)
        {
            right = false;
            left = true;
            Vector3 characterScale = transform.localScale;
            characterScale.x = 0.25f;

            transform.localScale = characterScale;
        }
        else
        {
            right = false;
            left = false;
        }

        if(left || right)
        {
            up = false;
            down = false;
        }

        animator.SetBool("walkingUp", up);
        animator.SetBool("walkingDown", down);
        animator.SetBool("walkingSide", right || left);
    }
}
