using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HunterMember : Member
{
    public GameObject target_animal;
    public AnimalSpawner spawner;

    override public void GenerateInfo()
    {
        base.GenerateInfo();
        info.name = "Huntard";
        info.type = MEMBER_TYPE.HUNTER;
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

    public void RequestAnimal()
    {
        target_animal = spawner.SpawnAnimal(this);
    }
}
