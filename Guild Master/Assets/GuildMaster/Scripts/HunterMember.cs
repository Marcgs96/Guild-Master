using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HunterMember : Member
{
    [Header("Hunter")]
    public GameObject target_animal;
    public AnimalSpawner spawner;
    public GameObject bow;

    List<string> names = new List<string> { "Yapo","Hawkeye","Rexxar","Windrunner","Hemet","Alejandro",
        "Nathanos","Robin","Arrow","Hanzo","Rowan","Rulf","Twitch"};

    override public void GenerateInfo()
    {
        base.GenerateInfo();
        member_name = names[Random.Range(0, names.Count)];
        type = MEMBER_TYPE.HUNTER;
    }

    public override void ChangeState(MEMBER_STATE state, bool force = false)
    {
        base.ChangeState(state, force);

        if (target_animal != null)
            DespawnAnimal();


        bow.SetActive(false);
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
        target_animal = null;
    }
}
