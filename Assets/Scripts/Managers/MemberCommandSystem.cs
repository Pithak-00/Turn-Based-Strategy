using UnityEngine;
using UnityEngine.EventSystems;
using UniRx;
using Command;
using Member;
using Grid;

public class MemberCommandSystem : MonoBehaviour
{
    public static MemberCommandSystem Instance { get; private set; }

    public ISubject<Unit> OnSelectedMemberChanged = new Subject<Unit>();
    public ISubject<Unit> OnSelectedCommandChanged = new Subject<Unit>();
    public ISubject<bool> OnBusyChanged = new Subject<bool>();
    public ISubject<Unit> OnActionStarted = new Subject<Unit>();

    [SerializeField] private MemberCharacter selectedUnit;
    [SerializeField] private LayerMask unitLayerMask;

    private BaseCommand selectedAction;
    private bool isBusy;

    private void Awake()
    {
        if(Instance != null)
        {
            Debug.LogError("There's more than one MemberCommandSystem! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        SetSelectedUnit(selectedUnit);
    }

    private void Update()
    {
        if (isBusy)
        {
            return;
        }

        if (!TurnSystem.Instance.IsPlayerTurn())
        {
            return;
        }

        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        if (TryHandleUnitSelection())
        {
            return;
        }

        //TODO:選択中キャラは死亡で行動できないようにする
        //選択中状態を抜け出す方法を考えるべき
        if(selectedUnit == null)
        {
            return;
        }

        HandleSelectedAction();
    }

    private void HandleSelectedAction()
    {
        if (InputManager.Instance.IsMouseButtonDownThisFrame())
        {
            GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());

            if (!selectedAction.IsValidActionGridPosition(mouseGridPosition))
            {
                return;
            }

            if (!selectedUnit.TrySpendActionPointsToTakeAction(selectedAction))
            {
                return;
            }

            SetBusy();
            selectedAction.TakeAction(mouseGridPosition, ClearBusy);

            OnActionStarted.OnNext(Unit.Default);
        }
    }

    private void SetBusy()
    {
        isBusy = true;

        OnBusyChanged.OnNext(isBusy);
    }

    private void ClearBusy()
    {
        isBusy = false;

        OnBusyChanged.OnNext(isBusy);
    }

    private bool TryHandleUnitSelection()
    {
        if (InputManager.Instance.IsMouseButtonDownThisFrame())
        {
            Ray ray = Camera.main.ScreenPointToRay(InputManager.Instance.GetMouseScreenPosition());
            if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, unitLayerMask))
            {
                if (raycastHit.transform.TryGetComponent(out MemberCharacter member))
                {
                    if (member == selectedUnit)
                    {
                        // Unitは既に選択中
                        return false;
                    }

                    if (member.IsEnemy())
                    {
                        // 敵に選択
                        return false;
                    }

                    //TODO:ヒールコマンド選択中を修正
                    if (selectedUnit != null && selectedAction == selectedUnit.GetAction<HealCommand>())
                    {
                    // ヒールコマンド選択中
                        return false;
                    }

                    SetSelectedUnit(member);
                    return true;
                }
            }
        }

        return false;
    }

    public void SetSelectedUnit(MemberCharacter unit)
    {
        selectedUnit = unit;

        if (selectedUnit != null)
        {
            selectedAction = unit.GetAction<MoveCommand>();
        }

        OnSelectedMemberChanged.OnNext(Unit.Default);
    }

    public void SetSelectedAction(BaseCommand baseAction)
    {
        selectedAction = baseAction;

        OnSelectedCommandChanged.OnNext(Unit.Default);
    }

    public MemberCharacter GetSelectedMember()
    {
        return selectedUnit;
    }

    public BaseCommand GetSelectedCommand()
    {
        return selectedAction;
    }
}
