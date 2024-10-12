using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnemyWaveManager : MonoBehaviour
{
    [SerializeField] private List<Transform> listPointSpawn;
    [SerializeField] private GameObject pfEnemy;
    [SerializeField] private Player player;
    public enum State
    {
        WaitingToSpawnWave,
        SpawningWave,
    }
    public static EnemyWaveManager instance;
    [SerializeField] private bool playOne;
    public State state;
    private Transform spawnPositionTransform;
    [SerializeField] private int numberOfEnemiesInWave;
    [SerializeField] private float timeToWaitSpawnNextEnemy;
    [SerializeField] private float timeEnemyStartMoving;
    [SerializeField] private int numberEnemyWaveIncreases;
    [SerializeField] private float timeMaxEnemyStartMoving;
    [SerializeField] private float timeToSpawnEnenmy;
    [SerializeField] private float timeToWaitSpawnNextWavel;
    public int wave {  get; private set; }
    public float timeToSpawn { get; private set; }
    private ObjectPool<GameObject> enemyPool;
    public event EventHandler<int> OnNumberWaveChange;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        numberEnemyWaveIncreases = 2;
        timeEnemyStartMoving = 1.5f;
        timeToWaitSpawnNextWavel = 10;
        state = State.WaitingToSpawnWave;
        timeToSpawn = 3f;
        wave = 0;
        enemyPool = new(
            createFunc: () => Instantiate(pfEnemy, spawnPositionTransform.position + UtilClass.GetRamdomVector() * UnityEngine.Random.Range(0f, 10f), Quaternion.identity),
            actionOnGet: (enemy) => {
                enemy.SetActive(true);
                enemy.transform.position = spawnPositionTransform.position + UtilClass.GetRamdomVector() * UnityEngine.Random.Range(0f, 10f);
                enemy.transform.rotation = Quaternion.identity;
                },
            actionOnRelease: (enemy) => enemy.SetActive(false),
            actionOnDestroy: (enemy) => Destroy(enemy),
            collectionCheck: true,
            defaultCapacity: 5,
            maxSize: 10
            );
    }
    private void Start()
    {
        Enemy.OnAnyEnemyDie += Enemy_OnAnyEnemyDie;
    }

    private void Enemy_OnAnyEnemyDie(object sender, GameObject enemy)
    {
        enemyPool.Release(enemy);
    }

    private void Update()
    {
        switch (state)
        {
            case State.WaitingToSpawnWave:
                timeToSpawn -= Time.deltaTime;
                if (timeToSpawn <= 0)
                {
                    SpawnEnemy(listPointSpawn, 3f, 10);
                }
                break;
            case State.SpawningWave:
                timeToSpawnEnenmy -= Time.deltaTime;
                if (numberOfEnemiesInWave > 0)
                {
                    if (timeToSpawnEnenmy <= 0)
                    {
                        timeToSpawnEnenmy = timeToWaitSpawnNextEnemy;
                        EnemyMovement enemyMovement = enemyPool.Get().GetComponent<EnemyMovement>();
                        if (enemyMovement.player == null)
                        {
                            enemyMovement.player = player;
                        }
                        numberOfEnemiesInWave--;
                    }
                }
                else
                {
                    timeToSpawn = timeToWaitSpawnNextWavel;
                    timeEnemyStartMoving -= Time.deltaTime;
                    if (playOne)
                    {
                        //SoundManager.Instance.PlaySound(SoundManager.Sound.EnemyWaveStarting);
                        playOne = false;
                    }
                    if (timeEnemyStartMoving <= 0)
                    {
                        timeEnemyStartMoving = timeMaxEnemyStartMoving;
                        //OnFullEnemyWaveReady?.Invoke(this, EventArgs.Empty);
                        state = State.WaitingToSpawnWave;
                    }
                }
                break;
        }
    }
    private void SpawnEnemy(List<Transform> spawnPositionTransform, float timeTotalToSpawnAllEnemy, int numberOfEnemiesInWave)
    {
        this.spawnPositionTransform = spawnPositionTransform[UnityEngine.Random.Range(0, spawnPositionTransform.Count)];
        this.numberOfEnemiesInWave = numberOfEnemiesInWave + numberEnemyWaveIncreases * wave;
        this.timeToWaitSpawnNextEnemy = timeTotalToSpawnAllEnemy / numberOfEnemiesInWave;
        timeEnemyStartMoving = timeMaxEnemyStartMoving;
        wave++;
        playOne = true;
        state = State.SpawningWave;
        OnNumberWaveChange?.Invoke(this, wave);
    }
}
