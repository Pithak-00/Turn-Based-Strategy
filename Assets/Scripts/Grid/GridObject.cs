using System.Collections.Generic;
using Member;

namespace Grid
{
    public class GridObject
    {
        private GridSystem<GridObject> gridSystem;
        private GridPosition gridPosition;
        private List<MemberCharacter> memberList;

        public GridObject(GridSystem<GridObject> gridSystem, GridPosition gridPosition)
        {
            this.gridSystem = gridSystem;
            this.gridPosition = gridPosition;
            memberList = new List<MemberCharacter>();
        }

        public override string ToString()
        {
            string unitString = "";
            foreach (MemberCharacter member in memberList)
            {
                unitString += member + "\n";
            }

            return gridPosition.ToString() + "\n" + unitString;
        }

        public void AddUnit(MemberCharacter member)
        {
            memberList.Add(member);
        }

        public void RemoveUnit(MemberCharacter member)
        {
            memberList.Remove(member);
        }

        public List<MemberCharacter> GetUnitList()
        {
            return memberList;
        }

        public bool HasAnyUnit()
        {
            return memberList.Count > 0;
        }

        public MemberCharacter GetUnit()
        {
            if (HasAnyUnit())
            {
                return memberList[0];
            }
            else
            {
                return null;
            }
        }
    }
}
