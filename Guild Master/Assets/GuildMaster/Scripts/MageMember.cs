using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageMember : Member
{
    override public void GenerateInfo()
    {
        base.GenerateInfo();
        info.name = "Magox";
        info.type = MEMBER_TYPE.MAGE;
    }
    protected override void ChangeState(uint hour)
    {
        if (state == MEMBER_STATE.QUEST)
            return;

        switch (hour)
        {
            case 21:
                state = MEMBER_STATE.SLEEP;
                break;
        } 
    }
}
