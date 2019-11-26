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
        CreateQuest(QuestType.BOUNTY, 1);
        CreateQuest(QuestType.DUNGEON, 2);
    }

    void CreateQuest(QuestType type, uint lvl)
    {
        Quest quest = new Quest();
        quest.type = type;
        quest.lvl = lvl;

        quest.enemies = new List<QuestEnemy>();
        quest.rewards = new Dictionary<ResourceManager.ResourceType, uint>();

        switch (type)
        {
            case QuestType.BOUNTY:
                quest.name = "Bounty";
                quest.xp = 200;

                quest.enemies.Add(CreateEnemy(lvl));

                //Todo: Create function for reward setup
                quest.rewards.Add(ResourceManager.ResourceType.Gold, 250);
                quest.rewards.Add(ResourceManager.ResourceType.Crown, 5);

                break;
            case QuestType.DUNGEON:
                quest.name = "Dungeon";
                quest.xp = 500;

                for(int i = 0; i < GetMemberSizeFromType(type); i++)
                {
                    quest.enemies.Add(CreateEnemy(lvl));
                }

                //Todo: Create function for reward setup
                quest.rewards.Add(ResourceManager.ResourceType.Gold, 250);
                quest.rewards.Add(ResourceManager.ResourceType.Crown, 5);
                quest.rewards.Add(ResourceManager.ResourceType.Shield, 5);

                break;
            case QuestType.RAID:
                //todo
                break;
        }

        OnQuestAdd?.Invoke(quest);
    }

    QuestEnemy CreateEnemy(uint lvl)
    {
        //Todo: improve enemy creation related with quest type and difficulty.
        QuestEnemy enemy;
        enemy.name = "Bobo";
        enemy.lvl = lvl;
        enemy.type = (EnemyType)Random.Range((int)EnemyType.SKELETON, (int)EnemyType.TOTAL);

        return enemy;
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

    public bool IsInParty(Member member)
    {
        return quest_party.Contains(member);
    }

    public void StartQuest()
    {
        Debug.Log("Starting Quest: " + selected_quest.name);

        foreach (Member member in quest_party)
        {
            member.ChangeState(Member.MEMBER_STATE.QUEST);
        }
    }

    void FinishQuest()
    {

    }
}
