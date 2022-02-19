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
    GameObject interactItem = null;
    Rigidbody2D rbody;
    public float speed = 1500f;
    Animator animator;


    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetButtonDown("TimeKey"))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            datePanel.SetActive(!datePanel.activeSelf);
            if(datePanel.activeSelf) CanMove(false);
            else CanMove(true);
        }

        //if (Input.GetButtonUp("Run")) speed = 2500f;
        //if (Input.GetButtonDown("Run")) speed = 4000f;
        if (canMove)
        {
            bool walkingLeft = false;
            bool walkingRight = false;
            bool walkingUp = false;
            bool walkingDown = false;

            if (Input.GetAxis("Horizontal") < 0) walkingLeft = true;
            if (Input.GetAxis("Horizontal") > 0) walkingRight = true;
            if (Input.GetAxis("Vertical") > 0) walkingUp = true;
            if (Input.GetAxis("Vertical") < 0) walkingDown = true;

            Vector2 move = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * Time.deltaTime * speed;
            if(move != Vector2.zero)
            {
                rbody.velocity = move;
                animator.SetBool("walkingUp", walkingUp);
                animator.SetBool("walkingDown", walkingDown);
                animator.SetBool("walkingSide", walkingLeft || walkingRight);

                if (walkingRight)
                {
                    Vector3 characterScale = transform.localScale;
                    characterScale.x = -0.25f;

                    transform.localScale = characterScale;
                }
                else
                {
                    Vector3 characterScale = transform.localScale;
                    characterScale.x = 0.25f;

                    transform.localScale = characterScale;
                }
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
            Items interacted = interactItem.gameObject.GetComponent<Items>();

            if (interacted.RangeItemEnd >= (int)interacted.item && (int)interacted.item >= interacted.RangeItemStart)
            {
                CanMove(false);
                interactShown.GetComponent<Image>().sprite = interactItem.GetComponent<SpriteRenderer>().sprite;
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
                interactShown.GetComponent<RectTransform>().sizeDelta = interactItem.GetComponent<SpriteRenderer>().sprite.pivot * 2;
                interactText.GetComponent<TextMeshProUGUI>().text = languageManager.GetLabel(interacted.item.ToString());
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
        Debug.Log("e");
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        interactItem = null;
        Debug.Log("o");
    }

    public void CloseInteractPanel()
    {
        interactPanel.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        CanMove(true);
    }

    IEnumerator InteractCoroutine()
    {
        yield return new WaitForSeconds(1);
        CanMove(true);
    }
}
