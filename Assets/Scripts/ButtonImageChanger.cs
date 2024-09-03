using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonImageChanger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite hoverSprite;
    [SerializeField] private Sprite clickSprite;

    private Image buttonImage;

    void Awake()
    {

        buttonImage = GetComponent<Image>();
        if (buttonImage == null)
        {
            //////Debug.LogError("Aucun composant Image trouvé sur le bouton.");
        }
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        buttonImage.sprite = hoverSprite;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        buttonImage.sprite = normalSprite;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        buttonImage.sprite = clickSprite;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        buttonImage.sprite = hoverSprite;
    }
}
