using UnityEngine;
using Member;
using UniRx;

namespace UI
{
    public class DamagePointsUI : MonoBehaviour
    {
        [SerializeField] private HealthSystem healthSystem;

        private float waitTimeSecond = 0.5f;
        private float waitTimer;
        private Vector3 defalutPosition;

        private void Start()
        {
            healthSystem.OnDamaged.Subscribe(_ => Show());
            defalutPosition = gameObject.transform.localPosition;

            Hide();
        }

        private void Update()
        {
            waitTimer += Time.deltaTime;

            if (waitTimer >= waitTimeSecond)
            {
                Fly();

                if (waitTimer >= waitTimeSecond + 0.5f)
                {
                    Hide();
                }
            }
        }

        private void Show()
        {
            waitTimer = 0f;
            gameObject.SetActive(true);
            gameObject.transform.localPosition = defalutPosition;
        }

        private void Fly()
        {
            Vector3 pos = gameObject.transform.localPosition;
            pos.y += 0.01f;
            gameObject.transform.localPosition = pos;
        }

        private void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}