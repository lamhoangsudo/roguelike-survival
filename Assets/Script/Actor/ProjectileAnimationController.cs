using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileAnimationController : MonoBehaviour
{
    [SerializeField] private Animator projectileAnimator;
    private void Start()
    {
        projectileAnimator.Play("ProjectileIdel");
    }
}
