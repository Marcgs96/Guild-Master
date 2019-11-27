using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AnimalSpawner: MonoBehaviour
{
    public GameObject animal_prefab;

    internal GameObject SpawnAnimal(Member member)
    {
        Vector3 animal_position;

        animal_position = UnityEngine.Random.insideUnitSphere;
        animal_position *= member.wander.radius;
        animal_position += member.transform.position;
        animal_position.y = member.transform.position.y;

        NavMeshHit hit;
        NavMesh.SamplePosition(animal_position, out hit, float.PositiveInfinity, (1 << NavMesh.GetAreaFromName("OffRoad")));

        GameObject spawned_animal = Instantiate(animal_prefab, hit.position, Quaternion.identity);

        return spawned_animal;
    }
}
