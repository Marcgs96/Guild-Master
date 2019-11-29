using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Member : MonoBehaviour
{
    public enum MEMBER_STATE { QUEST, WORK, REST, NONE };
    public enum MEMBER_TYPE { KNIGHT, HUNTER, MAGE, NONE };
    public struct MemberInfo
    {
        public string name;
        public uint lvl;
        public uint xp;
        public uint equipment_lvl;
        public float stamina;
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
    public MEMBER_STATE state = MEMBER_STATE.REST;
    public string action_string;
    public float night_value = 0;
    public bool producing = false;

    protected float production_stamina_cost;
    [SerializeField]
    protected uint production_hours_cycle;
    [SerializeField]
    protected float production_total_cycle_cost;

    protected Move move;
    protected Animator anim;
    public SteeringFollowNavMeshPath steer;
    public SteeringWander wander;
    protected Collider coll;
    public float task_time;
    GameObject assigned_position;

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

        DayNightCicle.OnDayCycleChange += ChangeNightValue;

        steer = GetComponent<SteeringFollowNavMeshPath>();
        wander = GetComponent<SteeringWander>();

        action_bubble = transform.Find("ActionBubble").gameObject;
        blacksmith_bubble = transform.Find("BlacksmithBubble").gameObject;

        state = (MEMBER_STATE)UnityEngine.Random.Range((int)MEMBER_STATE.WORK, (int)MEMBER_STATE.NONE);
        production_stamina_cost = production_total_cycle_cost / GameManager.manager.time.InGameHoursToSeconds(production_hours_cycle);

        //Setup state
        anim.SetInteger("char_type", (int)info.type);
    }

    // Update is called once per frame
    protected void Update()
    {
        anim.SetFloat("speed", move.current_velocity.magnitude);

        if (producing)
            DecreaseStamina(production_stamina_cost * Time.fixedDeltaTime);
        else if (state == MEMBER_STATE.REST)
            IncreaseStamina(production_stamina_cost * Time.fixedDeltaTime);
    }

    protected void ChangeNightValue(bool night)
    {
        if (night)
            night_value = 5; //weight value of night for random action selection. This makes sleep have 60% chance at night.
        else
            night_value = 0;
    }
    public void ChangeState(MEMBER_STATE state, bool force = false)
    {
        if (!force && this.state == MEMBER_STATE.QUEST)
            return;

        this.state = state;
        producing = false;
    }
    virtual public void GenerateInfo()
    {
        info.lvl = 1;
        info.xp = 0;
        info.stamina = 100;
    }

    public void OnBuildingExit()
    {
        transform.LookAt(steer.GetPathPosition());
        Disappear(false);
        steer.enabled = true;
    }

    internal void IncreaseStamina(float v)
    {
        if (info.stamina == 100)
            return;

        info.stamina += v;
        if (info.stamina > 100)
            info.stamina = 100;

        GameManager.manager.ui.OnMemberStaminaChange(this);
    }

    internal void DecreaseStamina(float value)
    {
        if (info.stamina == 0)
            return;

        info.stamina -= value;
        if (info.stamina < 0)
            info.stamina = 0;

        GameManager.manager.ui.OnMemberStaminaChange(this);
    }

    public void OnBuildingEnter()
    {
        Disappear(true);
        steer.enabled = false;

        if (state == MEMBER_STATE.QUEST)
            GameManager.manager.quests.OnDungeonEnter(this);
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

    virtual protected string GetMemberWorkString() { return "Unknown"; }
    public MemberInfo GetInfo()
    {
        return info;
    }

    public void ChangeActionString(string new_string)
    {
        action_string = new_string;
        GameManager.manager.ui.MemberActionChange(this);
    }

    public GameObject RequestPosition(GameObject location)
    {
        GameManager.manager.locations.ReleasePosition(assigned_position);
        assigned_position = GameManager.manager.locations.GetAvailablePosition(location);

        return assigned_position;
    }
}
