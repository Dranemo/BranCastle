using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionMenu : MonoBehaviour
{
    [SerializeField] Canvas mainMenu;
    [SerializeField] Button[] buttons;

    // Start is called before the first frame update
    void Awake()
    {
        gameObject.SetActive(false);


        buttons[0].onClick.AddListener(Back);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void Back()
    {
        mainMenu.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
}
