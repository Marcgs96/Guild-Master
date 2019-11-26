using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Member : MonoBehaviour
{
    public enum MEMBER_STATE { QUEST, SLEEP, WORK, REST, NONE};

    public enum MEMBER_TYPE { KNIGHT, HUNTER, MAGE, NONE };

    public struct MemberInfo
    {
        public string name;
        public uint lvl;
        public uint xp;
        public MEMBER_TYPE type;
    }

    protected MemberInfo info;
    public MEMBER_STATE state = MEMBER_STATE.SLEEP;

    protected Move move;
    protected Animator anim;
    protected SteeringFollowNavMeshPath steer;
    protected Collider coll;

    public GameObject weapon;
    public GameObject model;

    //UI
    protected GameObject action_bubble;
    protected GameObject blacksmith_bubble;

    // Start is called before the first frame update
    protected void Start()
    {
        coll = GetComponent<Collider>();
        move = this.GetComponent<Move>();
        anim = this.GetComponent<Animator>();

        DayNightCicle.OnHourChange += ChangeState;

        steer = GetComponent<SteeringFollowNavMeshPath>();

        action_bubble = transform.Find("ActionBubble").gameObject;
        blacksmith_bubble = transform.Find("BlacksmithBubble").gameObject;

        //Setup state
        anim.SetInteger("char_type", (int)info.type);
    }

    // Update is called once per frame
    protected void Update()
    {
        anim.SetFloat("speed", move.current_velocity.magnitude);
    }

    virtual protected void ChangeState(uint hour) { }
    virtual public void GenerateInfo() { }

    public void OnBuildingExit()
    {
        transform.LookAt(steer.GetPathPosition());
        Disappear(false);
        steer.enabled = true;
    }

    public void OnBuildingEnter()
    {
        Disappear(true);
        steer.enabled = false;
    }

    public void Disappear(bool mode)
    {
        if (mode)
        {
            model.SetActive(false);
            coll.enabled = false;
        }
        else
        {
            model.SetActive(true);
            coll.enabled = true;
        }
    }
    public MemberInfo GetInfo()
    {
        return info;
    }
}
