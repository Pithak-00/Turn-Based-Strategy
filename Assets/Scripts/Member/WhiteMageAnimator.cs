using UnityEngine;
using Command;
using UniRx;

namespace Member
{
    public class WhiteMageAnimator : MonoBehaviour
    {
        [SerializeField] private Animator animator;

        private void Awake()
        {
            if (TryGetComponent(out MoveCommand moveCommand))
            {
                moveCommand.OnStartCommand.Subscribe(_ => SetBoolIsWalkingTrue());
                moveCommand.OnEndCommand.Subscribe(_ => SetBoolIsWalkingFalse());
            }

            if (TryGetComponent(out AttackCommand attackCommand))
            {
                attackCommand.OnStartCommand.Subscribe(_ => SetTriggerAttack());
            }

            if (TryGetComponent(out HealCommand healCommand))
            {
                healCommand.OnStartCommand.Subscribe(_ => SetTriggerHeal());
            }
        }

        private void SetTriggerAttack()
        {
            animator.SetTrigger("Attack");
        }

        private void SetTriggerHeal()
        {
            animator.SetTrigger("Heal");
        }

        private void SetBoolIsWalkingTrue()
        {
            animator.SetBool("IsWalking", true);
        }

        private void SetBoolIsWalkingFalse()
        {
            animator.SetBool("IsWalking", false);
        }
    }
}