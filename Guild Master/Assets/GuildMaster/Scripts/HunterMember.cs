using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HunterMember : Member
{
    override public void GenerateInfo()
    {
        info.name = "Huntard";
        info.lvl = 1;
        info.xp = 0;
        info.type = MEMBER_TYPE.HUNTER;
    }
    override protected void ChangeAction(uint hour)
    {
        switch (hour)
        {
            case 6:
                steer.CreatePath(GameManager.manager.locations[(int)GameManager.LOCATION_TYPE.TAVERN].transform.position);
                StopAction();
                current_action = CHARACTER_ACTION.ENTER_TAVERN;
                // go tabern
                break;
            case 8:
                steer.CreatePath(GameManager.manager.locations[(int)GameManager.LOCATION_TYPE.BLACKSMITH].transform.position);
                StopAction();
                current_action = CHARACTER_ACTION.BLACKSMITH;
                // go blacksmith
                break;
            case 10:
                steer.CreatePath(GameManager.manager.locations[(int)GameManager.LOCATION_TYPE.HUNTER_LOCATION].transform.position);
                StopAction();
                current_action = CHARACTER_ACTION.TYPE_ACTION;
                //go train
                break;
            case 13:
                steer.CreatePath(GameManager.manager.locations[(int)GameManager.LOCATION_TYPE.TAVERN].transform.position);
                StopAction();
                current_action = CHARACTER_ACTION.ENTER_TAVERN;
                // go tabern
                break;
            case 17:
                steer.CreatePath(GameManager.manager.locations[(int)GameManager.LOCATION_TYPE.HUNTER_LOCATION].transform.position);
                StopAction();
                current_action = CHARACTER_ACTION.TYPE_ACTION;
                Disappear(false);
                // go train
                break;
            case 20:
                steer.CreatePath(GameManager.manager.locations[(int)GameManager.LOCATION_TYPE.TAVERN].transform.position);
                StopAction();
                current_action = CHARACTER_ACTION.ENTER_TAVERN;
                //go tabern
                break;
            case 23:
                steer.CreatePath(GameManager.manager.locations[(int)GameManager.LOCATION_TYPE.GUILD_HALL].transform.position);
                StopAction();
                current_action = CHARACTER_ACTION.ENTER_GUILD_HALL;
                //go sleep
                break;
        }
    }

    override protected string GetTypeActionString()
    {
        return "Hunting";
    }
}
