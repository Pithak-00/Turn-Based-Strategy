using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitBlackMageAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private void Awake()
    {
        if (TryGetComponent<MoveAction>(out MoveAction moveAction))
        {
            moveAction.OnStartMoving += MoveAction_OnStartMoving;
            moveAction.OnStopMoving += MoveAction_OnStopMoving;
        }

        if (TryGetComponent<RodAction>(out RodAction rodAction))
        {
            rodAction.OnActionStarted += RodAction_OnRodActionStarted;
        }

        if (TryGetComponent<MagicProjectile>(out MagicProjectile magicProjectile))
        {
            magicProjectile.OnAnyMagicUsed += MagicAction_OnMagicActionStarted;
        }
    }

    private void RodAction_OnRodActionStarted(object sender, EventArgs e)
    {
        animator.SetTrigger("Attack");
    }

    private void MagicAction_OnMagicActionStarted(object sender, EventArgs e)
    {
        animator.SetTrigger("Magic");
    }

    private void MoveAction_OnStartMoving(object sender, EventArgs e)
    {
        animator.SetBool("IsWalking", true);
    }

    private void MoveAction_OnStopMoving(object sender, EventArgs e)
    {
        animator.SetBool("IsWalking", false);
    }
}
