using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Member;
using UniRx;

namespace UI
{
    public class MemberWorldUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI actionPointsText;
        [SerializeField] private TextMeshProUGUI damagePointsText;
        [SerializeField] private TextMeshProUGUI healPointsText;
        [SerializeField] private MemberCharacter member;
        [SerializeField] private Image healthBarImage;
        [SerializeField] private HealthSystem healthSystem;

        private void Start()
        {
            MemberCharacter.OnAnyActionPointsChanged.Subscribe(_ => UpdateActionPointsText());
            healthSystem.OnDamaged.Subscribe(damageAmount => UpdateHealthBarDamaged(damageAmount));
            healthSystem.OnHealed.Subscribe(healAmount => UpdateHealthBarHealed(healAmount));

            UpdateActionPointsText();
            UpdateHealthBar();
        }

        private void UpdateActionPointsText()
        {
            actionPointsText.text = member.GetActionPoints().ToString();
        }

        private void UpdateHealthBar()
        {
            healthBarImage.fillAmount = healthSystem.GetHealthNormalized();
        }

        private void UpdateHealthBarDamaged(int damageAmount)
        {
            UpdateHealthBar();
            damagePointsText.text = damageAmount.ToString();
        }

        private void UpdateHealthBarHealed(int healAmount)
        {
            UpdateHealthBar();
            healPointsText.text = healAmount.ToString();
        }
    }
}