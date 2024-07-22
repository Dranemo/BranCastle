using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public float blood /*{ get; private set; }*/ = 10000;
    public static GameManager Instance { get; private set; }
    public bool isPlayerInLight = false;

    [SerializeField] private GameObject MapPrefab;
    private List<Path> paths;
    private List<Path> spawningPaths;
    [SerializeField] private GameObject playerPrefab;


    [Header("Enemy & Wave")]
    [SerializeField] private Enemy[] enemies;
    GameObject enemyFolder;

    [SerializeField] float enemyCooldown = 10;
    [SerializeField] float enemyCooldownLeft = 0;
    [SerializeField] float enemyWaveCooldown = 30;
    [SerializeField] float enemyWaveCooldownLeft = 30;
    [SerializeField] float enemyCooldownDecrease = 0.2f;
    public float wave { get; private set; }  = 1;
    int wavePathIndex  = 0;

    [SerializeField] public float time /*{ get; private set; }*/ = 690; // 11h30

    public bool isGameOver = false;


    // -------------------------------------------------------------- Unity Func -------------------------------------------------------------- 
    private void Awake()
    {
        // Singleton
        CreateSingleton();


        paths = new List<Path>();
        spawningPaths = new List<Path>();

        enemyFolder = new GameObject("EnemyFolder");



        // Paths
        AddPaths();
        SetDistancePaths();

       


        // Player
        Transform playerSpawnPoint = MapPrefab.transform.Find("PlayerSpawnPoint");
        GameObject player = Instantiate(playerPrefab, playerSpawnPoint.position, Quaternion.identity);

        wavePathIndex = Random.Range(0, spawningPaths.Count);
    }




    private void Update()
    {
        if (!isGameOver)
        {
            GameOver();

            if(time >= 720) // 12h
            {
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
                    enemyCooldown -= enemyCooldownDecrease;
                    wavePathIndex = Random.Range(0, spawningPaths.Count);
                }
                enemyWaveCooldownLeft -= Time.deltaTime;
            }

            time += Time.deltaTime;
        }
        
    }

    // -------------------------------------------------------------- Blood Func --------------------------------------------------------------
    public void AddBlood(float amount)
    {
        blood += amount;
    }
    public void TakeDamage(float damage)
    {
        blood -= damage;
        GameOver();
    }


    // -------------------------------------------------------------- Enemy Func --------------------------------------------------------------
    private void SpawnEnemy()
    {
        Path path = spawningPaths[wavePathIndex];
        Vector3 spawnPosition = path.waypoints[0].gameObject.transform.position;


        int enemyIndex = Random.Range(0, enemies.Length);

        GameObject enemyObj = Instantiate(enemies[enemyIndex].gameObject, spawnPosition, Quaternion.identity);
        Enemy enemy = enemyObj.GetComponent<Enemy>();

        enemyObj.transform.localScale = new Vector3(2f, 2f, 0f);



        Debug.Log("Spawn Enemy : " + enemy.name + " at " + path.name);
        Debug.Log(paths.Count);
        enemy.currentPathIndex = paths.IndexOf(path);
        enemy.paths = this.paths;

        enemy.transform.SetParent(enemyFolder.transform);
        
    }


    // -------------------------------------------------------------- Path Func --------------------------------------------------------------
    private void AddPaths()
    {
        GameObject pathsGO = MapPrefab.transform.Find("Paths").gameObject;
        foreach (Transform child in pathsGO.transform)
        {
            if (child.GetComponent<Path>() != null)
            {
                paths.Add(child.GetComponent<Path>());
                child.GetComponent<Path>().distancePath = 0;

                if(child.CompareTag("SpawningPath"))
                {
                    spawningPaths.Add(child.GetComponent<Path>());
                }
            }
        }
    }

    private void SetDistancePaths()
    {
        List<Path> pathsToLoad = new List<Path>();
        List<Path> pathsLoaded = new List<Path>();

        Debug.Log("Paths : " + paths.Count);
        foreach (Path path in paths) 
            pathsToLoad.Add(path);


        while (pathsToLoad.Count > 0)
        {
            foreach (Path path in pathsToLoad)
            {
                if (path.nextPath == null || pathsLoaded.Contains(path.nextPath))
                {
                    paths[paths.IndexOf(path)].SetDistancePath();
                    pathsLoaded.Add(path);

                    Debug.Log(path.name + " : " + path.distancePath + "m");
                }
            }

            foreach (Path path in pathsLoaded)
            {
                if (pathsToLoad.Contains(path))
                {
                    pathsToLoad.Remove(path);
                }
            }
        }
    }


    // -------------------------------------------------------------- Game Func --------------------------------------------------------------
    private void GameOver()
    {
        if (blood <= 0 || time > 1439) // 23h59
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




    // -------------------------------------------------------------- Getter Func --------------------------------------------------------------
    void CreateSingleton()
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
}
