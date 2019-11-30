using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightMember : Member
{
    public bool dueling = false;
    public KnightMember opponent;

    private float production_progress = 0.0f;

    override public void GenerateInfo()
    {
        base.GenerateInfo();
        member_name = "Marcos";
        type = MEMBER_TYPE.KNIGHT;

        production_time = GameManager.manager.time.InGameHoursToSeconds(production_time);
    }

    protected override void MemberUpdate()
    {
        base.MemberUpdate();

        if (producing)
        {
            Debug.Log("pepega producer " + production_progress);
            production_progress += Time.deltaTime;
            if(production_progress >= production_time)
            {
                GameManager.manager.resources.IncreaseResource(Resource.ResourceType.Flame, 1);
                production_progress = 0;
            }
        }
                     
    }

    override public void ChangeState(MEMBER_STATE state, bool force = false)
    {
        base.ChangeState(state, force);

        if(dueling && opponent)
        {
            dueling = false;
            opponent.dueling = false;

            opponent.opponent = null;
            opponent = null;
        }
    }
}
