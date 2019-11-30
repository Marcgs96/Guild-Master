using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageMember : Member
{
    override public void GenerateInfo()
    {
        base.GenerateInfo();
        member_name = "Magox";
        type = MEMBER_TYPE.MAGE;

        production_time = GameManager.manager.time.InGameHoursToSeconds(production_time) / 3; //3 is steps required for 1 potion (recipe,materials,alchemy)
    }
}
