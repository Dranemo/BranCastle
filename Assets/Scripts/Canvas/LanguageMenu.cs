using UnityEngine;
using UnityEngine.UI;

public class LanguageMenu : MonoBehaviour
{
    [SerializeField] Canvas mainMenu;
    [SerializeField] Button[] buttons;

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

    void Back()
    {
        mainMenu.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
}
