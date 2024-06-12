using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public float blood = 10000;
    public float radiusTowerOne = 2;
    public static GameManager Instance { get; private set; }
    public bool isPlayerInLight = false;

    private List<Tower> towers = new List<Tower>();
    [System.Serializable]
    public class Wave
    {
        public GameObject enemyPrefab;
        public int count;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        /*if (blood <= 0)
        {
            SceneManager.LoadScene("GameOver"); 
        }*/
        
    }
    public void AddBlood(int amount)
    {
        float deltaTime = Time.deltaTime;
        blood += amount*deltaTime;
    }
    public float GetBlood()
    {
        return blood;
    }
    public void AddTower(Tower tower)
    {
        towers.Add(tower);
    }

    public void RemoveTower(Tower tower)
    {
        towers.Remove(tower);
    }

    public Tower GetTower(int index)
    {
        if (index >= 0 && index < towers.Count)
        {
            return towers[index];
        }
        else
        {
            return null;
        }
    }
}
