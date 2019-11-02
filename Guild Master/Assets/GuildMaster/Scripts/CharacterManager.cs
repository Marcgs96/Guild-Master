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
    
    public GameObject model;

    enum CHARACTER_ACTION { NONE, DISAPPEAR, TYPE_ACTION, TALK };
    CHARACTER_ACTION current_action;

    public enum CHARACTER_TYPE {NONE, KNIGHT, HUNTER, MAGE};
    public CHARACTER_TYPE type;

    public enum LOCATION_TYPE {TABERN, GUILD_HALL, BLACKSMITH, CLASS};

    // Start is called before the first frame update
    void Start()
    {
        coll = GetComponent<Collider>();
        move = this.GetComponent<Move>();
        anim = this.GetComponent<Animator>();
        
        time = GameObject.Find("GameManager").GetComponent<DayNightCicle>();

        DayNightCicle.OnHourChange += ChangeAction;
        steer = GetComponent<SteeringFollowNavMeshPath>();
        steer.OnReachEnd += DoAction;

        anim.SetInteger("char_type", (int)type);
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
            case CHARACTER_ACTION.DISAPPEAR:
                Disappear(true);
                break;
            case CHARACTER_ACTION.TALK:

                break;
            case CHARACTER_ACTION.TYPE_ACTION:
                anim.SetBool("type_action", true);
                break;
            default:
                break;
        }
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
                current_action = CHARACTER_ACTION.DISAPPEAR;
                Disappear(false);
                // go tabern
                break;
            case 8:
                steer.CreatePath(locations[(int)LOCATION_TYPE.CLASS].transform.position);
                current_action = CHARACTER_ACTION.TYPE_ACTION;
                Disappear(false);
                // go blacksmith
                break;
            case 11:
                steer.CreatePath(locations[(int)LOCATION_TYPE.BLACKSMITH].transform.position);
                current_action = CHARACTER_ACTION.TALK;
                anim.SetBool("type_action", false);
                //go train
                break;
            case 14:
                steer.CreatePath(locations[(int)LOCATION_TYPE.TABERN].transform.position);
                current_action = CHARACTER_ACTION.DISAPPEAR;
                // go tabern
                break;
            case 16:
                steer.CreatePath(locations[(int)LOCATION_TYPE.CLASS].transform.position);
                current_action = CHARACTER_ACTION.TYPE_ACTION;
                Disappear(false);
                // go train
                break;
            case 19:
                steer.CreatePath(locations[(int)LOCATION_TYPE.TABERN].transform.position);
                current_action = CHARACTER_ACTION.DISAPPEAR;
                anim.SetBool("type_action", false);
                //go tabern
                break;
            case 22:
                steer.CreatePath(locations[(int)LOCATION_TYPE.GUILD_HALL].transform.position);
                current_action = CHARACTER_ACTION.DISAPPEAR;
                Disappear(false);
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
                current_action = CHARACTER_ACTION.DISAPPEAR;
                Disappear(false);
                // go tabern
                break;
            case 9:
                steer.CreatePath(locations[(int)LOCATION_TYPE.CLASS].transform.position);
                current_action = CHARACTER_ACTION.TYPE_ACTION;
                Disappear(false);
                // go blacksmith
                break;
            case 13:
                steer.CreatePath(locations[(int)LOCATION_TYPE.TABERN].transform.position);
                current_action = CHARACTER_ACTION.DISAPPEAR;
                anim.SetBool("type_action", false);
                // go tabern
                break;
            case 15:
                steer.CreatePath(locations[(int)LOCATION_TYPE.CLASS].transform.position);
                current_action = CHARACTER_ACTION.TYPE_ACTION;
                Disappear(false);
                // go train
                break;
            case 19:
                steer.CreatePath(locations[(int)LOCATION_TYPE.BLACKSMITH].transform.position);
                current_action = CHARACTER_ACTION.TALK;
                anim.SetBool("type_action", false);
                // go blacksmith
                break;
            case 21:
                steer.CreatePath(locations[(int)LOCATION_TYPE.TABERN].transform.position);
                current_action = CHARACTER_ACTION.DISAPPEAR;
                //go tabern
                break;
            case 23:
                steer.CreatePath(locations[(int)LOCATION_TYPE.GUILD_HALL].transform.position);
                current_action = CHARACTER_ACTION.DISAPPEAR;
                Disappear(false);
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
                current_action = CHARACTER_ACTION.DISAPPEAR;
                Disappear(false);
                // go tabern
                break;
            case 8:
                steer.CreatePath(locations[(int)LOCATION_TYPE.BLACKSMITH].transform.position);
                current_action = CHARACTER_ACTION.TALK;
                Disappear(false);
                // go blacksmith
                break;
            case 10:
                steer.CreatePath(locations[(int)LOCATION_TYPE.CLASS].transform.position);
                current_action = CHARACTER_ACTION.TYPE_ACTION;
                //go train
                break;
            case 13:
                steer.CreatePath(locations[(int)LOCATION_TYPE.TABERN].transform.position);
                current_action = CHARACTER_ACTION.DISAPPEAR;
                anim.SetBool("type_action", false);
                // go tabern
                break;
            case 17:
                steer.CreatePath(locations[(int)LOCATION_TYPE.CLASS].transform.position);
                current_action = CHARACTER_ACTION.TYPE_ACTION;
                Disappear(false);
                // go train
                break;
            case 21:
                steer.CreatePath(locations[(int)LOCATION_TYPE.TABERN].transform.position);
                current_action = CHARACTER_ACTION.DISAPPEAR;
                anim.SetBool("type_action", false);
                //go tabern
                break;
            case 23:
                steer.CreatePath(locations[(int)LOCATION_TYPE.GUILD_HALL].transform.position);
                current_action = CHARACTER_ACTION.DISAPPEAR;
                Disappear(false);
                //go sleep
                break;
        }
    }

}
