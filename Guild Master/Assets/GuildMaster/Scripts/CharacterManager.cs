using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    Move move;
    Animator anim;
    SteeringFollowNavMeshPath steer;
    Collider coll;
    DayNightCicle time;
    public Transform[] locations;


    public Building tavern;
    public Building guild_hall;
    

    public GameObject model;

    enum CHARACTER_ACTION { NONE, ENTER_TAVERN, ENTER_GUILD_HALL, TYPE_ACTION, BLACKSMITH };
    CHARACTER_ACTION current_action;

    public enum CHARACTER_TYPE {NONE, KNIGHT, HUNTER, MAGE};
    public CHARACTER_TYPE type;

    GameObject action_bubble;
    GameObject blacksmith_bubble;


    public enum LOCATION_TYPE {TABERN, GUILD_HALL, BLACKSMITH, CLASS};

    // Start is called before the first frame update
    void Start()
    {
        coll = GetComponent<Collider>();
        move = this.GetComponent<Move>();
        anim = this.GetComponent<Animator>();
        
        time = GameObject.Find("Main Camera").GetComponent<DayNightCicle>();

        DayNightCicle.OnHourChange += ChangeAction;
        steer = GetComponent<SteeringFollowNavMeshPath>();
        steer.OnReachEnd += DoAction;

        action_bubble = transform.Find("ActionBubble").gameObject;
        blacksmith_bubble = transform.Find("BlacksmithBubble").gameObject;


        //Setup state
        anim.SetInteger("char_type", (int)type);
        current_action = CHARACTER_ACTION.ENTER_GUILD_HALL;
        DoAction();
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetFloat("speed", move.current_velocity.magnitude);
    }

    public void DoAction()
    {
        switch (current_action)
        {
            case CHARACTER_ACTION.ENTER_TAVERN:
                tavern.EnterBuilding(this);
                OnBuildingEnter();
                break;
            case CHARACTER_ACTION.ENTER_GUILD_HALL:
                guild_hall.EnterBuilding(this);
                OnBuildingEnter();
                break;
            case CHARACTER_ACTION.BLACKSMITH:
                blacksmith_bubble.SetActive(true);
                break;
            case CHARACTER_ACTION.TYPE_ACTION:
                anim.SetBool("type_action", true);
                action_bubble.SetActive(true);
                break;
            default:
                break;
        }
    }

    public void StopAction()
    {
        switch (current_action)
        {
            case CHARACTER_ACTION.ENTER_TAVERN:
                tavern.RequestExit(this);
                break;
            case CHARACTER_ACTION.ENTER_GUILD_HALL:
                guild_hall.RequestExit(this);
                break;
            case CHARACTER_ACTION.BLACKSMITH:
                blacksmith_bubble.SetActive(false);
                break;
            case CHARACTER_ACTION.TYPE_ACTION:
                anim.SetBool("type_action", false);
                action_bubble.SetActive(false);
                break;
            default:
                break;
        }
    }

    public void OnBuildingExit()
    {
        Disappear(false);
        steer.enabled = true;
    }

    public void OnBuildingEnter()
    {
        Disappear(true);
        steer.enabled = false;
    }

    void Disappear(bool mode)
    {
        if(mode)
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

    void ChangeAction()
    {
        switch (type)
        {
            case CHARACTER_TYPE.NONE:
                break;
            case CHARACTER_TYPE.KNIGHT:
                WarriorsRoutine();
                break;
            case CHARACTER_TYPE.HUNTER:
                HuntersRoutine();
                break;
            case CHARACTER_TYPE.MAGE:
                MagesRoutine();
                break;
            default:
                break;
        }
    }

    void MagesRoutine()
    {
        switch (time.GetHour())
        {
            case 6:
                steer.CreatePath(locations[(int)LOCATION_TYPE.TABERN].transform.position);
                StopAction();
                current_action = CHARACTER_ACTION.ENTER_TAVERN;
                // go tabern
                break;
            case 8:
                steer.CreatePath(locations[(int)LOCATION_TYPE.CLASS].transform.position);
                StopAction();
                current_action = CHARACTER_ACTION.TYPE_ACTION;
                // go blacksmith
                break;
            case 11:
                steer.CreatePath(locations[(int)LOCATION_TYPE.BLACKSMITH].transform.position);
                StopAction();
                current_action = CHARACTER_ACTION.BLACKSMITH;
                //go train
                break;
            case 14:
                steer.CreatePath(locations[(int)LOCATION_TYPE.TABERN].transform.position);
                StopAction();
                current_action = CHARACTER_ACTION.ENTER_TAVERN;
                // go tabern
                break;
            case 16:
                steer.CreatePath(locations[(int)LOCATION_TYPE.CLASS].transform.position);
                StopAction();
                current_action = CHARACTER_ACTION.TYPE_ACTION;
                // go train
                break;
            case 19:
                steer.CreatePath(locations[(int)LOCATION_TYPE.TABERN].transform.position);
                StopAction();
                current_action = CHARACTER_ACTION.ENTER_TAVERN;
                //go tabern
                break;
            case 22:
                steer.CreatePath(locations[(int)LOCATION_TYPE.GUILD_HALL].transform.position);
                StopAction();
                current_action = CHARACTER_ACTION.ENTER_GUILD_HALL;
                //go sleep
                break;
        }
    }

    void WarriorsRoutine()
    {
        switch (time.GetHour())
        {
            case 6:
                steer.CreatePath(locations[(int)LOCATION_TYPE.TABERN].transform.position);
                StopAction();
                current_action = CHARACTER_ACTION.ENTER_TAVERN;
                // go tabern
                break;
            case 9:
                steer.CreatePath(locations[(int)LOCATION_TYPE.CLASS].transform.position);
                StopAction();
                current_action = CHARACTER_ACTION.TYPE_ACTION;
                // go blacksmith
                break;
            case 13:
                steer.CreatePath(locations[(int)LOCATION_TYPE.TABERN].transform.position);
                StopAction();
                current_action = CHARACTER_ACTION.ENTER_TAVERN;
                // go tabern
                break;
            case 15:
                steer.CreatePath(locations[(int)LOCATION_TYPE.CLASS].transform.position);
                StopAction();
                current_action = CHARACTER_ACTION.TYPE_ACTION;
                // go train
                break;
            case 19:
                steer.CreatePath(locations[(int)LOCATION_TYPE.BLACKSMITH].transform.position);
                StopAction();
                current_action = CHARACTER_ACTION.BLACKSMITH;
                // go blacksmith
                break;
            case 21:
                steer.CreatePath(locations[(int)LOCATION_TYPE.TABERN].transform.position);
                StopAction();
                current_action = CHARACTER_ACTION.ENTER_TAVERN;
                //go tabern
                break;
            case 23:
                steer.CreatePath(locations[(int)LOCATION_TYPE.GUILD_HALL].transform.position);
                StopAction();
                current_action = CHARACTER_ACTION.ENTER_GUILD_HALL;
                //go sleep
                break;
        }
    }

    void HuntersRoutine()
    {
        switch (time.GetHour())
        {
            case 6:
                steer.CreatePath(locations[(int)LOCATION_TYPE.TABERN].transform.position);
                StopAction();
                current_action = CHARACTER_ACTION.ENTER_TAVERN;
                // go tabern
                break;
            case 8:
                steer.CreatePath(locations[(int)LOCATION_TYPE.BLACKSMITH].transform.position);
                StopAction();
                current_action = CHARACTER_ACTION.BLACKSMITH;
                // go blacksmith
                break;
            case 10:
                steer.CreatePath(locations[(int)LOCATION_TYPE.CLASS].transform.position);
                StopAction();
                current_action = CHARACTER_ACTION.TYPE_ACTION;
                //go train
                break;
            case 13:
                steer.CreatePath(locations[(int)LOCATION_TYPE.TABERN].transform.position);
                StopAction();
                current_action = CHARACTER_ACTION.ENTER_TAVERN;
                // go tabern
                break;
            case 17:
                steer.CreatePath(locations[(int)LOCATION_TYPE.CLASS].transform.position);
                StopAction();
                current_action = CHARACTER_ACTION.TYPE_ACTION;
                Disappear(false);
                // go train
                break;
            case 21:
                steer.CreatePath(locations[(int)LOCATION_TYPE.TABERN].transform.position);
                StopAction();
                current_action = CHARACTER_ACTION.ENTER_TAVERN;
                //go tabern
                break;
            case 23:
                steer.CreatePath(locations[(int)LOCATION_TYPE.GUILD_HALL].transform.position);
                StopAction();
                current_action = CHARACTER_ACTION.ENTER_GUILD_HALL;
                //go sleep
                break;
        }
    }

}
