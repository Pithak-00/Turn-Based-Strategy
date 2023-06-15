using UnityEngine;
using Command;
using UniRx;

namespace Member
{
    public class WarriorAnimator : MonoBehaviour
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
                attackCommand.OnStartCommand.Subscribe(_ => SetTriggerSwordSlash());
            }
        }

        private void SetTriggerSwordSlash()
        {
            animator.SetTrigger("SwordSlash");
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