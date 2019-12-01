using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightMember : Member
{
    [Header("Knight")]
    public bool dueling = false;
    public KnightMember opponent;
    public static List<KnightMember> free_members = new List<KnightMember>();

    override public void GenerateInfo()
    {
        base.GenerateInfo();
        member_name = "Marcos";
        type = MEMBER_TYPE.KNIGHT;
    }

    override public void ChangeState(MEMBER_STATE state, bool force = false)
    {
        base.ChangeState(state, force);

        free_members.Remove(this);

        if(dueling && opponent)
        {
            dueling = false;
            opponent.dueling = false;

            opponent.opponent = null;
            opponent = null;
        }
    }
}
