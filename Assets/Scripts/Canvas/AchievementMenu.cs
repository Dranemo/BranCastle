using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AchievementMenu : MonoBehaviour
{
    [SerializeField] Canvas mainMenu;
    [SerializeField] Button[] buttons;
    [SerializeField] GameObject folderAch;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
        buttons[0].onClick.AddListener(Back);

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void Activate()
    {
        gameObject.SetActive(true);

        foreach (Achievement ach in AchievementManager.instance.achievements)
        {
            GameObject go = new GameObject();
            go.name = ach.id;
            go.transform.SetParent(folderAch.transform);

            ////Debug.Log(go.transform.position);

            go.AddComponent<TextMeshProUGUI>().text = ach.description;
            go.GetComponent<TextMeshProUGUI>().fontSize = 20;
            go.GetComponent<TextMeshProUGUI>().color = ach.isUnlocked ? Color.green : Color.red;
            go.transform.position = new Vector3(0, 0, 0) + folderAch.transform.position;
            go.transform.localScale = new Vector3(1, 1, 1);
        }

        mainMenu.gameObject.SetActive(false);
    }


    void Back()
    {
        mainMenu.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
}
