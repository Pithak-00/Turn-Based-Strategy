using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Member;

public class MemberManager : MonoBehaviour
{
    public static MemberManager Instance { get; private set; }

    private List<MemberCharacter> memberList;
    private List<MemberCharacter> allyMemberList;
    private List<MemberCharacter> enemyUnitList;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There's more than one MemberManager! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;

        memberList = new List<MemberCharacter>();
        allyMemberList = new List<MemberCharacter>();
        enemyUnitList = new List<MemberCharacter>();
    }

    private void Start()
    {
        MemberCharacter.OnAnyMemberSpawned.Subscribe(member => AddMemberList(member));
        MemberCharacter.OnAnyMemberDead.Subscribe(member => RemoveMemberList(member));
    }

    private void AddMemberList(object sender)
    {
        MemberCharacter member = sender as MemberCharacter;

        memberList.Add(member);

        if (member.IsEnemy())
        {
            enemyUnitList.Add(member);
        }
        else
        {
            allyMemberList.Add(member);
        }
    }


    private void RemoveMemberList(object sender)
    {
        MemberCharacter member = sender as MemberCharacter;

        memberList.Remove(member);

        if (member.IsEnemy())
        {
            enemyUnitList.Remove(member);
            ScoreManager.Instance.AddScore(100);
        }
        else
        {
            allyMemberList.Remove(member);
        }
    }

    public List<MemberCharacter> GetMemberList()
    {
        return memberList;
    }

    public List<MemberCharacter> GetAllyMemberList()
    {
        return allyMemberList;
    }

    public List<MemberCharacter> GetEnemyMemberList()
    {
        return enemyUnitList;
    }
}
