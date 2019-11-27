using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemberManager : MonoBehaviour
{
    [SerializeField]
    List<Member> members;
    [SerializeField]
    uint member_cap;
    uint members_amount;

    public GameObject[] member_prefabs;

    public delegate void MemberAdded(Member new_member);
    public static event MemberAdded OnMemberAdd;

    // Start is called before the first frame update
    void Start()
    {
        members = new List<Member>();
        //AddMember(Member.MEMBER_TYPE.KNIGHT);
       // AddMember(Member.MEMBER_TYPE.HUNTER);
       // AddMember(Member.MEMBER_TYPE.MAGE);
    }

    public void AddMember(Member.MEMBER_TYPE type)
    {
        Member new_member = null;
        GameObject new_member_go = null;
        switch (type)
        {
            case Member.MEMBER_TYPE.KNIGHT:
                new_member_go = Instantiate(member_prefabs[(int)Member.MEMBER_TYPE.KNIGHT]);

                //create knight
                break;
            case Member.MEMBER_TYPE.HUNTER:
                new_member_go = Instantiate(member_prefabs[(int)Member.MEMBER_TYPE.HUNTER]);

                //create hunter
                break;
            case Member.MEMBER_TYPE.MAGE:
                new_member_go = Instantiate(member_prefabs[(int)Member.MEMBER_TYPE.MAGE]);
                //create mage
                break;
        }

        new_member_go.transform.position = GameManager.manager.locations[(int)GameManager.LOCATION_TYPE.GUILD_HALL].transform.position;
        new_member = new_member_go.GetComponent<Member>();
        new_member_go.SetActive(true);
        new_member.GenerateInfo();


        OnMemberAdd?.Invoke(new_member);
    }
}
