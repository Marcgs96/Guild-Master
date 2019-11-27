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
        public uint equipment_lvl;
        public MEMBER_TYPE type;

        public string GetTypeString()
        {
            string ret = "Unknown";
            switch (type)
            {
                case MEMBER_TYPE.KNIGHT:
                    ret = "Knight";
                    break;
                case MEMBER_TYPE.HUNTER:
                    ret = "Hunter";
                    break;
                case MEMBER_TYPE.MAGE:
                    ret = "Mage";
                    break;
            }
            return ret;
        }
    }

    protected MemberInfo info;
    public MEMBER_STATE state = MEMBER_STATE.SLEEP;

    protected Move move;
    protected Animator anim;
    public SteeringFollowNavMeshPath steer;
    public SteeringWander wander;
    protected Collider coll;
    public float task_time = 3.0f;

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
        wander = GetComponent<SteeringWander>();

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
    public void ChangeState(MEMBER_STATE state)
    {
        if (this.state == MEMBER_STATE.QUEST || (this.state == MEMBER_STATE.SLEEP && state != MEMBER_STATE.QUEST))
            return;

        this.state = state;
    }
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

    public string GetStateString()
    {
        string ret = "Unknown";
        switch (state)
        {
            case MEMBER_STATE.WORK:
                ret = GetMemberWorkString();
                break;
            case MEMBER_STATE.SLEEP:
                ret = steer.ReachedDestination() ? "Sleeping": "Going to Sleep";
                break;
            case MEMBER_STATE.QUEST:
                ret = steer.ReachedDestination() ? "On Quest" : "Going to the Dungeon";
                break;
        }

        return ret;
    }

    public void OnNewTask()
    {
       // GameManager.manager.ui.MemberActionChange(this);
    }

    virtual protected string GetMemberWorkString() { return "Unknown"; }
    public MemberInfo GetInfo()
    {
        return info;
    }
}
