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


    [SerializeField] private Transform spawnpoint;
    [SerializeField] private Enemy[] enemies;
    [SerializeField] private Path[] paths;
    GameObject enemyFolder;

    [SerializeField] float enemyCooldown = 10;
    [SerializeField] float enemyCooldownLeft = 0;

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

        enemyFolder = new GameObject("EnemyFolder");
    }

    private void Update()
    {
        GameOver();
        if(enemyCooldownLeft <= 0)
        {
            enemyCooldownLeft = enemyCooldown;
            SpawnEnemy();
        }
        enemyCooldownLeft -= Time.deltaTime;
        
    }
    public void AddBlood(float amount)
    {
        blood += amount;
    }
    public float GetBlood()
    {
        return blood;
    }


    public void TakeDamage(float damage)
    {
        blood -= damage;
        GameOver();
    }



    private void SpawnEnemy()
    {
        int pathIndex = Random.Range(0, paths.Length);
        Path path = paths[pathIndex];

        Vector3 spawnPosition = spawnpoint.position;

        int enemyIndex = Random.Range(0, enemies.Length);

        GameObject enemyObj = Instantiate(enemies[enemyIndex].gameObject, spawnPosition, Quaternion.identity);
        Enemy enemy = enemyObj.GetComponent<Enemy>();

        enemyObj.transform.localScale = new Vector3(2f, 2f, 0f);
        enemy.SetPath(path);

        enemy.transform.SetParent(enemyFolder.transform);
        
    }


    private void GameOver()
    {
        if (blood <= 0)
        {
            AddBlood(10000);
            SceneManager.LoadScene("GameOver");
        }
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
