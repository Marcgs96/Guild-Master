using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightMember : Member
{
    [Header("Knight")]
    public bool dueling = false;
    public KnightMember opponent;
    public static List<KnightMember> free_members = new List<KnightMember>();

    List<string> names = new List<string> { "Byron","Tybalt","Uther","Arthas","Jarvan",
        "Quinn","Leo","Doran","Tristan","Arthur","Marcos","Quijote","Tirant","Geralt","Tirion"};

    override public void GenerateInfo()
    {
        base.GenerateInfo();
        member_name = names[UnityEngine.Random.Range(0, names.Count)];
        type = MEMBER_TYPE.KNIGHT;
    }

    override public void ChangeState(MEMBER_STATE state, bool force = false)
    {
        if (this.state == MEMBER_STATE.WORK)
        {
            if (dueling && opponent)
            {
                GameManager.manager.locations.ReleasePosition(assigned_position.transform.parent.gameObject);
                GameManager.manager.locations.ReleasePosition(opponent.assigned_position.transform.parent.gameObject);

                dueling = false;
                opponent.dueling = false;

                opponent.opponent = null;
                opponent = null;

            }
            else
                free_members.Remove(this);
        }

        base.ChangeState(state, force);
    }
}
