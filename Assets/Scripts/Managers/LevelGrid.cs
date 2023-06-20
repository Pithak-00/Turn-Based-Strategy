using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Member;
using Grid;

public class LevelGrid : MonoBehaviour
{
    public static LevelGrid Instance { get; private set; }

    public ISubject<Unit> OnAnyMemberMovedGridPosition = new Subject<Unit>();

    [SerializeField] private Transform gridDebugObjectPrefab;
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private float cellSize;

    private GridSystem<GridObject> gridSystem;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There's more than one LevelGrid! " + transform + " - " + Instance);
            Destroy(gameObject);
        }
        Instance = this;

        gridSystem = new GridSystem<GridObject>(width, height, cellSize,
            (GridSystem<GridObject>g,GridPosition gridPosition) => new GridObject(g,gridPosition));

        //TODO:デバッグ用
        //gridSystem.CreateDebugObjects(gridDebugObjectPrefab);
    }

    private void Start()
    {
        Pathfinding.Instance.Setup(width, height, cellSize);
    }

    public void AddMemberAtGridPosition(GridPosition gridPosition, MemberCharacter member)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        gridObject.AddMember(member);
    }

    public List<MemberCharacter> GetMemberListAtGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        return gridObject.GetMemberList();
    }

    public void RemoveMemberAtGridPosition(GridPosition gridPosition, MemberCharacter member)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        gridObject.RemoveMember(member);
    }

    public void MemberMovedGridPosition(MemberCharacter member, GridPosition fromGridPosition, GridPosition toGridPosition)
    {
        RemoveMemberAtGridPosition(fromGridPosition, member);

        AddMemberAtGridPosition(toGridPosition, member);

        OnAnyMemberMovedGridPosition.OnNext(Unit.Default);
    }

    public GridPosition GetGridPosition(Vector3 worldPosition) => gridSystem.GetGridPosition(worldPosition);

    public Vector3 GetWorldPosition(GridPosition gridPosition) => gridSystem.GetWorldPosition(gridPosition);

    public bool IsValidGridPosition(GridPosition gridPosition) => gridSystem.IsValidGridPosition(gridPosition);

    public int GetWidth() => gridSystem.GetWidth();

    public int GetHeight() => gridSystem.GetHeight();

    public bool HasAnyMemberOnGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        return gridObject.HasAnyMember();
    }

    public MemberCharacter GetMemberAtGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        return gridObject.GetMember();
    }
}
