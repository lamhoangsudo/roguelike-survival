using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour, IMovement
{
    public Player player;
    [SerializeField] private float moveSpeed;
    [Range(0, 1)]
    [SerializeField] private float moveSpeedMutiplyByLevel;
    [SerializeField] private Rigidbody2D enemyRigidbody;
    [SerializeField] private Transform enemyVisual;
    private float moveSpeed0;
    private bool isFullWaveEnemyReady;
    private void Awake()
    {
        CalculatorEnemyMovementLevelScale(LevelSystem.instance.level);
    }
    private void Start()
    {
        isFullWaveEnemyReady = false;
        EnemyWaveManager.instance.OnFullWaveEnemyReady += EnemyWaveManager_OnFullWaveEnemyReady;
        LevelSystem.instance.OnLevelChanged += LevelSystem_OnLevelChanged;
    }

    private void LevelSystem_OnLevelChanged(object sender, System.EventArgs e)
    {
        CalculatorEnemyMovementLevelScale(LevelSystem.instance.level);
    }

    private void Update()
    {
        if (player == null || !isFullWaveEnemyReady || GameManager.Instance.status == GameManager.GameStatus.GameOver) return;
        Move(player.GetPlayerPosition());
        Rotation(player.GetPlayerPosition());
    }
    public void Move(Vector3 movePosition)
    {
        Vector3 moveDir = (movePosition - this.transform.position).normalized;
        GetComponent<Rigidbody2D>().velocity = moveSpeed0 * moveDir;
        EnemyWaveManager.instance.OnFullWaveEnemyReady += EnemyWaveManager_OnFullWaveEnemyReady;
    }
    private void Rotation(Vector3 rotationPosition)
    {
        Vector2 rotationDirection = new(GameInputSystem.instance.GetVectorRotation(rotationPosition, transform.position).x, GameInputSystem.instance.GetVectorRotation(rotationPosition, transform.position).y);
        if (rotationDirection != Vector2.zero)
        {
            float angle = Mathf.Atan2(rotationDirection.y, rotationDirection.x) * Mathf.Rad2Deg;
            enemyVisual.rotation = Quaternion.Lerp(enemyVisual.rotation, Quaternion.Euler(0, 0, angle - 90), Time.deltaTime * 10f);
        }
    }
    private void EnemyWaveManager_OnFullWaveEnemyReady(object sender, System.EventArgs e)
    {
        isFullWaveEnemyReady = true;
    }
    private void CalculatorEnemyMovementLevelScale(int level)
    {
        moveSpeed0 = moveSpeed + moveSpeed * moveSpeedMutiplyByLevel * level;
    }
}
