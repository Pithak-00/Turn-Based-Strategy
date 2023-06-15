using System;
using UnityEngine;
using Member;
using Grid;
using Stage;

namespace Command
{
    public class MagicObject : MonoBehaviour
    {
        public event EventHandler OnAnyMagicUsed;
        public static event EventHandler OnAnyMagicExploded;

        [SerializeField] private Transform magicExplodeVfxPrefab;
        [SerializeField] private TrailRenderer trailRenderer;
        [SerializeField] private AnimationCurve arcYAnimationCurve;
        [SerializeField] private int damagePoint;
        [SerializeField] private float totalDistance;

        private Vector3 targetPosition;
        private Action onMagicBehaviourComplete;
        private Vector3 positionXZ;

        private void Update()
        {
            Vector3 moveDir = (targetPosition - positionXZ).normalized;

            float moveSpeed = 15f;
            positionXZ += moveDir * moveSpeed * Time.deltaTime;

            float distance = Vector3.Distance(positionXZ, targetPosition);
            float distanceNormalized = 1 - distance / totalDistance;

            float maxHeight = totalDistance / 4f;
            float positionY = arcYAnimationCurve.Evaluate(distanceNormalized) * maxHeight;
            transform.position = new Vector3(positionXZ.x, positionY, positionXZ.z);

            float reachedTargetDistance = .2f;

            OnAnyMagicUsed?.Invoke(this, EventArgs.Empty);

            if (Vector3.Distance(positionXZ, targetPosition) < reachedTargetDistance)
            {
                float damageRadius = 4f;
                Collider[] colliderArray = Physics.OverlapSphere(targetPosition, damageRadius);

                foreach (Collider collider in colliderArray)
                {
                    if (collider.TryGetComponent(out MemberCharacter targetUnit))
                    {
                        targetUnit.Damage(damagePoint);
                    }
                    if (collider.TryGetComponent(out DestructibleCrate destructibleCrate))
                    {
                        destructibleCrate.Damage();
                    }
                }

                OnAnyMagicExploded?.Invoke(this, EventArgs.Empty);

                trailRenderer.transform.parent = null;

                Instantiate(magicExplodeVfxPrefab, targetPosition + Vector3.up * 1f, Quaternion.identity);

                Destroy(gameObject);

                onMagicBehaviourComplete();
            }
        }

        public void Setup(GridPosition targetGridPosition, Action onMagicBehaviourComplete)
        {
            this.onMagicBehaviourComplete = onMagicBehaviourComplete;
            targetPosition = LevelGrid.Instance.GetWorldPosition(targetGridPosition);

            positionXZ = transform.position;
            positionXZ.y = 0;
            totalDistance = Vector3.Distance(transform.position, targetPosition);
        }
    }
}