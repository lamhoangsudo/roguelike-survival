using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UIElements;

public class EnemyWaveManager : MonoBehaviour
{
    [SerializeField] private List<Transform> listPointSpawns;
    [SerializeField] private GameObject pfEnemy;
    [SerializeField] private Player player;
    public enum State
    {
        PrepareWave,
        InWave,
        SpawningWave,
    }
    public static EnemyWaveManager instance;
    [SerializeField] private bool playOne;
    public State state;
    private Transform spawnPositionTransform;
    private HashSet<GameObject> activeEnemy;
    [SerializeField] private int numberMaxOfEnemiesInWave;
    [SerializeField] private int numberOfEnemiesInWave;
    [SerializeField] private int mutiplyNumberEnemyWaveIncreases;
    [SerializeField] private float timeMaxToWaitSpawnNextWavel;
    [SerializeField] private float timeMaxEnemyStartMoving;
    [SerializeField] private float timeEnemyStartMoving;
    [SerializeField] private float timeMaxToWaitSpawnNextEnemy;
    [SerializeField] private float timeToSpawnEnenmy;
    [SerializeField] private float timeMaxPrepareToSpawn;
    public int wave {  get; private set; }
    public float timeWave { get; private set; }
    public float timePrepareToSpawn { get; private set; }
    private ObjectPool<GameObject> enemyPool;
    public event EventHandler<int> OnNumberWaveChange;
    public event EventHandler<bool> OnPrepareToSpawn;
    public event EventHandler OnFullWaveEnemyReady;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        activeEnemy = new HashSet<GameObject>();
        numberOfEnemiesInWave = numberMaxOfEnemiesInWave;
        timeEnemyStartMoving = timeWave * 0.5f;
        state = State.PrepareWave;
        timeWave = timeMaxToWaitSpawnNextWavel * 0.1f;
        timePrepareToSpawn = timeMaxPrepareToSpawn;
        wave = 0;
        enemyPool = new(
            createFunc: () => Instantiate(pfEnemy, spawnPositionTransform.position + (UtilClass.GetRamdomVector() * UnityEngine.Random.Range(0f, 10f)), Quaternion.identity),
            actionOnGet: (enemy) =>
            { 
                enemy.GetComponent<Enemy>().ResetEnemy();
                enemy.SetActive(true); 
            },
            actionOnRelease: (enemy) => 
            {
                enemy.SetActive(false);
                enemy.transform.SetPositionAndRotation(spawnPositionTransform.position + (UtilClass.GetRamdomVector() * UnityEngine.Random.Range(0f, 10f)), Quaternion.identity);
            },
            actionOnDestroy: (enemy) => Destroy(enemy),
            collectionCheck: true,
            defaultCapacity: 5,
            maxSize: 55
            );
        OnPrepareToSpawn?.Invoke(this, true);
    }
    private void Start()
    {
        Enemy.OnAnyEnemyDie += Enemy_OnAnyEnemyDie;
    }

    private void Enemy_OnAnyEnemyDie(object sender, GameObject enemy)
    {
        if (activeEnemy.Contains(enemy))
        {
            activeEnemy.Remove(enemy);
            enemyPool.Release(enemy);
        }
        if(activeEnemy.Count == 0)
        {
            if (wave % 2 == 0)
            {
                timePrepareToSpawn = timeMaxPrepareToSpawn;
                OnPrepareToSpawn?.Invoke(this, true);
                state = State.PrepareWave;
            }
            else
            {
                SettingNextWave();
                state = State.SpawningWave;
            }
        }
    }

    private void Update()
    {
        HandlerWaveStatus();
    }
    private void HandlerWaveStatus()
    {
        switch (state)
        {
            case State.PrepareWave:
                timePrepareToSpawn -= Time.deltaTime;
                if (timePrepareToSpawn <= 0)
                {
                    SettingNextWave();
                    OnPrepareToSpawn?.Invoke(this, false);
                    state = State.SpawningWave;
                }
                break;
            case State.InWave:
                timeWave -= Time.deltaTime;
                if (timeWave <= 0)
                {
                    wave++;
                    if (wave % 2 == 0)
                    {
                        state = State.PrepareWave;
                        OnPrepareToSpawn?.Invoke(this, true);
                    }
                    else
                    {
                        SettingNextWave();
                        state = State.SpawningWave;
                    }
                }
                break;
            case State.SpawningWave:
                timeToSpawnEnenmy -= Time.deltaTime;
                if (numberOfEnemiesInWave > 0)
                {
                    if (timeToSpawnEnenmy <= 0)
                    {
                        timeToSpawnEnenmy = timeMaxToWaitSpawnNextEnemy;
                        EnemyMovement enemyMovement = enemyPool.Get().GetComponent<EnemyMovement>();
                        activeEnemy.Add(enemyMovement.gameObject);
                        if (enemyMovement.player == null)
                        {
                            enemyMovement.player = player;
                        }
                        numberOfEnemiesInWave--;
                    }
                }
                else
                {
                    timeWave = timeMaxToWaitSpawnNextWavel;
                    timeEnemyStartMoving -= Time.deltaTime;
                    if (playOne)
                    {
                        playOne = false;
                    }
                    if (timeEnemyStartMoving <= 0)
                    {
                        timeMaxEnemyStartMoving = timeMaxToWaitSpawnNextWavel * 0.1f;
                        timeEnemyStartMoving = timeMaxEnemyStartMoving;
                        timePrepareToSpawn = timeMaxPrepareToSpawn;
                        OnFullWaveEnemyReady?.Invoke(this, EventArgs.Empty);
                        state = State.InWave;
                    }
                }
                break;
        }
    }
    private void SettingNextWave()
    {
        spawnPositionTransform = listPointSpawns[UnityEngine.Random.Range(0, listPointSpawns.Count)];
        if (numberMaxOfEnemiesInWave < 55)
        {
            if (wave < 5)
            {
                numberMaxOfEnemiesInWave += mutiplyNumberEnemyWaveIncreases * wave;
            }
            else if (wave % 5 == 0)
            {
                numberMaxOfEnemiesInWave += mutiplyNumberEnemyWaveIncreases * wave;
                numberMaxOfEnemiesInWave = Mathf.Clamp(numberMaxOfEnemiesInWave, 1, 55);
            }
        }
        numberOfEnemiesInWave = numberMaxOfEnemiesInWave;
        timeMaxToWaitSpawnNextEnemy = (timeMaxToWaitSpawnNextWavel * 0.1f) / numberMaxOfEnemiesInWave;
        timeEnemyStartMoving = timeMaxEnemyStartMoving;
        playOne = true;
        OnNumberWaveChange?.Invoke(this, wave);
    }
    private void OnDestroy()
    {
        enemyPool.Clear();
        Enemy.OnAnyEnemyDie -= Enemy_OnAnyEnemyDie;
    }
}
