using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest
{
    public enum EnemyType { SKELETON, BANDIT, ORC, TOTAL };
    public enum QuestType { BOUNTY, ADVENTURE, RAID };
    public struct QuestEnemy
    {
        public string name;
        public uint lvl;
        public EnemyType type;
    }

    public string quest_name;
    public uint lvl;
    public uint xp;
    public QuestType type;
    public List<QuestEnemy> enemies;
    public List<Resource> rewards;
    public List<Member> party;
    public List<Resource> provisions;
    /// <summary>
    /// Quest duration in game hours
    /// </summary>
    public uint quest_duration;
    private float total_stamina_cost;
    private List<float> stamina_ratios;
    private uint total_food_checks = 1;
    private IEnumerator coroutine;
    private uint members_inside_dungeon = 0;
    private float real_time_duration;

    public Quest(QuestType type, uint level)
    {
        this.type = type;
        lvl = level;
        coroutine = QuestActivity();

        enemies = new List<QuestEnemy>();
        rewards = new List<Resource>();
        provisions = new List<Resource>();
        provisions.Add(new Resource(Resource.ResourceType.Potion, 0));
        provisions.Add(new Resource(Resource.ResourceType.Meat, 0));

        party = new List<Member>();

        switch (type)
        {
            case QuestType.BOUNTY:
                CreateBounty();
                break;
            case QuestType.ADVENTURE:
                CreateAdventure();
                break;
            case QuestType.RAID:
                //todo
                break;
        }
    }

    internal void OnDungeonEnter(Member member)
    {
        if (!party.Contains(member))
            return;

        members_inside_dungeon++;
        if (members_inside_dungeon == party.Count)
            GameManager.manager.quests.StartCoroutine(coroutine);
    }

    private void CreateAdventure()
    {
        quest_name = "Adventure"; //Todo: Create function for name randomization
        xp = 500;
        quest_duration = 5;
        total_stamina_cost = 75.0f;

        for (int i = 0; i < GetMemberSizeFromType(type); i++)
        {
            enemies.Add(CreateEnemy(lvl));
        }

        //Todo: Create function for reward setup to be semi randomized
        rewards.Add(new Resource(Resource.ResourceType.Gold, 500));
        rewards.Add(new Resource(Resource.ResourceType.Shield, 10));
        rewards.Add(new Resource(Resource.ResourceType.Crown, 10));
    }

    private void CreateBounty()
    {
        quest_name = "Bounty"; //Todo: Create function for name randomization
        xp = 200;
        quest_duration = 3;
        total_stamina_cost = 50.0f;

        enemies.Add(CreateEnemy(lvl));

        //Todo: Create function for reward setup
        rewards.Add(new Resource(Resource.ResourceType.Gold, 250));
        rewards.Add(new Resource(Resource.ResourceType.Shield, 5));
    }

    public void SendParty()
    {
        stamina_ratios = new List<float>();
        real_time_duration = GameManager.manager.time.InGameHoursToSeconds(quest_duration);

        foreach (Member member in party)
        {
            member.ChangeState(Member.MEMBER_STATE.QUEST);
            stamina_ratios.Add(total_stamina_cost / real_time_duration); //Todo: Makes this be affected by quest diffculty, member lvl, and member equipment lvl.
        }
    }

    private IEnumerator QuestActivity()
    {
        Debug.Log("Starting quest activity of" + quest_name);
        float food_check_chance = 0;
        bool food_check = false;
        float elapsed_time = 0.0f;

        while (elapsed_time < real_time_duration)
        {
            elapsed_time += Time.deltaTime;

            if (total_food_checks > 0) //Roll for a food check if thers any left
            {
                int roll = UnityEngine.Random.Range((int)food_check_chance, 100);
                if (roll == 100)
                {
                    Debug.Log("Food check");
                    food_check = true;
                    food_check_chance = 0;
                }
            }

            for (int i = 0; i < party.Count; i++)
            {
                //Decrease stamina basic
                party[i].DecreaseStamina(stamina_ratios[i] * Time.deltaTime);

                if (food_check) //Eat food if there is any left otherwise lose stamina
                {              
                    foreach (Resource provision in provisions)
                    {
                        if (provision.GetResourceType() == Resource.ResourceType.Meat && provision.GetAmount() > 0)
                        {
                            Debug.Log(party[i].member_name + " ate a piece of meat.");
                            provision.Decrease(1);
                        }
                        else
                        {
                            Debug.Log(party[i].member_name + " couldn't eat, loses 25 stamina.");
                            party[i].DecreaseStamina(25);
                        }
                    }
                }

                //If stamina too low, try to recover with a potion
                if (party[i].stamina < 25)
                {
                    foreach (Resource provision in provisions)
                    {
                        if (provision.GetResourceType() == Resource.ResourceType.Potion && provision.GetAmount() > 0)
                        {
                            Debug.Log(party[i].member_name + " used a potion.");
                            provision.Decrease(1);
                            party[i].IncreaseStamina(50);
                        }
                    }
                }
            }

            if (food_check) //Reset food check
            {
                total_food_checks--;
                food_check = false;
            }

            food_check_chance += 0.5f * Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }

        FinishQuest();
    }

    private void FinishQuest()
    {
        foreach (Member member in party)
        {
            if (member.stamina == 0)
            {
                if (UnityEngine.Random.Range(1, 2) == 1)
                {
                    //Todo: Kill member/Remove from game
                    Debug.Log(member.member_name + " couldn't make it out alive.");
                    continue;
                }
                else
                    Debug.Log(member.member_name + " escaped death.");
            }
            //give xp to member
            member.ChangeState(Member.MEMBER_STATE.REST, true);
        }

        foreach (Resource resource in rewards)
        {
            GameManager.manager.resources.IncreaseResource(resource.GetResourceType(), resource.GetAmount());
        }

        GameManager.manager.quests.RemoveQuest(this);
    }

    QuestEnemy CreateEnemy(uint lvl)
    {
        //Todo: improve enemy creation related with quest type and difficulty.
        QuestEnemy enemy;
        enemy.name = "Bobo";
        enemy.lvl = lvl;
        enemy.type = (EnemyType)UnityEngine.Random.Range((int)EnemyType.SKELETON, (int)EnemyType.TOTAL);

        return enemy;
    }

    public uint GetMemberSizeFromType(QuestType type)
    {
        switch (type)
        {
            case QuestType.BOUNTY:
                return 1;
            case QuestType.ADVENTURE:
                return 3;
            case QuestType.RAID:
                return 10;
        }

        return 1;
    }
}
