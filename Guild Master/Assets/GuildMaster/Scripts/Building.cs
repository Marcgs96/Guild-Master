using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    static uint MAX_LVL = 6;
    static uint START_LVL = 1;

    Queue<Member> release_queue;
    List<Member> members_inside;

    uint level = START_LVL;

    [System.Serializable]
    public struct LevelCost //Wrapper for serialization of array of lists. This should be read from JSON but no time for it yet.
    {
        public List<Resource> resources;
    }
    [SerializeField]
    LevelCost[] level_costs;

    public delegate void BuildingLevel();
    public event BuildingLevel OnLevelUp;

    public float exit_time = 0.5f;
    float exit_timer = 0.0f;
    public GameObject bubble;
    public AudioSource sound;

    void Awake()
    {
        members_inside = new List<Member>();
        release_queue = new Queue<Member>();
    }


    // Update is called once per frame
    void Update()
    {
        if(release_queue.Count > 0)
        {
            exit_timer += Time.deltaTime;
            if (exit_timer >= exit_time)
            {
                Member temp_agent = release_queue.Dequeue();
                ReleaseAgent(temp_agent);

                exit_timer = 0.0f;
            }
        }
    }

    public void EnterBuilding(Member agent)
    {
        members_inside.Add(agent);
        OnMemberInteraction();
    }

    public void RequestExit(Member agent)
    {
        foreach (Member a in members_inside)
        {
            if(a == agent)
            {
                release_queue.Enqueue(a);
            }
        }
    }

    void ReleaseAgent(Member agent)
    {
        members_inside.Remove(agent);
        agent.OnBuildingExit();
        OnMemberInteraction();
    }

    void OnMemberInteraction()
    {
        if(bubble) bubble.SetActive(members_inside.Count > 0 ? true : false);
        if(sound) sound.enabled = members_inside.Count > 0 ? true : false;
    }

    virtual public void LevelUp()
    {
        if (level < MAX_LVL && HaveResources())
        {
            DecreaseResources();
            level++;
            OnLevelUp?.Invoke();
        }
    }

    bool HaveResources()
    {
        foreach (Resource resource in level_costs[level-1].resources)
        {
            if (!GameManager.manager.resources.HaveAmount(resource.GetResourceType(), resource.GetAmount()))
                return false;
        }

        return true;
    }

    void DecreaseResources()
    {
        foreach (Resource resource in level_costs[level-1].resources)
        {
            GameManager.manager.resources.DecreaseResource(resource.GetResourceType(), resource.GetAmount());
        }
    }

    public List<Resource> GetResourcesCost()
    {
        List<Resource> total_resources = new List<Resource>();
        foreach (Resource resource in level_costs[level-1].resources)
        {
            total_resources.Add(resource);
        }
                
        return total_resources;
    }
}
