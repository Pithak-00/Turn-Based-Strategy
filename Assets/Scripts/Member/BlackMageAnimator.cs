using UnityEngine;
using Command;
using UniRx;

namespace Member
{
    public class BlackMageAnimator : MonoBehaviour
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

            if (TryGetComponent(out MagicCommand magicCommand))
            {
                magicCommand.OnStartCommand.Subscribe(_ => SetTriggerMagic());
            }
        }

        private void SetTriggerAttack()
        {
            animator.SetTrigger("Attack");
        }

        private void SetTriggerMagic()
        {
            animator.SetTrigger("Magic");
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
