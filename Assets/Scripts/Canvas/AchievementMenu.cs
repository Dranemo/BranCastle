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
    [SerializeField] TMP_FontAsset font;

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

        List<Achievement> listAch = AchievementManager.instance.achievements;


        for (int i = 0; i < listAch.Count; i++)
        {
            GameObject go = new GameObject();
            go.name = listAch[i].id;

            go.AddComponent<TextMeshProUGUI>().text = listAch[i].description;
            go.GetComponent<TextMeshProUGUI>().fontSize = 20;
            go.GetComponent<TextMeshProUGUI>().font = font;
            go.GetComponent <TextMeshProUGUI>().alignment = TextAlignmentOptions.Center;
            go.GetComponent<TextMeshProUGUI>().color = listAch[i].isUnlocked ? Color.green : Color.red;




            int whereX = -1;
            
            if (i % 2 != 0)
            {
                whereX = 1;
            }
            else if (i == listAch.Count - 1)
            {
                whereX = 0;
            }

            int whereY = i / 2;


            go.transform.position = new Vector3(150 * whereX, -(whereY * 70), 0) + folderAch.transform.position;

            GameObject back = new();
            back.name = "back " + listAch[i].id;
            back.AddComponent<Image>().color = Color.black;
            back.GetComponent<Image>().color = new Color(back.GetComponent<Image>().color.r, back.GetComponent<Image>().color.g, back.GetComponent<Image>().color.b, 0.75f);
            back.transform.localPosition = new Vector3(go.transform.position.x, go.transform.position.y, -0.5f) ;
            back.GetComponent<RectTransform>().sizeDelta = go.GetComponent<RectTransform>().sizeDelta;




            back.transform.SetParent(folderAch.transform);
            go.transform.SetParent(back.transform);
        }


        mainMenu.gameObject.SetActive(false);
    }


    void Back()
    {
        mainMenu.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
}
