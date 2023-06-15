using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UniRx;

namespace UI
{
    public class TurnSystemUI : MonoBehaviour
    {
        [SerializeField] private Button endTurnBtn;
        [SerializeField] private TextMeshProUGUI turnNumberText;
        [SerializeField] private GameObject enemyTurnVisualGameObject;

        private void Start()
        {
            endTurnBtn.onClick.AddListener(() =>
            {
                TurnSystem.Instance.NextTurn();
            });

            TurnSystem.Instance.OnTurnChanged.Subscribe(_ => UpdateOnTurnChanged());

            UpdateOnTurnChanged();
        }

        private void UpdateOnTurnChanged()
        {
            UpdateTurnText();
            UpdateEnemyTurnVisual();
            UpdateEndTurnButtonVisibility();
        }

        private void UpdateTurnText()
        {
            turnNumberText.text = "TURN " + TurnSystem.Instance.GetTurnNumber();
        }

        private void UpdateEnemyTurnVisual()
        {
            enemyTurnVisualGameObject.SetActive(!TurnSystem.Instance.IsPlayerTurn());
        }

        private void UpdateEndTurnButtonVisibility()
        {
            endTurnBtn.gameObject.SetActive(TurnSystem.Instance.IsPlayerTurn());
        }
    }
}