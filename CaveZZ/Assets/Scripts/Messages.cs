using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Messages : MonoBehaviour
{
    public LanguageManager languageManager;
    public TextMeshProUGUI textHolder;
    public GameObject subtitlePanel;

    public void GetSubtitle(string titleId, float time = 2f)
    {
        if (FirstDoor.interact)
        {
            StartCoroutine(ShowSubtitle("StartInfo", 5f));
            FirstDoor.info = true;
            FirstDoor.interact = false;
        } else
        {
            StartCoroutine(ShowSubtitle(titleId, time));
        }
    }

    IEnumerator ShowSubtitle(string titleId, float time)
    {
        languageManager.SetSubtitle(textHolder, titleId);
        subtitlePanel.SetActive(true);
        yield return new WaitForSeconds(time);
        subtitlePanel.SetActive(false);
    }
}
