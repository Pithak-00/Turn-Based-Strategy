using UnityEngine;
using UniRx;
using Command;
using Grid;

namespace Member
{
    public class MemberCharacter : MonoBehaviour, IDamage, IHeal
    {
        public static ISubject<Unit> OnAnyActionPointsChanged = new Subject<Unit>();
        public static ISubject<MemberCharacter> OnAnyMemberSpawned = new Subject<MemberCharacter>();
        public static ISubject<MemberCharacter> OnAnyMemberDead = new Subject<MemberCharacter>();

        [SerializeField] private int actionPointsMax;
        [SerializeField] private bool isEnemy;

        private GridPosition gridPosition;
        private HealthSystem healthSystem;
        private BaseCommand[] baseCommandArray;
        private int actionPoints;

        private void Awake()
        {
            actionPoints = actionPointsMax;

            healthSystem = GetComponent<HealthSystem>();
            baseCommandArray = GetComponents<BaseCommand>();
        }

        private void Start()
        {
            gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
            LevelGrid.Instance.AddMemberAtGridPosition(gridPosition, this);

            TurnSystem.Instance.OnTurnChanged.Subscribe(_=> ResetActionPoint());

            healthSystem.OnDead.Subscribe(_ => HealthSystem_OnDead());

            OnAnyMemberSpawned.OnNext(this);
        }

        private void Update()
        {
            GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
            if (newGridPosition != gridPosition)
            {
                // UnitのGridPosition位置変更
                GridPosition oldGridPosition = gridPosition;
                gridPosition = newGridPosition;

                LevelGrid.Instance.MemberMovedGridPosition(this, oldGridPosition, newGridPosition);
            }
        }

        public T GetAction<T>() where T : BaseCommand
        {
            foreach (BaseCommand baseAction in baseCommandArray)
            {
                if (baseAction is T)
                {
                    return (T)baseAction;
                }
            }
            return null;
        }

        public GridPosition GetGridPosition()
        {
            return gridPosition;
        }

        public Vector3 GetWorldPosition()
        {
            return transform.position;
        }

        public BaseCommand[] GetBaseCommandArray()
        {
            return baseCommandArray;
        }

        public bool TrySpendActionPointsToTakeAction(BaseCommand baseAction)
        {
            if (CanSpendActionPointsToTakeAction(baseAction))
            {
                SpendActionPoints(baseAction.GetActionPointsCost());
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool CanSpendActionPointsToTakeAction(BaseCommand baseAction)
        {
            if (actionPoints >= baseAction.GetActionPointsCost())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // アクションポイント消費
        private void SpendActionPoints(int amount)
        {
            actionPoints -= amount;

            OnAnyActionPointsChanged.OnNext(Unit.Default);
        }
        /// <summary>
        /// アクションポイント取得
        /// </summary>
        public int GetActionPoints()
        {
            return actionPoints;
        }

        private void ResetActionPoint()
        {
            if ((IsEnemy() && !TurnSystem.Instance.IsPlayerTurn()) ||
                (!IsEnemy() && TurnSystem.Instance.IsPlayerTurn()))
            {
                actionPoints = actionPointsMax;

                OnAnyActionPointsChanged.OnNext(Unit.Default);
            }
        }

        public bool IsEnemy()
        {
            return isEnemy;
        }

        public void Damage(int damageAmount)
        {
            healthSystem.Damage(damageAmount);
        }

        public void Heal(int healAmount)
        {
            healthSystem.Heal(healAmount);
        }

        private void HealthSystem_OnDead()
        {
            LevelGrid.Instance.RemoveMemberAtGridPosition(gridPosition, this);

            Destroy(gameObject);

            MemberCommandSystem.Instance.SetSelectedMember(null);

            OnAnyMemberDead.OnNext(this);
        }

        public float GetHealthNormalized()
        {
            return healthSystem.GetHealthNormalized();
        }
    }
}