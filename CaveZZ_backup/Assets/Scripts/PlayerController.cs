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
    bool canMove = true;
    GameObject interactItem = null;
    Rigidbody2D rbody;
    public float speed = 2500f;

    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.GetButtonDown("TimeKey"))
        {
            datePanel.SetActive(!datePanel.activeSelf);
            if(datePanel.activeSelf) CanMove(false);
            else CanMove(true);
        }

        if (Input.GetButtonUp("Run")) speed = 2500f;
        if (Input.GetButtonDown("Run")) speed = 4000f;
        if (canMove) rbody.velocity = new Vector2(Input.GetAxis("Horizontal"), 0f) * Time.deltaTime * speed;

        if (Input.GetButtonDown("Interact") && interactItem != null)
        {
            CanMove(false);
            interactShown.GetComponent<Image>().sprite = interactItem.GetComponent<SpriteRenderer>().sprite;
            interactShown.GetComponent<RectTransform>().sizeDelta = interactItem.GetComponent<SpriteRenderer>().sprite.pivot * 2;
            languageManager.SetSubtitle(interactText.GetComponent<TextMeshProUGUI>(), interactItem.name);
            interactPanel.SetActive(true);
            Cursor.visible = true;
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
        CanMove(true);
    }

    IEnumerator InteractCoroutine()
    {
        yield return new WaitForSeconds(1);
        CanMove(true);
    }
}
