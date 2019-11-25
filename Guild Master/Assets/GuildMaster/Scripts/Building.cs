using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    Queue<Member> release_queue;

    List<Member> agents;

    public GameObject bubble;
    public AudioSource sound;

    public float exit_time = 0.5f;
    float exit_timer = 0.0f;

    void Awake()
    {
        agents = new List<Member>();
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
        agents.Add(agent);
        BubbleCheck();
    }

    public void RequestExit(Member agent)
    {
        foreach (Member a in agents)
        {
            if(a == agent)
            {
                release_queue.Enqueue(a);
            }
        }
    }

    void ReleaseAgent(Member agent)
    {
        agents.Remove(agent);
        agent.OnBuildingExit();
        BubbleCheck();
    }

    void BubbleCheck()
    {
        bubble.SetActive(agents.Count > 0 ? true : false);
        sound.enabled = agents.Count > 0 ? true : false;
    }
}
