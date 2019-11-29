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

    public void RequestAnimal()
    {
        target_animal = spawner.SpawnAnimal(this);
    }

    public void KillAnimal()
    {
        target_animal.GetComponent<Animator>().SetBool("dead", true);
    }
}
