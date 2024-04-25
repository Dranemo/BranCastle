using UnityEngine;

public class TowerSpot : MonoBehaviour
{
    public GameObject towerPrefab;
    public int towerCost = 2000;
    private Camera mainCamera;
    GameManager manager;
    void Start()
    {
        manager = GameManager.Instance;
        mainCamera = Camera.main;
    }

    void OnMouseDown()
    {
        if (manager.GetBlood() >= towerCost)
        {
            manager.AddBlood(-towerCost);
            Instantiate(towerPrefab, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }
}

