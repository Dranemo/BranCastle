using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GameManager : MonoBehaviour
{
    public float blood /*{ get; private set; }*/ = 500;
    public static GameManager Instance { get; private set; }
    public bool isPlayerInLight = false;

    [SerializeField] private GameObject CanvaPrefab;
    [SerializeField] private GameObject MapPrefab;
    [SerializeField] private List<Path> paths;
    [SerializeField] private List<Path> spawningPaths;
    [SerializeField] private GameObject playerPrefab;

    [SerializeField] private GameObject ritual;
    [SerializeField] private GameObject demonRitual;
    [SerializeField] private GameObject fireAnim;


    [Header("Enemy & Wave")]
 private Dictionary<Path, List<int>> recentEnemiesByPath = new Dictionary<Path, List<int>>();
    [SerializeField] private Enemy[] enemies;
    [SerializeField] private GameObject King;
    GameObject enemyFolder;

    [SerializeField] float enemyCooldown = 10;
    [SerializeField] float enemyCooldownLeft = 0;
    [SerializeField] float enemyWaveCooldown = 30;
    [SerializeField] float enemyWaveCooldownLeft = 30;
    [SerializeField] float enemyCooldownDecrease = 0.2f;
    private int enemyCount = 0;
    public float wave { get; private set; }  = 1;
    int wavePathIndex  = 0;

    [SerializeField] public float time /*{ get; private set; }*/ = 1170; // 11h30

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
    public bool kingKilled = false;

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
            //////////Debug.LogError("Pas assez de composants AudioSource attachés au GameObject.");
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


        // Ritual
        ritual = MapPrefab.transform.Find("Center").Find("Ritual").gameObject;



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


    bool initialGong = false;

    private void Update()
    {
        if (!isGameOver)
        {
            //GameOver();


            if (time >= 1200) // 20h
            {
                if (!initialGong)
                {
                    initialGong = true;
                    audioSourceGong.Play();
                }

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


                    if (wave == 4)
                        AddPaths(true);

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
        if(blood > 1000)
        {
            blood = 1000;
        }
        audioSourceBlood.Play();
    }
    public void BloodCost(float amount)
    {
        blood -= amount;
        if(player != null)
            GameOver();
    }
    public void TakeDamage(float damage)
    {
        //Debug.Log("TookDamage");
        //Debug.Break();

        if (shake == null)
        {
            //////////Debug.LogError("shake est null !");
            return;
        }

        if (!isInvincible && player != null)
        {


            //Debug.Log("tookDamage");

            shake.StartShake();
            blood -= damage;
            //////////Debug.Log("Nouveau niveau de sang: " + blood);
            StartCoroutine(InvincibilityCoroutine());
            
            GameOver();
        }
        else
        {
            //////////Debug.Log("Le joueur est invincible !");
            return;
        }
    }
    public void RitualTakeDamage(float damage)
    {
        //Debug.Log("TookDamage");
        //Debug.Break();

        if (shake == null)
        {
            //////////Debug.LogError("shake est null !");
            return;
        }

        if (player != null)
        {


            //Debug.Log("tookDamage");

            shake.StartShake();
            blood -= damage;
            //////////Debug.Log("Nouveau niveau de sang: " + blood);

            GameOver();
        }
        else
        {
            //////////Debug.Log("Le joueur est invincible !");
            return;
        }
    }





    private IEnumerator InvincibilityCoroutine()
    {
        //Debug.Log("Invincible");

        isInvincible = true;
        float elapsedTime = 0f;

        while (elapsedTime < invincibilityDuration)
        {
            if (player != null)
            {
                SpriteRenderer spriteRenderer = player.GetComponent<SpriteRenderer>(); 
                Color color = spriteRenderer.color;
                    color.a = 0.5f; 
                    spriteRenderer.color = color;

                elapsedTime += Time.deltaTime;
                yield return null; 
            }
        }

        if(player != null)
        {
            SpriteRenderer finalSpriteRenderer = player.GetComponent<SpriteRenderer>();
            if (finalSpriteRenderer != null)
            {
                Color finalColor = finalSpriteRenderer.color;
                finalColor.a = 1f;
                finalSpriteRenderer.color = finalColor;
            }
        }

        isInvincible = false;
    }





    // -------------------------------------------------------------- Enemy Func --------------------------------------------------------------

    private void SpawnEnemy()
    {
        List<Path> spawnPaths = GetSpawnPathsForWave((int)wave);

        foreach (Path path in spawnPaths)
        {
            enemyCount++;

            Vector3 spawnPosition = path.waypoints[0].gameObject.transform.position;

            int enemyIndex = GetEnemyIndexForWave((int)wave, path);

            GameObject enemyObj = Instantiate(enemies[enemyIndex].gameObject, spawnPosition, Quaternion.identity);

            if (enemyIndex == enemies.Length - 1)
            {
                King = enemyObj;
            }

            Enemy enemy = enemyObj.GetComponent<Enemy>();

            enemyObj.transform.localScale = new Vector3(2f, 2f, 0f);

            ////////////Debug.Log("Spawn Enemy : " + enemy.name + " at " + path.name);
            enemy.currentPathIndex = paths.IndexOf(path);
            enemy.paths = this.paths;
            enemy.name = enemyObj.name + " " + enemyCount;

            enemy.transform.SetParent(enemyFolder.transform);

            if (!recentEnemiesByPath.ContainsKey(path))
            {
                recentEnemiesByPath[path] = new List<int>();
            }

            recentEnemiesByPath[path].Add(enemyIndex);
            if (recentEnemiesByPath[path].Count > 2)
            {
                recentEnemiesByPath[path].RemoveAt(0);
            }

            // Debug logs for each value in the dictionary
            foreach (var kvp in recentEnemiesByPath)
            {
                //////Debug.Log($"Path: {kvp.Key.name}, Enemies: {string.Join(", ", kvp.Value)}");
            }
        }
    }


    private int GetEnemyIndexForWave(int wave, Path path)
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
            int enemyIndex;

            do
            {
                enemyIndex = Random.Range(0, maxEnemyIndex + 1);
            } while (recentEnemiesByPath.ContainsKey(path) && recentEnemiesByPath[path].Contains(enemyIndex));

            return enemyIndex;
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
    private void AddPaths(bool secret = false)
    {
        paths.Clear();
        spawningPaths.Clear();

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

                if (secret && child.CompareTag("SecretPath"))
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

        ////////////Debug.Log("Paths : " + paths.Count);
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

                    ////////////Debug.Log(path.name + " : " + path.distancePath + "m");
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
        //Debug.Log("StartGameOver");

        ////Debug.Log("GameOver() called");

        if (blood <= 0 && audioSourceGameOver != null)
        {
            //Debug.Log("StartSon");
            ////Debug.Log("Blood is zero or less, playing game over audio");
            audioSourceGameOver.Play();
            audioSourceMusic.Stop();
        }

        //Debug.Log(blood + " : blood");
        if (blood <= 0)
        {
            //Debug.Log("blood < 0");

            ////Debug.Log("Blood is zero or less, setting isGameOver to true");
            isGameOver = true;
            PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
            ////Debug.Log("PlayerMovement component: " + playerMovement);

            if (playerMovement != null && !coroutineStartedDeath)
            {
                StopAllCoroutines();

                ////Debug.Log("Starting death animation coroutine");
                coroutineStartedDeath = true;
                Destroy(player);
                ScenesManager.Instance.LoadScene("GameOver");
            }
        }
        else if (wave >= 9)
        {
            ////Debug.Log("Wave is 9 or more, stopping music");
            audioSourceMusic.Stop();

            if (King != null && King.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Death"))
            {
                ////Debug.Log("King is dead, setting isGameOver to true");
                isGameOver = true;
                enemyCooldown = 100000;
                enemyWaveCooldown = 100000;

                // Disable player
                //////Debug.Log("Disabling player components");
                player.GetComponent<PlayerMovement>().enabled = false;
                player.GetComponent<PlayerAttack>().enabled = false;

                // Disable camera
                //////Debug.Log("Disabling camera follow");
                player.transform.Find("Main Camera").GetComponent<CameraFollow>().enabled = false;
                player.transform.Find("Main Camera").position = new Vector3(King.transform.position.x, King.transform.position.y, -10);

                foreach (Transform child in GameObject.Find("EnemyFolder").transform)
                {
                    if (child != King)
                    {
                        //////Debug.Log("Muting and damaging enemy: " + child.name);
                        child.GetComponent<AudioSource>().mute = true;
                        child.GetComponent<Enemy>().TakeDamage(1000, false);
                    }
                }

                GameObject[] units = GameObject.FindGameObjectsWithTag("Unit");
                for (int i = 0; i < units.Length; i++)
                {
                    ////Debug.Log("Muting and damaging unit: " + units[i].name);
                    units[i].GetComponent<AudioSource>().mute = true;
                    units[i].GetComponent<Unit>().TakeDamage(1000);
                }

                ////Debug.Log("Starting WaitForKingDeath coroutine");
                StartCoroutine(WaitForKingDeath());

                //ScenesManager.Instance.LoadScene("GameOver");
            }
        }
    }


    IEnumerator WaitForKingDeath()
    {
        while (King != null)
            yield return null;


        StartCoroutine(GameOverRitualAnim());
    }

    IEnumerator GameOverRitualAnim()
    {
        kingKilled = true;
        player.transform.Find("Main Camera").position = new Vector3(ritual.transform.position.x, ritual.transform.position.y, -10);
        yield return new WaitForSeconds(1);

        ritual.GetComponent<Animator>().SetBool("theEnd", true);
        yield return new WaitForSeconds(ritual.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);

        GameObject invoc = GameObject.Instantiate(fireAnim, ritual.transform.position, Quaternion.identity);
        invoc.transform.position = new Vector3(invoc.transform.position.x, invoc.transform.position.y, 1);
        yield return new WaitForSeconds(invoc.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);

        GameObject demon = GameObject.Instantiate(demonRitual, ritual.transform.position, Quaternion.identity);
        demon.transform.position = new Vector3(demon.transform.position.x, demon.transform.position.y + 2, 2);
        yield return new WaitForSeconds(demon.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length * 3);


        ScenesManager.Instance.LoadScene("GameOver");

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
