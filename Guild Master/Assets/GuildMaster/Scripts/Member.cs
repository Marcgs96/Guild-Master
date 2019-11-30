using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Member : MonoBehaviour
{
    public enum MEMBER_STATE { QUEST, WORK, REST, NONE };
    public enum MEMBER_TYPE { KNIGHT, HUNTER, MAGE, TOTAL };

    [Header("Member Information")]
    public string member_name;
    public uint lvl;
    public uint xp;
    public uint equipment_lvl;
    public float stamina;
    public MEMBER_TYPE type;
    public MEMBER_STATE state = MEMBER_STATE.REST;
    public string action_string;
    public float night_value = 0;
    public GameObject assigned_position;
    public GameObject weapon;
    public GameObject model;

    [Header("Production")]
    public Resource.ResourceType product;
    [SerializeField]
    private int product_amount;
    public bool producing = false;
    public float production_time_game_hours;
    public int production_steps;
    protected float production_time_rt;
    public float production_step_time_rt;
    public float production_progress = 0.0f;
    [SerializeField]
    protected uint production_hours_cycle;
    [SerializeField]
    protected float production_total_cycle_cost;
    protected float production_stamina_cost;

    [Header("Components")]
    protected Move move;
    protected Animator anim;
    public SteeringFollowNavMeshPath steer;
    public SteeringWander wander;
    protected Collider coll;

    [Header("UI")]
    [SerializeField]
    protected Slider production_slider;
    [SerializeField]
    protected RawImage bubble_icon;

    // Start is called before the first frame update
    protected void Start()
    {
        coll = GetComponent<Collider>();
        move = this.GetComponent<Move>();
        anim = this.GetComponent<Animator>();

        DayNightCicle.OnDayCycleChange += ChangeNightValue;

        steer = GetComponent<SteeringFollowNavMeshPath>();
        wander = GetComponent<SteeringWander>();

        ChangeState((MEMBER_STATE)UnityEngine.Random.Range((int)MEMBER_STATE.WORK, (int)MEMBER_STATE.NONE));
        production_time_rt = GameManager.manager.time.InGameHoursToSeconds(production_time_game_hours);
        production_step_time_rt = production_time_rt / production_steps;
        production_stamina_cost = production_total_cycle_cost / GameManager.manager.time.InGameHoursToSeconds(production_hours_cycle);

        //Setup state
        anim.SetInteger("char_type", (int)type);
    }

    // Update is called once per frame
    void Update()
    {
        MemberUpdate();
    }
    
    virtual protected void MemberUpdate()
    {
        anim.SetFloat("speed", move.movement.magnitude);

        if (state == MEMBER_STATE.WORK && producing)
        {
            production_slider.gameObject.SetActive(true);
            DecreaseStamina(production_stamina_cost * Time.deltaTime);
            production_progress += Time.deltaTime;
            production_slider.value = production_progress / production_time_rt;
            if (production_progress >= production_time_rt)
            {
                GameManager.manager.resources.IncreaseResource(product, product_amount);
                production_progress = 0.0f;
            }
        }         
        else if (state == MEMBER_STATE.REST)
            IncreaseStamina(production_stamina_cost * Time.deltaTime);
    }

    protected void ChangeNightValue(bool night)
    {
        if (night)
            night_value = 5; //weight value of night for random action selection. This makes sleep have 60% chance at night.
        else
            night_value = 0;
    }
    virtual public void ChangeState(MEMBER_STATE state, bool force = false)
    {
        if (!force && this.state == MEMBER_STATE.QUEST)
            return;

        this.state = state;

        GameManager.manager.ui.UpdateStateButtonText(this);

        producing = false;
        production_progress = 0.0f;
        production_slider.gameObject.SetActive(false);
    }
    virtual public void GenerateInfo()
    {
        lvl = 1;
        xp = 0;
        stamina = 100;
    }

    public void OnBuildingExit()
    {
        transform.LookAt(steer.GetPathPosition());
        Disappear(false);
        steer.enabled = true;
    }

    internal void IncreaseStamina(float v)
    {
        if (stamina == 100)
            return;

        stamina += v;
        if (stamina > 100)
            stamina = 100;

        GameManager.manager.ui.OnMemberStaminaChange(this);
    }

    internal void DecreaseStamina(float value)
    {
        if (stamina == 0)
            return;

        stamina -= value;
        if (stamina < 0)
            stamina = 0;

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

    public void ActivateBubble(int icon_id)
    {
        bubble_icon.texture = GameManager.manager.ui.resource_images[icon_id];
        bubble_icon.gameObject.transform.parent.gameObject.SetActive(true);
        GameManager.manager.ui.UpdateStateIcon(icon_id, this);
    }
    
    public void DisableBubble()
    {
        bubble_icon.gameObject.transform.parent.gameObject.SetActive(false);
    }
}
