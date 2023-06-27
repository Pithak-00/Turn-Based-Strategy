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

    [SerializeField] private MemberCharacter selectedMember;
    [SerializeField] private LayerMask unitLayerMask;

    private BaseCommand selectedCommand;
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
        SetSelectedMember(selectedMember);
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
        if(selectedMember == null)
        {
            return;
        }

        HandleSelectedCommand();
    }

    private void HandleSelectedCommand()
    {
        if (InputManager.Instance.IsMouseButtonDownThisFrame())
        {
            GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());

            if (!selectedCommand.IsValidActionGridPosition(mouseGridPosition))
            {
                return;
            }

            if (!selectedMember.TrySpendActionPointsToTakeAction(selectedCommand))
            {
                return;
            }

            SetBusy();
            selectedCommand.TakeAction(mouseGridPosition, ClearBusy);

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
                    if (member == selectedMember)
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
                    if (selectedMember != null && selectedCommand == selectedMember.GetAction<HealCommand>())
                    {
                    // ヒールコマンド選択中
                        return false;
                    }

                    SetSelectedMember(member);
                    return true;
                }
            }
        }

        return false;
    }

    public void SetSelectedMember(MemberCharacter member)
    {
        selectedMember = member;

        if (selectedMember != null)
        {
            selectedCommand = member.GetAction<MoveCommand>();
        }

        OnSelectedMemberChanged.OnNext(Unit.Default);
    }

    public void SetSelectedCommand(BaseCommand baseCommand)
    {
        selectedCommand = baseCommand;

        OnSelectedCommandChanged.OnNext(Unit.Default);
    }

    public MemberCharacter GetSelectedMember()
    {
        return selectedMember;
    }

    public BaseCommand GetSelectedCommand()
    {
        return selectedCommand;
    }
}
