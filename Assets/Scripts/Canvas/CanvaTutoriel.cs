using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvaTutoriel : MonoBehaviour
{
    [SerializeField] Button[] buttons;
    [SerializeField] GameObject[] pages;
    [SerializeField] GameObject Background;
    int page = 0;


    void Start()
    {
        buttons[0].onClick.AddListener(Previous);
        buttons[0].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = LanguageManager.GetText("previous");
        buttons[1].onClick.AddListener(Next);
        buttons[1].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = LanguageManager.GetText("next");
        buttons[2].onClick.AddListener(Skip);
        buttons[2].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = LanguageManager.GetText("skip");


        pages[0].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = LanguageManager.GetText("tutoP1Text");
        pages[0].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = LanguageManager.GetText("tutoP1Main");
        pages[1].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = LanguageManager.GetText("tutoP2Text");
        pages[1].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = LanguageManager.GetText("tutoP2Main");
        pages[2].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = LanguageManager.GetText("tutoP3Text");
        pages[2].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = LanguageManager.GetText("tutoP3Main");
        pages[3].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = LanguageManager.GetText("tutoP4Text");
        pages[3].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = LanguageManager.GetText("tutoP4Main");
        pages[4].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = LanguageManager.GetText("tutoP5Text");
        pages[4].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = LanguageManager.GetText("tutoP5Main");
        pages[5].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = LanguageManager.GetText("tutoP6Text");
        pages[5].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = LanguageManager.GetText("tutoP6Main");




        foreach (GameObject text in pages)
        {
            text.SetActive(false);
        }
        pages[0].SetActive(true);
        buttons[0].gameObject.SetActive(false);

        if(GameObject.FindGameObjectWithTag("MenuBG") == null)
        {
            Instantiate(Background, new Vector3(0, 0, 0), Quaternion.identity);
        }
    }

    void Next()
    {
        if(page != pages.Length - 1)
        {
            pages[page].SetActive(false);
            page++;
            pages[page].SetActive(true);
            buttons[0].gameObject.SetActive(true);

            if(page == pages.Length - 1)
            {
                buttons[1].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Lancer Le Jeu";
            }
        }
        else if (page == pages.Length - 1)
        {
            Skip();
        }
    }

    void Previous()
    {
        if(page != 0)
        {
            pages[page].SetActive(false);
            page--;
            pages[page].SetActive(true);

            if(page != pages.Length - 1)
            {
                buttons[1].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Continuer";
            }

            if(page == 0)
            {
                buttons[0].gameObject.SetActive(false);
            }
        }
    }

    void Skip()
    {
        ScenesManager.Instance.LoadScene("Level-1");
        Destroy(GameObject.FindGameObjectWithTag("MenuBG"));
    }
}
