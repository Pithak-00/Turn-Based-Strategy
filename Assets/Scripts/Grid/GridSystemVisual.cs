using System;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Member;
using Command;

namespace Grid
{
    public class GridSystemVisual : MonoBehaviour
    {
        [Serializable]
        public struct GridVisualTypeMaterial
        {
            public GridVisualType gridVisualType;
            public Material material;
        }

        public enum GridVisualType
        {
            White,
            Blue,
            Red,
            RedSoft,
            Yellow,
            Green,
            GreenSoft
        }

        [SerializeField] private Transform gridSystemVisualSinglePrefab;
        [SerializeField] private List<GridVisualTypeMaterial> gridVisualTypeMaterialList;

        private GridSystemVisualSingle[,] gridSystemVisualSingleArray;

        private void Start()
        {
            gridSystemVisualSingleArray = new GridSystemVisualSingle[
                LevelGrid.Instance.GetWidth(),
                LevelGrid.Instance.GetHeight()
            ];

            for (int x = 0; x < LevelGrid.Instance.GetWidth(); x++)
            {
                for (int z = 0; z < LevelGrid.Instance.GetHeight(); z++)
                {
                    GridPosition gridPosition = new GridPosition(x, z);
                    Transform gridSystemVisualSingleTransform =
                        Instantiate(gridSystemVisualSinglePrefab, LevelGrid.Instance.GetWorldPosition(gridPosition), Quaternion.identity);

                    gridSystemVisualSingleArray[x, z] = gridSystemVisualSingleTransform.GetComponent<GridSystemVisualSingle>();
                }
            }

            MemberCommandSystem.Instance.OnSelectedMemberChanged.Subscribe(_ => UpdateGridVisual());
            MemberCommandSystem.Instance.OnSelectedCommandChanged.Subscribe(_ => UpdateGridVisual());
            LevelGrid.Instance.OnAnyUnitMovedGridPosition.Subscribe(_ => UpdateGridVisual());

            UpdateGridVisual();
        }

        public void HideAllGridPosition()
        {
            for (int x = 0; x < LevelGrid.Instance.GetWidth(); x++)
            {
                for (int z = 0; z < LevelGrid.Instance.GetHeight(); z++)
                {
                    gridSystemVisualSingleArray[x, z].Hide();
                }
            }
        }

        private void ShowGridPositionRange(GridPosition gridPosition, int range, GridVisualType gridVisualType)
        {
            List<GridPosition> gridPositionList = new List<GridPosition>();

            for (int x = -range; x <= range; x++)
            {
                for (int z = -range; z <= range; z++)
                {
                    GridPosition testGridPosition = gridPosition + new GridPosition(x, z);

                    if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                    {
                        continue;
                    }

                    int testDistance = Math.Abs(x) + Mathf.Abs(z);
                    if (testDistance > range)
                    {
                        continue;
                    }

                    gridPositionList.Add(testGridPosition);
                }
            }

            ShowGridPositionList(gridPositionList, gridVisualType);
        }

        private void ShowGridPositionRangeSquare(GridPosition gridPosition, int range, GridVisualType gridVisualType)
        {
            List<GridPosition> gridPositionList = new List<GridPosition>();

            for (int x = -range; x <= range; x++)
            {
                for (int z = -range; z <= range; z++)
                {
                    GridPosition testGridPosition = gridPosition + new GridPosition(x, z);

                    if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                    {
                        continue;
                    }

                    gridPositionList.Add(testGridPosition);
                }
            }

            ShowGridPositionList(gridPositionList, gridVisualType);
        }

        public void ShowGridPositionList(List<GridPosition> gridPositionList, GridVisualType gridVisualType)
        {
            foreach (GridPosition gridPosition in gridPositionList)
            {
                gridSystemVisualSingleArray[gridPosition.x, gridPosition.z].
                    Show(GetGridVisualTypeMaterial(gridVisualType));
            }
        }

        private void UpdateGridVisual()
        {
            HideAllGridPosition();

            MemberCharacter selectedMember = MemberCommandSystem.Instance.GetSelectedMember();
            BaseCommand selectedCommand = MemberCommandSystem.Instance.GetSelectedCommand();

            GridVisualType gridVisualType;

            //TODO:死亡の際、グリッド表示させないように
            if (selectedMember != null)
            {
                switch (selectedCommand)
                {
                    default:
                    case MoveCommand moveCommand:
                        gridVisualType = GridVisualType.Yellow;
                        break;
                    case WaitCommand waitCommand:
                        gridVisualType = GridVisualType.Blue;
                        break;
                    case AttackCommand attackCommand:
                        gridVisualType = GridVisualType.Red;
                        ShowGridPositionRangeSquare(selectedMember.GetGridPosition(), attackCommand.GetMaxDistance(), GridVisualType.RedSoft);
                        break;
                    case MagicCommand magicCommand:
                        gridVisualType = GridVisualType.Red;
                        ShowGridPositionRangeSquare(selectedMember.GetGridPosition(), magicCommand.GetMaxDistance(), GridVisualType.RedSoft);
                        break;
                    case HealCommand healCommand:
                        gridVisualType = GridVisualType.Green;
                        ShowGridPositionRange(selectedMember.GetGridPosition(), healCommand.GetMaxShootDistance(), GridVisualType.GreenSoft);
                        break;
                }


                ShowGridPositionList(selectedCommand.GetValidActionGridPositionList(), gridVisualType);
            }
        }

        private Material GetGridVisualTypeMaterial(GridVisualType gridVisualType)
        {
            foreach (GridVisualTypeMaterial gridVisualTypeMaterial in gridVisualTypeMaterialList)
            {
                if (gridVisualTypeMaterial.gridVisualType == gridVisualType)
                {
                    return gridVisualTypeMaterial.material;
                }
            }

            Debug.LogError("Could not find GridVisualTypeMaterial for GridVisualType " + gridVisualType);
            return null;
        }
    }
}