using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    [SerializeField]
    List<Quest> quests;
    [SerializeField]
    List<Member> quest_party;
    Quest selected_quest;


    public enum EnemyType { SKELETON, BANDIT, ORC, TOTAL };
    public enum QuestType { BOUNTY, DUNGEON, RAID };

    public delegate void QuestAdded(Quest new_quest);
    public static event QuestAdded OnQuestAdd;

    public struct QuestEnemy
    {
        public string name;
        public uint lvl;
        public EnemyType type;
    }

    public class Quest
    {
        public string name;
        public uint lvl;
        public uint xp;
        public QuestType type;
        public List<QuestEnemy> enemies;
        public Dictionary<ResourceManager.ResourceType, uint> rewards;
    }


    void Start()
    {
        quests = new List<Quest>();
        quest_party = new List<Member>();
        GenerateQuests();
    }

    void CreateQuest(QuestType type)
    {
        Quest quest = new Quest();
        quest.type = type;
        quest.name = "";
        quest.lvl = 0;
        quest.xp = 0;

        quest.enemies = new List<QuestEnemy>();
        quest.rewards = new Dictionary<ResourceManager.ResourceType, uint>();

        switch (type)
        {
            case QuestType.BOUNTY:
                quest.name = "Bounty";
                quest.lvl = 1;
                quest.xp = 200;

                //Todo: Create function for enemy creation.
                quest.enemies = new List<QuestEnemy>();
                QuestEnemy enemy;
                enemy.name = "Bobo";
                enemy.lvl = 1;
                enemy.type = (EnemyType) Random.Range((int)EnemyType.SKELETON, (int)EnemyType.TOTAL);
                quest.enemies.Add(enemy);

                //Todo: Create function for reward setup
                quest.rewards = new Dictionary<ResourceManager.ResourceType, uint>();
                quest.rewards.Add(ResourceManager.ResourceType.Gold, 250);
                quest.rewards.Add(ResourceManager.ResourceType.Crown, 5);

                break;
            case QuestType.DUNGEON:
                //Todo
                break;
            case QuestType.RAID:
                //todo
                break;
        }

        OnQuestAdd?.Invoke(quest);
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

    void CleanQuests()
    {
        selected_quest = null;
    }

    void GenerateQuests()
    {
        //Todo: Create random quests depending of set parameteres like highest lvl member, amount of members, etc.
        CreateQuest(QuestType.BOUNTY);
        CreateQuest(QuestType.BOUNTY);
    }

    public void SelectQuest(Quest new_quest)
    {
        selected_quest = new_quest;
        quest_party.Clear();
    }

    public void AddMemberToQuest(Member new_member)
    {
        quest_party.Add(new_member);
    }

    public void RemoveMemberFromQuest(Member old_member)
    {
        quest_party.Remove(old_member);
    }

    public void StartQuest()
    {
        Debug.Log("Starting Quest: " + selected_quest.name);
    }
}
