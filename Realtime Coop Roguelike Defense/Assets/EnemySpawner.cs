using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Redcode.Pools;
using System.Threading.Tasks;

public class EnemySpawner : MonoBehaviour
{
    [Header("Pool Settings")]
    [SerializeField]
    private bool _resetAllPoolOnStart;

    [Header("Spawner Settings")]
    [SerializeField] private bool waveStartWhenAllEnemiesDead = true;
    [SerializeField] private float timeBetweenWaves;
    [SerializeField] private Wave[] waves;

    private IPool<Enemy> enemyPool;

    [System.Serializable]
    struct Wave
    {
        public EnemyInfo[] enemies;
        public float spawnTimeBetweenEnemies;
        public SpawnMethod spawnMethod;
    }

    [System.Serializable]
    struct EnemyInfo
    {
        public string enemyName;
        public int enemyNumber;
        public SpawnPoint spawnPoint;
    }

    [System.Serializable]
    public enum SpawnPoint
    {
        SpawnPoint1,
        SpawnPoint2,
        SpawnPoint3
    };

    public enum SpawnMethod
    {
        Sequentially,
        Syncrhonouslly
    };

    [SerializeField] private Transform[] spawnPositions;

    private bool waveFinished => waveEnemyNumber == 0;
    private int waveEnemyNumber;

    private int waveIndex = 0;

    PoolManager poolManager;
    private void Awake()
    {
        poolManager = GetComponent<PoolManager>();
        enemyPool = poolManager.GetPool<Enemy>("Square");
        enemyPool.Clear();

    }


    private void Start()
    {
        waveIndex = 0;
        
        //enemyPool.Get();
        //for (int i = 0; i < 5; i++)
        //    Spawn("Square");
        //StartSpawnEnemies(waveIndex);
    }

    private void Update()
    {
        Debug.Log(waveFinished);
        if (waveFinished)
            StartSpawnEnemies(waveIndex);
    }

    private async void StartSpawnEnemies(int index)
    {
        
        await Task.Delay((int)(timeBetweenWaves * 1000));
        // spawn enemies
        if (waveStartWhenAllEnemiesDead)
        {
            //await new WaitUntil(() => waveFinished == true);
            if (waveFinished)
                await SpawnWave(waves[waveIndex]);
        }

        
    }

    private async Task SpawnWave(Wave wave)
    {
        Debug.Log($"Wave {waveIndex} started");
        var enemies = wave.enemies;
        int spawnTime = (int)(wave.spawnTimeBetweenEnemies*1000);
        waveEnemyNumber = GetTotalEnemyNumberOf(wave);
        waveIndex++;

        switch(wave.spawnMethod)
        {
            case SpawnMethod.Sequentially:
                for (int i = 0; i < enemies.Length; i++)
                {
                    for (int k = 0; k < enemies[i].enemyNumber; k++)
                    {
                        await Task.Delay(spawnTime);
                        Spawn(enemies[i].enemyName, enemies[i].spawnPoint);
                    }
                }
                break;
            case SpawnMethod.Syncrhonouslly:
                foreach(EnemyInfo enemInfo in enemies)
                {
                    Spawn(enemInfo.enemyName, enemInfo.spawnPoint, wave.spawnTimeBetweenEnemies, enemInfo.enemyNumber);
                }
                break;
        }

    }

    
    // Spawns each enemeis
    public void Spawn(string name, SpawnPoint spawnPoint)
    {
        Health health = poolManager.GetFromPool<Enemy>(name).gameObject.GetComponent<Health>();

        health.OnDeath += ReturnPool;

        health.transform.position = spawnPositions[(int)spawnPoint].position;
    }

    public async void Spawn(string name, SpawnPoint spawnPoint, float spawnTime, int enemyNumber)
    {
        for (int i = 0; i < enemyNumber; i++)
        {
            await Task.Delay((int)(1000 * spawnTime));
            Spawn(name, spawnPoint);
        }

    }

    private int GetTotalEnemyNumberOf(Wave wave)
    {
        int num = 0;
        for (int i = 0; i < wave.enemies.Length; i++)
        {
            num += wave.enemies[i].enemyNumber;
        }
        return num;
    }

    private void OnEnemyDeath()
    {
        waveEnemyNumber--;
        
    }
    public void ReturnPool(GameObject go)
    {
        Debug.Log("Returned to pool");
        OnEnemyDeath();
        Enemy enemy = go.GetComponent<Enemy>();
        poolManager.TakeToPool<Enemy>(enemy.Name, enemy);

        go.GetComponent<Health>().OnDeath -= ReturnPool;
        
    }
} 
