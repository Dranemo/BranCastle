using UnityEngine.UI;
using UnityEngine;

public class UnitSpawn : MonoBehaviour
{
    public GameObject[] unitPrefabs;
    public int[] unitCosts;
    public float spawnDistance = 1f;
    public GameObject[] squares;

    private GameManager gameManager;
    private int selectedIndex = 0;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
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
        foreach (GameObject square in squares)
        {
            square.GetComponent<Outline>().effectColor = Color.black;
        }

        // Mettez la bordure de l'unité sélectionnée en blanc
        squares[selectedIndex].GetComponent<Outline>().effectColor = Color.white;

        if (Input.GetButtonDown("UnitSpawn"))
        {
            if (gameManager.blood >= unitCosts[selectedIndex])
            {
                Vector3 spawnPosition = transform.position + transform.forward * spawnDistance;
                GameObject unit = Instantiate(unitPrefabs[selectedIndex], spawnPosition, Quaternion.identity);

                gameManager.blood -= unitCosts[selectedIndex];
            }
        }
    }
}
