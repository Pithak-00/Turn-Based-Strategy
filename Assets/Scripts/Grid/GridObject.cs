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
            string memberString = "";
            foreach (MemberCharacter member in memberList)
            {
                memberString += member + "\n";
            }

            return gridPosition.ToString() + "\n" + memberString;
        }

        public void AddMember(MemberCharacter member)
        {
            memberList.Add(member);
        }

        public void RemoveMember(MemberCharacter member)
        {
            memberList.Remove(member);
        }

        public List<MemberCharacter> GetMemberList()
        {
            return memberList;
        }

        public bool HasAnyMember()
        {
            return memberList.Count > 0;
        }

        public MemberCharacter GetMember()
        {
            if (HasAnyMember())
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
