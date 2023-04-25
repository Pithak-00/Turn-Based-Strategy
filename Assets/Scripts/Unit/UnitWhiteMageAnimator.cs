using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitWhiteMageAnimator : MonoBehaviour
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

        if (TryGetComponent<HealAction>(out HealAction healAction))
        {
            healAction.OnHeal += HealAction_OnHealActionStarted;
        }
    }

    private void RodAction_OnRodActionStarted(object sender, EventArgs e)
    {
        animator.SetTrigger("Attack");
    }

    private void HealAction_OnHealActionStarted(object sender, EventArgs e)
    {
        animator.SetTrigger("Heal");
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
