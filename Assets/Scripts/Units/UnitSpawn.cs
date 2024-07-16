using UnityEngine.UI;
using UnityEngine;

public class UnitSpawn : MonoBehaviour
{
    [SerializeField] GameObject CanvaPrefab;
    public GameObject[] unitPrefabs;
    public int[] unitCosts;
    GameObject[] panels;
    public float spawnDistance = 1f;


    private GameManager gameManager;
    private int selectedIndex = 0;

    private void Start()
    {
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
                Vector3 spawnPosition = transform.position + transform.forward * spawnDistance;
                GameObject unit = Instantiate(unitPrefabs[selectedIndex], spawnPosition, Quaternion.identity);

                gameManager.TakeDamage(unitCosts[selectedIndex]);
            }
        }
    }
}
