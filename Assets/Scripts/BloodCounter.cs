using TMPro;
using UnityEngine;

public class BloodCounter : MonoBehaviour
{
    GameManager manager;
    private TextMeshProUGUI text;

    void Start()
    {
        manager = GameManager.Instance; 
        GameObject textObject = GameObject.Find("BloodCounter");
        if (textObject != null)
        {
            text = textObject.GetComponent<TextMeshProUGUI>();
        }
        else
        {
            ////Debug.LogError("GameObject 'BloodCounter' non trouvé");
        }
    }

    void Update()
    {
        text.text = manager.blood.ToString();
    }

}
