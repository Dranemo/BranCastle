using UnityEngine.UI;
using UnityEngine;
using TMPro;
public class UnitSpawn : MonoBehaviour
{
    [SerializeField] GameObject CanvaPrefab;
    [SerializeField] GameObject[] unitPrefabs;
    [SerializeField] int[] unitCosts;
    [SerializeField] float spawnDistance = 1f;
    [SerializeField] GameObject[] panels;

    private GameObject player;
    private GameManager gameManager;
    private int selectedIndex = 0;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        gameManager = GameManager.Instance;

        Transform panelsTransform = CanvaPrefab.transform.Find("Panels");
        int childCount = panelsTransform.childCount;
        panels = new GameObject[childCount];
        for (int i = 0; i < childCount; i++)
        {
            panels[i] = panelsTransform.GetChild(i).gameObject;

            if (panels[i].transform.childCount > 2)
            {
                GameObject child2 = panels[i].transform.GetChild(2).gameObject;
                TextMeshProUGUI textComponent = child2.GetComponent<TextMeshProUGUI>();
                if (textComponent != null)
                {
                    textComponent.text = "Cost: " + unitCosts[i].ToString();
                }
                else
                {
                    Text textComponentLegacy = child2.GetComponent<Text>();
                    if (textComponentLegacy != null)
                    {
                        textComponentLegacy.text = "Cost: " + unitCosts[i].ToString();
                    }
                }
            }
        }
    }
    private void SetPanelChildrenActive(GameObject panel, bool isActive)
    {
        if (panel.transform.childCount > 2)
        {
            panel.transform.GetChild(1).gameObject.SetActive(isActive);
            panel.transform.GetChild(2).gameObject.SetActive(isActive);
        }
    }

    void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            if (scroll > 0) selectedIndex--;
            else selectedIndex++;

            if (selectedIndex < 0) selectedIndex = unitPrefabs.Length - 1;
            if (selectedIndex >= unitPrefabs.Length) selectedIndex = 0;
        }

        for (int i = 0; i < panels.Length; i++)
        {
            GameObject panel = panels[i];
            Outline outline = panel.GetComponent<Outline>();
            if (outline != null)
            {
                outline.effectColor = (i == selectedIndex) ? Color.red : Color.black;
            }

            // Activer ou désactiver les enfants 1 et 2
            SetPanelChildrenActive(panel, i == selectedIndex);
        }

        if (Input.GetButtonDown("UnitSpawn"))
        {
            if (gameManager.blood >= unitCosts[selectedIndex])
            {
                Vector3 spawnPosition = player.transform.position + transform.forward * spawnDistance;
                GameObject unit = Instantiate(unitPrefabs[selectedIndex], spawnPosition, Quaternion.identity);

                gameManager.BloodCost(unitCosts[selectedIndex]);
            }
        }
    }

}
