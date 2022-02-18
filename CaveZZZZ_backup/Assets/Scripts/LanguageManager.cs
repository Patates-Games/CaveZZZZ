using System.Collections;
using System.Collections.Generic;
using System.Xml;
using TMPro;
using UnityEngine;

public class LanguageManager : MonoBehaviour
{
    Dictionary<string, string> dicTR = new Dictionary<string, string>();
    Dictionary<string, string> dicEN = new Dictionary<string, string>();
    public GameObject optionsPanel;
    public string language = "english";

    void Start()
    {
        CreateXmlDic();
        SetTXT(language);
        if (optionsPanel != null) optionsPanel.SetActive(false);
    }

    public void SetTXT(string lang)
    {
        Debug.Log(lang.ToString());
        Dictionary<string, string> dic = GetDic(lang.ToString());

        GameObject[] texts = GameObject.FindGameObjectsWithTag("Text");
        for (int i = 0; i < texts.Length; i++)
        {
            string label = texts[i].gameObject.name;
            dic.TryGetValue(label, out label);

            texts[i].gameObject.GetComponent<TextMeshProUGUI>().text = label;
        }
    }

    Dictionary<string, string> GetDic(string lang)
    {
        if (lang == "turkish")
            return dicTR;
        else
            return dicEN;
    }

    void CreateXmlDic()
    {
        using var reader = XmlReader.Create("Assets/lang.xml");
        reader.ReadToFollowing("text");

        do
        {
            reader.MoveToFirstAttribute();
            string key = reader.Value;

            reader.ReadToFollowing("english");
            dicEN.Add(key, reader.ReadElementContentAsString());

            reader.ReadToFollowing("turkish");
            dicTR.Add(key, reader.ReadElementContentAsString());
        } while (reader.ReadToFollowing("text"));
    }

    string GetLabel(string key, string language = "english")
    {
        string label = "";
        if (language == "english") dicEN.TryGetValue(key, out label);
        else if (language == "turkish") dicTR.TryGetValue(key, out label);
        else return "UNDEFINED_TEXT";

        return label;
    }
}
