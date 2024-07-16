using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BloodCounter : MonoBehaviour
{
    GameManager manager;
    private TextMeshProUGUI text;

    void Start()
    {
        manager = GameManager.Instance;
        text = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        text.text = "Blood: " + manager.blood;
    }
}
