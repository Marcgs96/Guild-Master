using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Member : MonoBehaviour
{
    protected enum CHARACTER_ACTION { NONE, ENTER_TAVERN, ENTER_GUILD_HALL, TYPE_ACTION, BLACKSMITH };
    protected CHARACTER_ACTION current_action;

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

        DayNightCicle.OnHourChange += ChangeAction;

        steer = GetComponent<SteeringFollowNavMeshPath>();
        steer.OnReachEnd += DoAction;

        action_bubble = transform.Find("ActionBubble").gameObject;
        blacksmith_bubble = transform.Find("BlacksmithBubble").gameObject;

        //Setup state
        anim.SetInteger("char_type", (int)info.type);
        current_action = CHARACTER_ACTION.ENTER_GUILD_HALL;
        DoAction();
    }

    // Update is called once per frame
    protected void Update()
    {
        anim.SetFloat("speed", move.current_velocity.magnitude);
    }

    virtual protected void ChangeAction(uint hour) { }
    virtual public void GenerateInfo() { }

    virtual public void DoAction()
    {
        switch (current_action)
        {
            case CHARACTER_ACTION.ENTER_TAVERN:
                GameManager.manager.buildings[(int)GameManager.LOCATION_TYPE.TAVERN].EnterBuilding(this);
                OnBuildingEnter();
                break;
            case CHARACTER_ACTION.ENTER_GUILD_HALL:
                GameManager.manager.buildings[(int)GameManager.LOCATION_TYPE.GUILD_HALL].EnterBuilding(this);
                OnBuildingEnter();
                break;
            case CHARACTER_ACTION.BLACKSMITH:
                blacksmith_bubble.SetActive(true);
                break;
            case CHARACTER_ACTION.TYPE_ACTION:
                GameManager.manager.audios[(int)info.type].enabled = true;
                anim.SetBool("type_action", true);
                action_bubble.SetActive(true);
                weapon.SetActive(true);
                break;
            default:
                break;
        }
    }

    virtual public void StopAction()
    {
        switch (current_action)
        {
            case CHARACTER_ACTION.ENTER_TAVERN:
                GameManager.manager.buildings[(int)GameManager.LOCATION_TYPE.TAVERN].RequestExit(this);
                break;
            case CHARACTER_ACTION.ENTER_GUILD_HALL:
                GameManager.manager.buildings[(int)GameManager.LOCATION_TYPE.GUILD_HALL].RequestExit(this);
                break;
            case CHARACTER_ACTION.BLACKSMITH:
                blacksmith_bubble.SetActive(false);
                break;
            case CHARACTER_ACTION.TYPE_ACTION:
                GameManager.manager.audios[(int)info.type].enabled = false;
                anim.SetBool("type_action", false);
                action_bubble.SetActive(false);
                weapon.SetActive(false);
                break;
            default:
                break;
        }
    }

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
        switch (current_action)
        {
            case CHARACTER_ACTION.ENTER_TAVERN:
                ret = "Tavern";
                break;
            case CHARACTER_ACTION.ENTER_GUILD_HALL:
                ret = "Guild Hall";
                break;
            case CHARACTER_ACTION.BLACKSMITH:
                ret = "Blacksmith";
                break;
            case CHARACTER_ACTION.TYPE_ACTION:
                ret = GetTypeActionString();
                break;
        }

        return ret;
    }

    virtual protected string GetTypeActionString() { return "Unknown"; }
    public MemberInfo GetInfo()
    {
        return info;
    }
}
