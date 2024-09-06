using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvaTutoriel : MonoBehaviour
{
    [SerializeField] Button[] buttons;
    [SerializeField] GameObject[] pages;
    int page = 0;


    void Start()
    {
        buttons[0].onClick.AddListener(Previous);
        buttons[1].onClick.AddListener(Next);
        buttons[2].onClick.AddListener(Skip);

        foreach (GameObject text in pages)
        {
            text.SetActive(false);
        }
        pages[0].SetActive(true);
        buttons[0].gameObject.SetActive(false);
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
        Destroy(GameObject.Find("CanvasBackground"));
        ScenesManager.Instance.LoadScene("Level-1");
    }
}
