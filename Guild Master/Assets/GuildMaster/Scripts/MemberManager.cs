using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemberManager : MonoBehaviour
{
    [SerializeField]
    List<Member> members;
    [SerializeField]
    uint member_cap;
    [SerializeField]
    uint[] member_cap_lvl;

    public GameObject[] member_prefabs;
    public Transform spawn_location;

    public delegate void MemberAdded(Member new_member);
    public event MemberAdded OnMemberAdd;

    // Start is called before the first frame update
    void Start()
    {
        KnightMember.free_members.Clear();

        members = new List<Member>();
        SetMemberCap(GameManager.manager.buildings[(int)Building.BUILDING_TYPE.GUILD_HOUSE].GetLevel());
        GameManager.manager.buildings[(int)Building.BUILDING_TYPE.GUILD_HOUSE].OnLevelUp += SetMemberCap;
    }

    public void AddMember(Member.MEMBER_TYPE type)
    {
        if (members.Count >= member_cap)
            return;

        Member new_member = null;
        GameObject new_member_go = null;
        switch (type)
        {
            case Member.MEMBER_TYPE.KNIGHT:
                new_member_go = Instantiate(member_prefabs[(int)Member.MEMBER_TYPE.KNIGHT]);
                break;
            case Member.MEMBER_TYPE.HUNTER:
                new_member_go = Instantiate(member_prefabs[(int)Member.MEMBER_TYPE.HUNTER]);
                break;
            case Member.MEMBER_TYPE.MAGE:
                new_member_go = Instantiate(member_prefabs[(int)Member.MEMBER_TYPE.MAGE]);
                break;
        }

        new_member_go.transform.position = spawn_location.position;
        new_member = new_member_go.GetComponent<Member>();
        new_member.GenerateInfo();
        new_member_go.SetActive(true);
        members.Add(new_member);

        OnMemberAdd?.Invoke(new_member);
    }

    internal void SetMemberCap(uint level)
    {
        member_cap = member_cap_lvl[level-1];
        GameManager.manager.ui.UpdateMemberCountText();
    }

    internal object GetMemberCount()
    {
        return members.Count;
    }

    internal object GetMemberCap()
    {
        return member_cap;
    }

    public void RemoveMember(Member old_member)
    {
        members.Remove(old_member);
        GameManager.manager.ui.RemoveMemberListing(old_member);
        Destroy(old_member.gameObject);
    }
}
