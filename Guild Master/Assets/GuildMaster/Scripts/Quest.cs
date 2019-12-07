using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest
{
    public enum QuestType { BOUNTY, ADVENTURE, RAID };

    public string quest_name;
    public uint lvl;
    public uint xp;
    public QuestType type;
    public List<Enemy> enemies;
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

        enemies = new List<Enemy>();
        rewards = new List<Resource>();
        provisions = new List<Resource>();
        provisions.Add(new Resource(Resource.ResourceType.Potion, 0));
        provisions.Add(new Resource(Resource.ResourceType.Meat, 0));
        provisions.Add(new Resource(Resource.ResourceType.Flame, 0));

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

    internal void ResetCounters()
    {
        foreach (Enemy enemy in enemies)
        {
            enemy.countered = false;
        }
    }

    internal void RemoveMemberFromParty(Member old_member)
    {
        party.Remove(old_member);
        CheckEnemyCounter(old_member, false);
    }

    internal void AddMemberToParty(Member new_member)
    {
        party.Add(new_member);
        CheckEnemyCounter(new_member, true);
    }

    private void CheckEnemyCounter(Member new_member, bool counter)
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i].type == (Enemy.EnemyType)((int)new_member.type))
            {
                if(counter && !enemies[i].countered)
                {
                    enemies[i].countered = counter;
                    GameManager.manager.ui.quest_panel.OnEnemyCounter(enemies[i], counter);
                    return;
                }
                else if(!counter && enemies[i].countered)
                {
                    enemies[i].countered = counter;
                    GameManager.manager.ui.quest_panel.OnEnemyCounter(enemies[i], counter);
                    return;
                }           
            }
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
            GenerateEnemy();
        }

        //Todo: Create function for reward setup to be semi randomized
        rewards.Add(new Resource(Resource.ResourceType.Gold, 500));
        rewards.Add(new Resource(Resource.ResourceType.Shield, 10));
        rewards.Add(new Resource(Resource.ResourceType.Crown, 10));
    }

    private void GenerateEnemy()
    {
        Enemy.EnemyType random_type = (Enemy.EnemyType)UnityEngine.Random.Range((int)Enemy.EnemyType.SKELETON, (int)Enemy.EnemyType.TOTAL);
        enemies.Add(new Enemy(random_type, lvl));
    }

    private void CreateBounty()
    {
        quest_name = "Bounty"; //Todo: Create function for name randomization
        xp = 200;
        quest_duration = 3;
        total_stamina_cost = 50.0f;

        GenerateEnemy();

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
        Debug.Log("Starting quest activity of " + quest_name);
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
        bool receive_rewards = false;
        List<Member> survivors = new List<Member>();

        foreach (Member member in party)
        {
            if (member.stamina == 0)
            {
                if (UnityEngine.Random.Range(0, 3) > 0) //Roll for chance to die
                {
                    Debug.Log(member.member_name + " couldn't make it out alive.");
                    GameManager.manager.buildings[(int)Building.BUILDING_TYPE.DUNGEON].RemoveMember(member);
                    GameManager.manager.members.RemoveMember(member);
                    continue;
                }
            }

            member.ChangeState(Member.MEMBER_STATE.REST, true);
            receive_rewards = true;
            survivors.Add(member);
        }

        if(receive_rewards) //if at least 1 survivor, receive rewards
        {
            foreach (Resource resource in rewards)
                GameManager.manager.resources.IncreaseResource(resource.GetResourceType(), resource.GetAmount());
        }

        GameManager.manager.ui.CreateQuestResultPopup(this, receive_rewards, survivors);
        GameManager.manager.quests.RemoveQuest(this);
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
