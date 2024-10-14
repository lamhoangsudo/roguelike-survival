using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInputSystem : MonoBehaviour
{
    public static GameInputSystem instance;
    private PlayerInput inputActions;
    public bool isShoot {  get; private set; }
    private void Awake()
    {
        if (instance == null) 
        { 
            instance = this;
        }

        inputActions = new();

        inputActions.Player.Enable();

        inputActions.Player.PlayerShoot.performed += PlayerShoot_performed;
        inputActions.Player.PlayerShoot.canceled += PlayerShoot_canceled;
    }

    private void PlayerShoot_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        isShoot = false;
    }

    private void PlayerShoot_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        isShoot = true;
    }

    private void OnDestroy()
    {
        inputActions.Dispose();
    }
    public Vector2 GetVectorMovementnormalized()
    {
        return inputActions.Player.PlayerControl.ReadValue<Vector2>().normalized;
    }
    public Vector3 GetVectorRotation(Camera camera, Vector3 worldMousePosition, Vector3 playerPosition)
    {
        return camera.ScreenToWorldPoint(worldMousePosition) - playerPosition;
    }
}
