using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour, IMovement
{
    public Player player;
    [SerializeField] private float moveSpeed;
    [SerializeField] private Rigidbody2D enemyRigidbody;
    public void Move(Vector3 movePosition)
    {
        Vector3 moveDir = (movePosition - this.transform.position).normalized;
        GetComponent<Rigidbody2D>().velocity = moveSpeed * moveDir;
    }
    private void Update()
    {
        if (player == null) return;
        Move(player.GetPlayerPosition());

    }
}
