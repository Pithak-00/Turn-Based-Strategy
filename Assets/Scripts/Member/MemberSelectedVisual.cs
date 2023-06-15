using UnityEngine;
using UniRx;

namespace Member
{
    public class MemberSelectedVisual : MonoBehaviour
    {
        [SerializeField] private MemberCharacter member;

        private MeshRenderer meshRenderer;

        private void Awake()
        {
            meshRenderer = GetComponent<MeshRenderer>();
        }

        private void Start()
        {
            MemberCommandSystem.Instance.OnSelectedMemberChanged.Subscribe(_ => UpdateVisual());

            UpdateVisual();
        }

        private void UpdateVisual()
        {
            if (MemberCommandSystem.Instance.GetSelectedMember() == member)
            {
                meshRenderer.enabled = true;
            }
            else
            {
                meshRenderer.enabled = false;
            }
        }

        private void OnDestroy()
        {
            MemberCommandSystem.Instance.OnSelectedMemberChanged.OnCompleted();
        }
    }
}