using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightMember : Member
{
    override public void GenerateInfo()
    {
        info.name = "Marcos";
        info.lvl = 1;
        info.xp = 0;
        info.type = MEMBER_TYPE.KNIGHT;
    }
    protected override void ChangeAction(uint hour)
    {
        switch (hour)
        {
            case 6:
                steer.CreatePath(GameManager.manager.locations[(int)GameManager.LOCATION_TYPE.TAVERN].transform.position);
                StopAction();
                current_action = CHARACTER_ACTION.ENTER_TAVERN;
                // go tavern
                break;
            case 9:
                steer.CreatePath(GameManager.manager.locations[(int)GameManager.LOCATION_TYPE.KNIGHT_LOCATION].transform.position);
                StopAction();
                current_action = CHARACTER_ACTION.TYPE_ACTION;
                // go blacksmith
                break;
            case 13:
                steer.CreatePath(GameManager.manager.locations[(int)GameManager.LOCATION_TYPE.TAVERN].transform.position);
                StopAction();
                current_action = CHARACTER_ACTION.ENTER_TAVERN;
                // go tabern
                break;
            case 15:
                steer.CreatePath(GameManager.manager.locations[(int)GameManager.LOCATION_TYPE.KNIGHT_LOCATION].transform.position);
                StopAction();
                current_action = CHARACTER_ACTION.TYPE_ACTION;
                // go train
                break;
            case 19:
                steer.CreatePath(GameManager.manager.locations[(int)GameManager.LOCATION_TYPE.BLACKSMITH].transform.position);
                StopAction();
                current_action = CHARACTER_ACTION.BLACKSMITH;
                // go blacksmith
                break;
            case 21:
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
}
