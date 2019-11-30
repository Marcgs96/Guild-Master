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
        member_name = "Huntard";
        type = MEMBER_TYPE.HUNTER;

        production_time = GameManager.manager.time.InGameHoursToSeconds(production_time) / 2; //2 is steps required for 1 meat (track beast, gather meat).
    }

    public void RequestAnimal()
    {
        target_animal = spawner.SpawnAnimal(this);
    }

    public void KillAnimal()
    {
        target_animal.GetComponent<Animator>().SetBool("dead", true);
    }

    public void DespawnAnimal()
    {
        target_animal.GetComponent<Animator>().SetBool("despawn", true);
    }
}
