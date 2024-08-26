using UnityEngine.UI;
using UnityEngine;

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

        // Mettez toutes les bordures en noir
        foreach (GameObject panel in panels)
        {
            panel.GetComponent<Outline>().effectColor = Color.black;
        }

        // Mettez la bordure de l'unité sélectionnée en blanc
        panels[selectedIndex].GetComponent<Outline>().effectColor = Color.white;

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
