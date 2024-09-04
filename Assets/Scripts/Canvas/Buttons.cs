using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Buttons : MonoBehaviour
{
    [Header("Buttons : Normal / Hover / Clicked")]
    [SerializeField] Sprite[] buttonsState;

    private Sprite imageComponent;

    // Start is called before the first frame update
    void Start()
    {
        imageComponent = GetComponent<Image>().sprite;

        imageComponent = buttonsState[0];
    }

    // Hover
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (buttonsState[1] != null)
        {
            imageComponent = buttonsState[1];
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (buttonsState[0] != null)
        {
            imageComponent = buttonsState[0];
        }
    }

    //Click
    public void OnPointerDown(PointerEventData eventData)
    {
        if (buttonsState[2] != null)
        {
            imageComponent = buttonsState[2];
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (buttonsState[0] != null)
        {
            imageComponent = buttonsState[0];
        }
    }
}
