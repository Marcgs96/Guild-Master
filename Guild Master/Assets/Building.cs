using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    Queue<CharacterManager> release_queue;

    List<CharacterManager> agents;

    public GameObject bubble;

    public float exit_time = 0.5f;
    float exit_timer = 0.0f;

    void Awake()
    {
        agents = new List<CharacterManager>();
        release_queue = new Queue<CharacterManager>();
    }


    // Update is called once per frame
    void Update()
    {
        if(release_queue.Count > 0)
        {
            exit_timer += Time.deltaTime;
            if (exit_timer >= exit_time)
            {
                CharacterManager temp_agent = release_queue.Dequeue();
                ReleaseAgent(temp_agent);

                exit_timer = 0.0f;
            }
        }
    }

    public void EnterBuilding(CharacterManager agent)
    {
        agents.Add(agent);
        BubbleCheck();
    }

    public void RequestExit(CharacterManager agent)
    {
        foreach (CharacterManager a in agents)
        {
            if(a == agent)
            {
                release_queue.Enqueue(a);
            }
        }
    }

    void ReleaseAgent(CharacterManager agent)
    {
        agents.Remove(agent);
        agent.OnBuildingExit();
        BubbleCheck();
    }

    void BubbleCheck()
    {
        bubble.SetActive(agents.Count > 0 ? true : false);
    }
}
