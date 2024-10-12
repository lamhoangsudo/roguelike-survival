using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private PlayerMovement playerMovement;
    private void Update()
    {
        if(playerMovement.isWalking)
        {
            playerAnimator.Play("WalkAnimation");
        }
        else
        {
            playerAnimator.Play("IdleAnimation");
        }
    }
}
