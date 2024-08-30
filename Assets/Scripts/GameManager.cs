using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GameManager : MonoBehaviour
{
    public float blood /*{ get; private set; }*/ = 1000;
    public static GameManager Instance { get; private set; }
    public bool isPlayerInLight = false;

    [SerializeField] private GameObject CanvaPrefab;
    [SerializeField] private GameObject MapPrefab;
    [SerializeField] private List<Path> paths;
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
    bool coroutineStartedDeath = false;

    private AudioSource audioSourceGong;
    private AudioSource audioSourceGameOver;
    private AudioSource audioSourceBlood;
    private AudioSource audioSourceKing;
    private AudioSource audioSourceMusic;
    private GameObject player;
    [SerializeField] private AudioClip audioGong;
    [SerializeField] private AudioClip audioGameOver;
    [SerializeField] private AudioClip bloodPickup;
    [SerializeField] private AudioClip audioKing;
    [SerializeField] private AudioClip audioMusic;

    [SerializeField] private ScreenShake shake;
    private bool isInvincible = false;
    [SerializeField] private float invincibilityDuration = 0.5f;
    private bool kingSpawned = false;

    // -------------------------------------------------------------- Unity Func -------------------------------------------------------------- 
    private void Awake()
    {
        // Singleton
        CreateSingleton();
        AudioSource[] audioSources = GetComponents<AudioSource>();
        if (audioSources.Length >= 3)
        {
            audioSourceGameOver = audioSources[0];
            audioSourceGong = audioSources[1];
            audioSourceBlood = audioSources[2];
            audioSourceKing = audioSources[3];
            audioSourceMusic = audioSources[4];
        }
        else
        {
            //Debug.LogError("Pas assez de composants AudioSource attachés au GameObject.");
        }
        audioSourceBlood.clip = bloodPickup;
        audioSourceGameOver.clip = audioGameOver;
        audioSourceGong.clip = audioGong;
        audioSourceKing.clip = audioKing;
        audioSourceMusic.clip = audioMusic;
        paths = new List<Path>();
        spawningPaths = new List<Path>();

        enemyFolder = new GameObject("EnemyFolder");



        // Paths
        AddPaths();
        SetDistancePaths();




        // Player
        Transform playerSpawnPoint = MapPrefab.transform.Find("PlayerSpawnPoint");
        player = Instantiate(playerPrefab, playerSpawnPoint.position, Quaternion.identity);


        // Canva
        GameObject canva = Instantiate(CanvaPrefab);
        canva.GetComponent<Canvas>().worldCamera = player.transform.Find("Main Camera").GetComponent<Camera>();

        wavePathIndex = Random.Range(0, spawningPaths.Count);

        //Screenshake
        shake = player.transform.Find("Main Camera").GetComponent<ScreenShake>();
    }

    void Start()
    {
        audioSourceMusic.Play();
    }


    private void Update()
    {
        if (!isGameOver)
        {
            GameOver();

            if(time >= 720) // 12h
            {
              audioSourceGong.Play();
                if (enemyCooldownLeft <= 0)
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
                    audioSourceGong.Play();
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
        if(blood > 5000)
        {
            blood = 5000;
        }
        audioSourceBlood.Play();
    }
    public void BloodCost(float amount)
    {
        blood -= amount;
    }
    public void TakeDamage(float damage, bool playerIsDamaged = false)
    {

        if (shake == null)
        {
            //Debug.LogError("shake est null !");
            return;
        }

        if (!isInvincible)
        {
            shake.StartShake();
            blood -= damage;
            //Debug.Log("Nouveau niveau de sang: " + blood);
            GameOver();
            StartCoroutine(InvincibilityCoroutine());
        }
        else
        {
            //Debug.Log("Le joueur est invincible !");
            return;
        }
    }

    private IEnumerator InvincibilityCoroutine()
    {
        isInvincible = true;
        float elapsedTime = 0f;

        while (elapsedTime < invincibilityDuration)
        {
            if (player)
            {
            SpriteRenderer spriteRenderer = player.GetComponent<SpriteRenderer>(); 
            Color color = spriteRenderer.color;
                color.a = 0.5f; 
                spriteRenderer.color = color;

            elapsedTime += Time.deltaTime;
            yield return null; 
            }
        }

        SpriteRenderer finalSpriteRenderer = player.GetComponent<SpriteRenderer>();
        if (finalSpriteRenderer != null)
        {
            Color finalColor = finalSpriteRenderer.color;
            finalColor.a = 1f;
            finalSpriteRenderer.color = finalColor;
        }

        isInvincible = false;
    }





    // -------------------------------------------------------------- Enemy Func --------------------------------------------------------------
    private void SpawnEnemy()
    {
        List<Path> spawnPaths = GetSpawnPathsForWave((int)wave);

        foreach (Path path in spawnPaths)
        {
            Vector3 spawnPosition = path.waypoints[0].gameObject.transform.position;

            int enemyIndex = GetEnemyIndexForWave((int)wave);

            GameObject enemyObj = Instantiate(enemies[enemyIndex].gameObject, spawnPosition, Quaternion.identity);
            Enemy enemy = enemyObj.GetComponent<Enemy>();

            enemyObj.transform.localScale = new Vector3(2f, 2f, 0f);

            ////Debug.Log("Spawn Enemy : " + enemy.name + " at " + path.name);
            enemy.currentPathIndex = paths.IndexOf(path);
            enemy.paths = this.paths;

            enemy.transform.SetParent(enemyFolder.transform);
        }
    }

    private int GetEnemyIndexForWave(int wave)
    {
        if (wave == 9 && !kingSpawned)
        {
            StartCoroutine(PlayKingAudioAndPauseMusic());
            kingSpawned = true;
            return enemies.Length - 1;
        }
        else
        {
            int maxEnemyIndex = Mathf.Min(3 + (wave - 1) / 2, enemies.Length - 2);
            return Random.Range(0, maxEnemyIndex + 1);
        }
    }

    private IEnumerator PlayKingAudioAndPauseMusic()
    {
        audioSourceMusic.Pause();
        audioSourceKing.Play();
        yield return new WaitForSeconds(audioSourceKing.clip.length);
        audioSourceMusic.UnPause();
    }


    private List<Path> GetSpawnPathsForWave(int wave)
    {
        List<Path> selectedPaths = new List<Path>();
        int numberOfPaths = 0;

        if (wave >= 8)
        {
            numberOfPaths = 4;
        }
        else if (wave >= 5)
        {
            numberOfPaths = 3;
        }
        else
        {
            numberOfPaths = 2;
        }

        for (int i = 0; i < numberOfPaths; i++)
        {
            int pathIndex = (wavePathIndex + i) % spawningPaths.Count;
            selectedPaths.Add(spawningPaths[pathIndex]);
        }

        return selectedPaths;
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

        ////Debug.Log("Paths : " + paths.Count);
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

                    ////Debug.Log(path.name + " : " + path.distancePath + "m");
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
        if (blood <= 0 && audioSourceGameOver != null)
        {
            audioSourceGameOver.Play();
        }
        if (blood <= 0)
        {
            isGameOver = true;
            //Debug.Log("Game Over: Loading GameOver Scene");
            PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
            //Debug.Log(playerMovement);
            if (playerMovement != null && !coroutineStartedDeath)
            {
                coroutineStartedDeath = true;
                StartCoroutine(playerMovement.WaitForDeathAnimation());
            }
        }
        else if (wave >= 10)
        {
            isGameOver = true;
            //Debug.Log("Victory: Loading Victory Scene");
            ScenesManager.Instance.LoadScene("GameOver");
        }
    }

    public void RestartGame()
    {
        ScenesManager.Instance.LoadScene("Level-1"); 
        Destroy(gameObject);
        Instance = null;
    }

    public void Reset()
    {
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
