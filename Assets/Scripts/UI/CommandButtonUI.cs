using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Command;

namespace UI
{
    public class CommandButtonUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textMeshPro;
        [SerializeField] private Button button;
        [SerializeField] private GameObject selectedGameObject;

        private BaseCommand BaseCommand;

        public void SetBaseAction(BaseCommand baseCommand)
        {
            this.BaseCommand = baseCommand;
            textMeshPro.text = baseCommand.GetCommandName();

            button.onClick.AddListener(() =>
            {
                MemberCommandSystem.Instance.SetSelectedAction(baseCommand);
            });
        }

        public void UpdateSelectedVisual()
        {
            BaseCommand selectedBaseAction = MemberCommandSystem.Instance.GetSelectedCommand();
            selectedGameObject.SetActive(selectedBaseAction == BaseCommand);
        }
    }
}