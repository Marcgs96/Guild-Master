using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightMember : Member
{
    public bool go_duel = false;
    public KnightMember opponent;

    override public void GenerateInfo()
    {
        base.GenerateInfo();
        info.name = "Marcos";
        info.type = MEMBER_TYPE.KNIGHT;
    }
}
