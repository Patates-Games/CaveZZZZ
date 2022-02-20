using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimeController : MonoBehaviour
{
    public enum Times
    {
        Past = 0,
        Now = 1,
        Future = 2
    }

    public LanguageManager languageManager;
    public Timeline timeline;
    public Messages messageManager;
    public Times time = Times.Now;
    public Animator animator;
    public TextMeshProUGUI currentDateText;
    PlayerController playerController;

    private void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        SetTime();
    }

    void SetTime()
    {
        PlayerPrefs.SetInt("time", (int)time);
        timeline.ObjectManager();
    }

    public void GoPast(GameObject gameObject)
    {
        StartCoroutine(ButtonHolder(gameObject));
        if (time == Times.Future) time = Times.Now;
        else if (time == Times.Now) time = Times.Past;
        else
        {
            messageManager.GetSubtitle("NoPast");
            return;
        }
        SetTime();
        animator.SetTrigger("ChangeTime");
        StartCoroutine(DateChanger());
        StartCoroutine(HoldPanel());
    }

    public void GoFuture(GameObject gameObject)
    {
        StartCoroutine(ButtonHolder(gameObject));
        if (time == Times.Past) time = Times.Now;
        else if (time == Times.Now) time = Times.Future;
        else
        {
            messageManager.GetSubtitle("NoFuture");
            return;
        }
        SetTime();
        animator.SetTrigger("ChangeTime");
        StartCoroutine(DateChanger());
        StartCoroutine(HoldPanel());
    }

    IEnumerator ButtonHolder(GameObject gameObject)
    {
        gameObject.SetActive(false);
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(true);
    }
    IEnumerator DateChanger()
    {
        yield return new WaitForSeconds(1f);
        languageManager.SetDate(currentDateText, time);
    }

    IEnumerator HoldPanel()
    {
        playerController.timePanelOpen = false;
        yield return new WaitForSeconds(2f);
        playerController.timePanelOpen = true;
    }

}
