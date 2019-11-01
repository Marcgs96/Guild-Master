using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorBehaviour : MonoBehaviour
{
    bool training = false;
    DayNightCicle time;
    SteeringFollowNavMeshPath steer;
    CharacterManager char_manager;
    public GameObject locations;
    public GameObject model;

    enum CHARACTER_ACTION
    {
        DISAPPEAR,
        TALK,
        TRAIN,
    };
    CHARACTER_ACTION action;


    // Start is called before the first frame update
    void Start()
    {
        char_manager = GetComponent<CharacterManager>();
        steer = GetComponent<SteeringFollowNavMeshPath>();
        time = GameObject.Find("GameManager").GetComponent<DayNightCicle>();

        DayNightCicle.OnHourChange += ChangeAction;
    }

    // Update is called once per frame
    void Update()
    {
        if(steer.arrived)
        {
            switch (action)
            {
                case CHARACTER_ACTION.DISAPPEAR:
                    model.SetActive(false);
                    break;
                case CHARACTER_ACTION.TALK:

                    break;
                case CHARACTER_ACTION.TRAIN:
                    char_manager.DoAction(true);
                    break;
                default:
                    break;
            }
        }
    }

    void ChangeAction()
    {
        switch (time.GetHour())
        {
            case 9:
                steer.CreatePath(locations.transform.Find("Tabern Location").transform.position);
                action = CHARACTER_ACTION.DISAPPEAR;
                // go tabern
                break;
            case 10:
                steer.CreatePath(locations.transform.Find("Blacksmith Location").transform.position);
                action = CHARACTER_ACTION.TALK;
                model.SetActive(true);
                // go blacksmith
                break;
            case 11:
                steer.CreatePath(locations.transform.Find("Warrior Location").transform.position);
                action = CHARACTER_ACTION.TRAIN;
                //go train
                break;
            case 14:
                steer.CreatePath(locations.transform.Find("Tabern Location").transform.position);
                action = CHARACTER_ACTION.DISAPPEAR;
                char_manager.DoAction(false);
                // go tabern
                break;
            case 15:
                steer.CreatePath(locations.transform.Find("Warrior Location").transform.position);
                action = CHARACTER_ACTION.TRAIN;
                model.SetActive(true);
                // go train
                break;
            case 21:
                steer.CreatePath(locations.transform.Find("Tabern Location").transform.position);
                action = CHARACTER_ACTION.DISAPPEAR;
                char_manager.DoAction(false);
                //go tabern
                break;
            case 22:
                steer.CreatePath(locations.transform.Find("Guild Hall Location").transform.position);
                action = CHARACTER_ACTION.DISAPPEAR;
                //go sleep
                break;
        }
    }
}
