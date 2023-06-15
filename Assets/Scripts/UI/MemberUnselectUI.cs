using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class MemberUnselectUI : MonoBehaviour
    {
        [SerializeField] private Button unitUnselectBtn;

        private void Start()
        {
            unitUnselectBtn.onClick.AddListener(() =>
            {
                MemberCommandSystem.Instance.SetSelectedUnit(null);
            });
        }
    }
}
