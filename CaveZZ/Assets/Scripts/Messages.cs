using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Messages : MonoBehaviour
{
    public LanguageManager languageManager;
    public TextMeshProUGUI textHolder;
    public GameObject subtitlePanel;

    private void Start()
    {
    }

    public void GetSubtitle(string titleId, float time = 2f)
    {
        StartCoroutine(ShowSubtitle(titleId, time));
    }

    IEnumerator ShowSubtitle(string titleId, float time)
    {
        languageManager.SetSubtitle(textHolder, titleId);
        subtitlePanel.SetActive(true);
        yield return new WaitForSeconds(2);
        subtitlePanel.SetActive(false);
    }
}
