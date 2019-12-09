using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest
{
    public enum QuestType { BOUNTY, ADVENTURE};
    public enum QuestSize { ONE = 1, THREE = 3, FIVE = 5, TEN = 10};

    public string quest_name;
    public uint lvl;
    public QuestType type;
    public QuestSize size;
    public List<Enemy> enemies;
    public List<Resource> rewards;
    public List<Member> party;
    public List<Resource> provisions;
    float total_success = 0;
    float resources_success;
    float members_success;
    float enemies_success;

    /// <summary>
    /// Quest duration in game hours
    /// </summary>
    public uint quest_duration;
    private float total_stamina_cost;
    private List<float> stamina_ratios;
    private IEnumerator coroutine;
    private uint members_inside_dungeon = 0;
    private float real_time_duration;

    public Quest(QuestSize size, uint level)
    {
        this.size = size;
        lvl = level;
        type = (QuestType) UnityEngine.Random.Range(0, 2);
        coroutine = QuestActivity();

        enemies = new List<Enemy>();
        rewards = new List<Resource>();
        provisions = new List<Resource>();
        provisions.Add(new Resource(Resource.ResourceType.Potion, 0));
        provisions.Add(new Resource(Resource.ResourceType.Meat, 0));
        provisions.Add(new Resource(Resource.ResourceType.Flame, 0));

        party = new List<Member>();

        for (int i = 0; i < (int)size; i++)
        {
            GenerateEnemy();
        }
        switch (size)
        {
            case QuestSize.ONE:
                quest_duration = 3;
                total_stamina_cost = 50.0f;
                quest_name = "One Man " + GetTypeString();

                rewards.Add(new Resource(Resource.ResourceType.Gold, 100 * (int)level));
                break;
            case QuestSize.THREE:
                quest_duration = 5;
                total_stamina_cost = 60.0f;
                quest_name = "Three Man " + GetTypeString();

                rewards.Add(new Resource(Resource.ResourceType.Gold, 100 * (int)level));
                rewards.Add(new Resource((Resource.ResourceType)UnityEngine.Random.Range(3,5), 1 * (int)level));
                break;
            case QuestSize.FIVE:
                quest_duration = 7;
                total_stamina_cost = 70.0f;
                quest_name = "Five Man " + GetTypeString();

                rewards.Add(new Resource(Resource.ResourceType.Gold, 100 * (int)level));
                rewards.Add(new Resource(Resource.ResourceType.Crown, 1 * (int)level));
                rewards.Add(new Resource(Resource.ResourceType.Shield, 1 * (int)level));
                break;
            case QuestSize.TEN:
                quest_duration = 9;
                total_stamina_cost = 75.0f;
                break;
        }
    }

    private string GetTypeString()
    {
        return type == QuestType.ADVENTURE ? "Adventure" : "Bounty";
    }

    internal void Reset()
    {
        party.Clear();

        foreach(Resource provision in provisions)
        {
            provision.SetAmount(0);
        }

        foreach (Enemy enemy in enemies)
        {
            enemy.countered = false;
            enemy.counterer = null;
        }

        members_success = 0;
        resources_success = 0;
        enemies_success = 0;
        total_success = 0;
    }

    internal void RemoveMemberFromParty(Member old_member)
    {
        party.Remove(old_member);

        float lvl_multiplier = (float)old_member.lvl / (float)lvl;
        float member_value = (float)40 / (float)size;
        members_success -= lvl_multiplier * member_value;

        UnCounterEnemy(old_member);

        total_success = members_success + enemies_success + resources_success;
        GameManager.manager.ui.quest_panel.UpdateSuccess((int)total_success);
    }

    internal void AddMemberToParty(Member new_member)
    {
        party.Add(new_member);

        float lvl_multiplier = (float)new_member.lvl / (float)lvl;
        float member_value = (float)40 / (float)size;
        members_success += lvl_multiplier * member_value;

        CounterEnemy(new_member);

        total_success = members_success + enemies_success + resources_success;
        GameManager.manager.ui.quest_panel.UpdateSuccess((int)total_success);
    }

    private void CounterEnemy(Member counterer)
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i].type == (Enemy.EnemyType)((int)counterer.type))
            {
                if (!enemies[i].countered)
                {
                    enemies[i].countered = true;
                    enemies[i].counterer = counterer;
                    enemies_success += (float)40 / (float)size;

                    GameManager.manager.ui.quest_panel.OnEnemyCounter(enemies[i], true);
                    return;
                }
            }
        }
    }

    private void UnCounterEnemy(Member member)
    {
        foreach (Enemy enemy in enemies)
        {
            if (enemy.counterer == member && !CheckPartyCounters(enemy))
            {
                enemy.countered = false;
                enemy.counterer = null;
                enemies_success -= (float)40 / (float)size;

                GameManager.manager.ui.quest_panel.OnEnemyCounter(enemy, false);
                return;
            }
        }
    }

    internal bool CheckPartyCounters(Enemy enemy)
    {
        foreach (Member m in party)
        {
            if (enemy.type == (Enemy.EnemyType)((int)m.type))
            {
                bool candidate = true;

                foreach (Enemy e in enemies)
                {
                    if (e.counterer == m)
                    {
                        candidate = false;
                        break;
                    }
                }
                if (candidate)
                {
                    enemy.counterer = m;
                    return true;
                }
            }
        }

        return false;
    }

    internal void OnDungeonEnter(Member member)
    {
        if (!party.Contains(member))
            return;

        members_inside_dungeon++;
        if (members_inside_dungeon == party.Count)
            GameManager.manager.quests.StartCoroutine(coroutine);
    }

    private void GenerateEnemy()
    {
        Enemy.EnemyType random_type = (Enemy.EnemyType)UnityEngine.Random.Range((int)Enemy.EnemyType.SKELETON, (int)Enemy.EnemyType.TOTAL);
        enemies.Add(new Enemy(random_type, lvl));
    }

    public bool IsFull()
    {
        if (party.Count == enemies.Count)
            return true;
        else return false;
    }

    public void SendParty()
    {
        stamina_ratios = new List<float>();
        real_time_duration = GameManager.manager.time.InGameHoursToSeconds(quest_duration);

        foreach (Member member in party)
        {
            member.ChangeState(Member.MEMBER_STATE.QUEST);
            stamina_ratios.Add(total_stamina_cost / real_time_duration);
        }
    }

    private IEnumerator QuestActivity()
    {
        Debug.Log("Starting quest activity of " + quest_name);
        float elapsed_time = 0.0f;

        while (elapsed_time < real_time_duration)
        {
            elapsed_time += Time.deltaTime;

            for (int i = 0; i < party.Count; i++)
            {
                //Decrease stamina basic
                party[i].DecreaseStamina(stamina_ratios[i] * Time.deltaTime);

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
            survivors.Add(member);
        }

        if(survivors.Count > 0) //if at least 1 survivor, roll mission success chance
        {
            receive_rewards = UnityEngine.Random.Range(0, 100) < total_success ? true : false;
            if(receive_rewards)
            {
                foreach (Resource resource in rewards)
                    GameManager.manager.resources.IncreaseResource(resource.GetResourceType(), resource.GetAmount());
            }
        }

        GameManager.manager.ui.CreateQuestResultPopup(this, receive_rewards, survivors);
        GameManager.manager.quests.RemoveQuest(this);
    }

    internal void AddResource(Resource.ResourceType type, int amount)
    {
        if(this.type == QuestType.ADVENTURE)
        {
            resources_success += (int)(type == Resource.ResourceType.Meat ? 5/lvl : 1/lvl);
        }
        else
            resources_success += (int)(type == Resource.ResourceType.Flame ? 5 / lvl : 1 / lvl);

        switch (type)
        {
            case Resource.ResourceType.Potion:
                provisions[0].Increase(amount);
                break;
            case Resource.ResourceType.Meat:
                provisions[1].Increase(amount);
                break;
            case Resource.ResourceType.Flame:
                provisions[2].Increase(amount);
                break;
        }

        total_success = members_success + enemies_success + resources_success;
        GameManager.manager.ui.quest_panel.UpdateSuccess((int)total_success);
    }

    internal void RemoveResource(Resource.ResourceType type, int amount)
    {
        bool changed = false;
        switch (type)
        {
            case Resource.ResourceType.Potion:
                if(provisions[0].GetAmount() > 0)
                {
                    provisions[0].Decrease(amount);
                    changed = true;
                }                  
                break;
            case Resource.ResourceType.Meat:
                if (provisions[1].GetAmount() > 0)
                {
                    provisions[1].Decrease(amount);
                    changed = true;
                }
                break;
            case Resource.ResourceType.Flame:
                if (provisions[2].GetAmount() > 0)
                {
                    provisions[2].Decrease(amount);
                    changed = true;
                }
                break;
        }

        if(changed)
        {
            if (this.type == QuestType.ADVENTURE)
                resources_success -= (int)(type == Resource.ResourceType.Meat ? 5 / lvl : 1 / lvl);
            else
                resources_success -= (int)(type == Resource.ResourceType.Flame ? 5 / lvl : 1 / lvl);

            total_success = members_success + enemies_success + resources_success;
            GameManager.manager.ui.quest_panel.UpdateSuccess((int)total_success);
        }
    }
}
