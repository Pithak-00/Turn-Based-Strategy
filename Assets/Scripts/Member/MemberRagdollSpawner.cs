using UnityEngine;
using UniRx;

namespace Member
{
    public class MemberRagdollSpawner : MonoBehaviour
    {
        [SerializeField] private Transform ragdollPrefab;
        [SerializeField] private Transform originalRootBone;

        private HealthSystem healthSystem;

        private void Awake()
        {
            healthSystem = GetComponent<HealthSystem>();

            healthSystem.OnDead.Subscribe(_ => InstantiateRagdoll());
        }

        private void InstantiateRagdoll()
        {
            Transform ragdollTransform = Instantiate(ragdollPrefab, transform.position, transform.rotation);
            MemberRagdoll unitRagdoll = ragdollTransform.GetComponent<MemberRagdoll>();
            unitRagdoll.SetUp(originalRootBone);
        }
    }
}