using UnityEngine;
using TMPro;
using UniRx;
using Command;

namespace UI
{
    public class ActionBusyUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textMeshPro;

        private void Start()
        {
            MemberCommandSystem.Instance.OnBusyChanged.Subscribe(isBusy => UpdateBusy(isBusy));

            Hide();
        }

        private void Show()
        {
            BaseCommand selectedAction = MemberCommandSystem.Instance.GetSelectedCommand();
            textMeshPro.text = selectedAction.GetCommandName();

            gameObject.SetActive(true);
        }

        private void Hide()
        {
            gameObject.SetActive(false);
        }

        private void UpdateBusy(bool isBusy)
        {
            if (isBusy)
            {
                Show();
            }
            else
            {
                Hide();
            }
        }
    }
}