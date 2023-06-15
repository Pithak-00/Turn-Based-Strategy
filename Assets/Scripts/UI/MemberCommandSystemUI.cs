using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UniRx;
using Member;

namespace UI
{
    public class MemberCommandSystemUI : MonoBehaviour
    {
        [SerializeField] private Transform commandButtonPrefab;
        [SerializeField] private Transform commandButtonContainerTransform;
        [SerializeField] private TextMeshProUGUI actionPointsText;

        private List<CommandButtonUI> commandButtonUIList;

        private void Awake()
        {
            commandButtonUIList = new List<CommandButtonUI>();
        }

        private void Start()
        {
            MemberCommandSystem.Instance.OnSelectedMemberChanged.Subscribe(_ => UpdateSelectedUnitChanged());
            MemberCommandSystem.Instance.OnSelectedCommandChanged.Subscribe(_ => UpdateSelectedVisual());
            MemberCommandSystem.Instance.OnActionStarted.Subscribe(_ => UpdateActionPoints());
            TurnSystem.Instance.OnTurnChanged.Subscribe(_ => UpdateActionPoints());
            MemberCharacter.OnAnyActionPointsChanged.Subscribe(_ => UpdateActionPoints());

            UpdateActionPoints();
            CreateUnitActionButtons();
            UpdateSelectedVisual();
        }
        private void CreateUnitActionButtons()
        {
            foreach (Transform buttonTransform in commandButtonContainerTransform)
            {
                Destroy(buttonTransform.gameObject);
            }

            commandButtonUIList.Clear();

            MemberCharacter selectedUnit = MemberCommandSystem.Instance.GetSelectedMember();

            if (selectedUnit != null)
            {
                foreach (Command.BaseCommand baseAction in selectedUnit.GetBaseActionArray())
                {
                    Transform actionButtonTransform = Instantiate(commandButtonPrefab, commandButtonContainerTransform);
                    CommandButtonUI actionButtonUI = actionButtonTransform.GetComponent<CommandButtonUI>();
                    actionButtonUI.SetBaseAction(baseAction);

                    commandButtonUIList.Add(actionButtonUI);
                }
            }
        }

        private void UpdateSelectedUnitChanged()
        {
            CreateUnitActionButtons();
            UpdateSelectedVisual();
            UpdateActionPoints();
        }

        private void UpdateSelectedVisual()
        {
            foreach (CommandButtonUI actionButtonUI in commandButtonUIList)
            {
                actionButtonUI.UpdateSelectedVisual();
            }
        }

        private void UpdateActionPoints()
        {
            MemberCharacter selectedUnit = MemberCommandSystem.Instance.GetSelectedMember();

            if (selectedUnit != null)
            {
                actionPointsText.text = "行動ポイント: " + selectedUnit.GetActionPoints();
            }
            else
            {
                actionPointsText.text = "行動ポイント: 0";
            }
        }
    }
}