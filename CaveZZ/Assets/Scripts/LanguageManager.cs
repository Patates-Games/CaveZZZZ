using System.Collections;
using System.Collections.Generic;
using System.Xml;
using TMPro;
using UnityEngine;

public class LanguageManager : MonoBehaviour
{
    Dictionary<string, string> dicTR = new Dictionary<string, string>();
    Dictionary<string, string> dicEN = new Dictionary<string, string>();
    public GameObject[] optionalPanels;
    public string language = "english";

    void Start()
    {
        if (PlayerPrefs.HasKey("language")) language = PlayerPrefs.GetString("language");

        CreateXmlDic();
        SetTXT(language);

        for (int i = 0; i < optionalPanels.Length; i++)
        {
            optionalPanels[i].SetActive(false);
        }
    }

    public void SetSubtitle(TextMeshProUGUI text, string subtitle)
    {
        Dictionary<string, string> dic = GetDic();
        dic.TryGetValue(subtitle, out subtitle);

        text.text = subtitle;
    }

    public void SetDate(TextMeshProUGUI text, TimeController.Times time)
    {
        string subtitle;
        Dictionary<string, string> dic = GetDic();

        if (time == TimeController.Times.Future) subtitle = "FutureDate";
        else if (time == TimeController.Times.Now) subtitle = "NowDate";
        else subtitle = "PastDate";
        
        dic.TryGetValue(subtitle, out subtitle);
        text.text = subtitle;
    }

    public void SetTXT(string lang)
    {
        language = lang;
        PlayerPrefs.SetString("language", language);
        Dictionary<string, string> dic = GetDic();

        GameObject[] texts = GameObject.FindGameObjectsWithTag("Text");
        for (int i = 0; i < texts.Length; i++)
        {
            string label = texts[i].gameObject.name;
            dic.TryGetValue(label, out label);

            texts[i].gameObject.GetComponent<TextMeshProUGUI>().text = label;
        }
    }

    Dictionary<string, string> GetDic()
    {
        if (language == "turkish")
            return dicTR;
        else
            return dicEN;
    }

    void CreateXmlDic()
    {
        using var reader = XmlReader.Create("TimeShifter_Data/Resources/lang.xml");
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

    public string GetLabel(string key, string language = "english")
    {
        string label = "";
        if (language == "english") dicEN.TryGetValue(key, out label);
        else if (language == "turkish") dicTR.TryGetValue(key, out label);
        else return "UNDEFINED_TEXT";

        return label;
    }
}
