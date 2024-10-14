using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [Range(0, 1f)]
    [SerializeField] private float moveSpeedMultipleByLevel;
    [SerializeField] private float rotationSpeed;
    [Range(0, 1f)]
    [SerializeField] private float rotationSpeedMultipleByLevel;
    [SerializeField] private BoxCollider2D boundingShape;
    [SerializeField] private Transform gun;
    public bool isWalking {  get; private set; }
    private void Start()
    {
        LevelSystem.instance.OnLevelChanged += LevelSystem_OnLevelChanged;
    }

    private void LevelSystem_OnLevelChanged(object sender, EventArgs e)
    {
        CalculatorPlayerMovementLevelScale(LevelSystem.instance.level);
    }

    private void Update()
    {
        HanderMovement();
        HanderRotation();
    }
    private void HanderMovement()
    {
        Vector3 direction = new(GameInputSystem.instance.GetVectorMovementnormalized().x, GameInputSystem.instance.GetVectorMovementnormalized().y, 0f);
        if (GameInputSystem.instance.GetVectorMovementnormalized() != Vector2.zero)
        {
            isWalking = true;
        }
        else
        {
            isWalking= false;
        }
        Vector3 newPosition = transform.position + moveSpeed * Time.deltaTime * direction;
        newPosition.x = Mathf.Clamp(newPosition.x, -boundingShape.size.x / 2, boundingShape.size.x / 2);
        newPosition.y = Mathf.Clamp(newPosition.y, -boundingShape.size.y / 2, boundingShape.size.y / 2);
        transform.position = newPosition;
    }
    private void HanderRotation()
    {
        Vector2 rotationDirection = new(GameInputSystem.instance.GetVectorRotation(Camera.main, Input.mousePosition, transform.position).x, GameInputSystem.instance.GetVectorRotation(Camera.main, Input.mousePosition, transform.position).y);
        if (rotationDirection != Vector2.zero)
        {
            float angle = Mathf.Atan2(rotationDirection.y, rotationDirection.x) * Mathf.Rad2Deg;
            gun.transform.rotation = Quaternion.Lerp(gun.transform.rotation, Quaternion.Euler(0, 0, angle - 90), Time.deltaTime * rotationSpeed);
        }
    }
    private void CalculatorPlayerMovementLevelScale(int level)
    {
        moveSpeed += moveSpeedMultipleByLevel * level;
        rotationSpeed += rotationSpeedMultipleByLevel * level;
    }
}
