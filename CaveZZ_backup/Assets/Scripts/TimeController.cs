using System.Collections;
using System.Collections.Generic;
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

    public Messages messageManager;
    public Times time = Times.Now;
    public Animator animator;

    private void Start()
    {
        SetTime();
    }

    void SetTime()
    {
        PlayerPrefs.SetInt("time", (int)time);
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
        animator.SetTrigger("GoPast");
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
        animator.SetTrigger("GoFuture");
    }

    IEnumerator ButtonHolder(GameObject gameObject)
    {
        gameObject.SetActive(false);
        yield return new WaitForSeconds(1);
        gameObject.SetActive(true);
    }


}
