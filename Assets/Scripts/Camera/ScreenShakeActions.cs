using UnityEngine;
using Command;
using UniRx;

namespace CameraControl
{
    public class ScreenShakeActions : MonoBehaviour
    {
        private void Start()
        {
            AttackCommand.OnEndAttack.Subscribe(_ => ScreenShakeShake(2f));
        }

        private void ScreenShakeShake(float shakeRate)
        {
            CameraManager.Instance.Shake(shakeRate);
        }
    }
}