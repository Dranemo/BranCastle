using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public float blood = 10000;
    public static GameManager Instance { get; private set; }
    public bool isPlayerInLight = false;

    [SerializeField] private GameObject MapPrefab;
    private Path[] paths;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Enemy[] enemies;

    GameObject enemyFolder;

    [SerializeField] float enemyCooldown = 10;
    [SerializeField] float enemyCooldownLeft = 0;
    [SerializeField] float enemyWaveCooldown = 30;
    [SerializeField] float enemyWaveCooldownLeft = 30;
    public float wave { get; private set; }  = 1;
    int wavePathIndex  = 0;

    [SerializeField] public float time /*{ get; private set; }*/ = 690; // 11h30

    public bool isGameOver = false;



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

        GameObject pathsGO = MapPrefab.transform.Find("Paths").gameObject;
        paths = pathsGO.GetComponentsInChildren<Path>();

        Transform playerSpawnPoint = MapPrefab.transform.Find("PlayerSpawnPoint");
        GameObject player = Instantiate(playerPrefab, playerSpawnPoint.position, Quaternion.identity);

        wavePathIndex = Random.Range(0, paths.Length);

        enemyCooldownLeft += 30;
        enemyWaveCooldownLeft += 30;
    }

    private void Update()
    {
        if (!isGameOver)
        {
            GameOver();
            if(enemyCooldownLeft <= 0)
            {
                enemyCooldownLeft = enemyCooldown;
                SpawnEnemy();
            }
            enemyCooldownLeft -= Time.deltaTime;

            if (enemyWaveCooldownLeft <= 0)
            {
                enemyWaveCooldownLeft = enemyWaveCooldown;
                enemyCooldownLeft = 0;
                wave++;
                enemyCooldown -= 0.5f;
                wavePathIndex = Random.Range(0, paths.Length);
            }
            enemyWaveCooldownLeft -= Time.deltaTime;

            time += Time.deltaTime;
        }
        
    }
    public void AddBlood(float amount)
    {
        blood += amount;
    }
    public void TakeDamage(float damage)
    {
        blood -= damage;
        GameOver();
    }



    private void SpawnEnemy()
    {
        Path path = paths[wavePathIndex];
        Vector3 spawnPosition = path.waypoints[0].position;


        int enemyIndex = Random.Range(0, enemies.Length);

        GameObject enemyObj = Instantiate(enemies[enemyIndex].gameObject, spawnPosition, Quaternion.identity);
        Enemy enemy = enemyObj.GetComponent<Enemy>();

        enemyObj.transform.localScale = new Vector3(2f, 2f, 0f);
        enemy.SetPath(path);

        enemy.transform.SetParent(enemyFolder.transform);
        
    }


    private void GameOver()
    {
        if (blood <= 0 || time > 1439)
        {
            isGameOver = true;
            SceneManager.LoadScene("GameOver");
        }
    }


    public void RestartGame()
    {
        SceneManager.LoadScene("Level-1"); 
        Destroy(gameObject);
        Instance = null;
    }

}
