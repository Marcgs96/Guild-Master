using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest
{
    public enum EnemyType { SKELETON, BANDIT, ORC, TOTAL };
    public enum QuestType { BOUNTY, DUNGEON, RAID };
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
    public uint quest_duration;
    public List<Member> party;
    public List<Resource> provisions;

    private List<uint> stamina_ratios;
    private uint total_food_checks = 1;
    private bool food_check = false;
    private float stamina_check_time = 5.0f;
    private IEnumerator coroutine;
    private uint members_inside_dungeon = 0;

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
            case QuestType.DUNGEON:
                CreateDungeon();
                break;
            case QuestType.RAID:
                //todo
                break;
        }
    }

    private void CreateDungeon()
    {
        quest_name = "Dungeon";
        xp = 500;
        quest_duration = 5;

        for (int i = 0; i < GetMemberSizeFromType(type); i++)
        {
            enemies.Add(CreateEnemy(lvl));
        }

        //Todo: Create function for reward setup
        rewards.Add(new Resource(Resource.ResourceType.Gold, 500));
        rewards.Add(new Resource(Resource.ResourceType.Shield, 10));
        rewards.Add(new Resource(Resource.ResourceType.Crown, 10));
    }

    internal void OnDungeonEnter(Member member)
    {
        if (party.Contains(member))
            members_inside_dungeon++;

        if (members_inside_dungeon == party.Count)
            GameManager.manager.quests.StartCoroutine(coroutine);
    }

    private void CreateBounty()
    {
        quest_name = "Bounty";
        xp = 200;
        quest_duration = 3;

        enemies.Add(CreateEnemy(lvl));

        //Todo: Create function for reward setup
        rewards.Add(new Resource(Resource.ResourceType.Gold, 250));
        rewards.Add(new Resource(Resource.ResourceType.Shield, 5));
    }

    public void SendParty()
    {
        stamina_ratios = new List<uint>();
        foreach (Member member in party)
        {
            member.ChangeState(Member.MEMBER_STATE.QUEST);
            stamina_ratios.Add(10);
        }
    }

    private IEnumerator QuestActivity()
    {
        int food_check_chance = 0;
        yield return new WaitForSeconds(stamina_check_time);

        while (true)
        {
            if (total_food_checks > 0) //Roll for a food check if thers any left
            {
                int roll = UnityEngine.Random.Range(food_check_chance, 100);
                if (roll == 100)
                {
                    food_check = true;
                    food_check_chance = 0;
                }
            }

            for (int i = 0; i < party.Count; i++)
            {
                //Decrease stamina basic
                party[i].DecreaseStamina(stamina_ratios[i]);

                if (food_check) //Eat food if there is any left otherwise lose stamina
                {
                    foreach (Resource provision in provisions)
                    {
                        if (provision.GetResourceType() == Resource.ResourceType.Meat && provision.GetAmount() > 0)
                            provision.Decrease(1);
                        else
                            party[i].DecreaseStamina(25);
                    }
                }

                //If stamina too low, try to recover with a potion
                if (party[i].GetInfo().stamina < 25)
                {
                    foreach (Resource provision in provisions)
                    {
                        if (provision.GetResourceType() == Resource.ResourceType.Potion && provision.GetAmount() > 0)
                        {
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

            yield return new WaitForSeconds(stamina_check_time);
        }
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
            case QuestType.DUNGEON:
                return 5;
            case QuestType.RAID:
                return 10;
        }

        return 1;
    }
}
