using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CanvaPause : MonoBehaviour
{
    // Start is called before the first frame update


    [SerializeField] GameObject[] texts;


    void Awake()
    {
        texts[0].GetComponent<TextMeshProUGUI>().text = LanguageManager.GetText("pause");
        texts[1].GetComponent<TextMeshProUGUI>().text = LanguageManager.GetText("resume");
        texts[2].GetComponent<TextMeshProUGUI>().text = LanguageManager.GetText("menu");
    }
}
