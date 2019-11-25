using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    [SerializeField]
    List<Quest> quests;

    public enum EnemyType { SKELETON, BANDIT, ORC, TOTAL };
    public enum QuestType { BOUNTY, DUNGEON, RAID };

    public struct QuestEnemy
    {
        public string name;
        public uint lvl;
        public EnemyType type;
    }

    public struct Quest
    {
        public string name;
        public uint lvl;
        public QuestType type;
        public QuestEnemy[] enemies;
        public ResourceManager.ResourceType[] rewards;
        public uint xp;
    }

    void CreateQuest(QuestType type)
    {
        switch (type)
        {
            case QuestType.BOUNTY:
                Quest quest;
                quest.type = type;
                quest.name = "Bounty";
                quest.lvl = 1;

                //Todo: Create function for enemy creation.
                QuestEnemy enemy;
                enemy.name = "Bobo";
                enemy.lvl = 1;
                enemy.type = (EnemyType) Random.Range((int)EnemyType.SKELETON, (int)EnemyType.TOTAL);
                break;
            case QuestType.DUNGEON:
                //Todo
                break;
            case QuestType.RAID:
                //todo
                break;
        }
    }

    void CleanQuests()
    {

    }

    void GenerateQuests()
    {

    }
}
