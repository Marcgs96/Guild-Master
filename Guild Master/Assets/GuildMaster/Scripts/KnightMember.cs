using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightMember : Member
{
    override public void GenerateInfo()
    {
        base.GenerateInfo();
        info.name = "Marcos";
        info.type = MEMBER_TYPE.KNIGHT;
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
