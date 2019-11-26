using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageMember : Member
{
    override public void GenerateInfo()
    {
        info.name = "Magox";
        info.lvl = 1;
        info.xp = 0;
        info.type = MEMBER_TYPE.MAGE;
    }
    protected override void ChangeAction(uint hour)
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
                steer.CreatePath(GameManager.manager.locations[(int)GameManager.LOCATION_TYPE.MAGE_LOCATION].transform.position);
                StopAction();
                current_action = CHARACTER_ACTION.TYPE_ACTION;
                // go blacksmith
                break;
            case 11:
                steer.CreatePath(GameManager.manager.locations[(int)GameManager.LOCATION_TYPE.BLACKSMITH].transform.position);
                StopAction();
                current_action = CHARACTER_ACTION.BLACKSMITH;
                //go train
                break;
            case 14:
                steer.CreatePath(GameManager.manager.locations[(int)GameManager.LOCATION_TYPE.TAVERN].transform.position);
                StopAction();
                current_action = CHARACTER_ACTION.ENTER_TAVERN;
                // go tabern
                break;
            case 16:
                steer.CreatePath(GameManager.manager.locations[(int)GameManager.LOCATION_TYPE.MAGE_LOCATION].transform.position);
                StopAction();
                current_action = CHARACTER_ACTION.TYPE_ACTION;
                // go train
                break;
            case 19:
                steer.CreatePath(GameManager.manager.locations[(int)GameManager.LOCATION_TYPE.TAVERN].transform.position);
                StopAction();
                current_action = CHARACTER_ACTION.ENTER_TAVERN;
                //go tabern
                break;
            case 22:
                steer.CreatePath(GameManager.manager.locations[(int)GameManager.LOCATION_TYPE.GUILD_HALL].transform.position);
                StopAction();
                current_action = CHARACTER_ACTION.ENTER_GUILD_HALL;
                //go sleep
                break;
        }
    }

    override protected string GetTypeActionString()
    {
        return "Making Potions";
    }
}
