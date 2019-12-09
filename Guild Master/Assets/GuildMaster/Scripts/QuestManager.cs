using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    [SerializeField]
    List<Quest> quests;
    [SerializeField]
    List<Quest> active_quests;
    [SerializeField]
    public Quest selected_quest;

    public delegate void QuestAdded(Quest new_quest);
    public static event QuestAdded OnQuestAdd;

    void Start()
    {
        active_quests = new List<Quest>();
        quests = new List<Quest>();
        DayNightCicle.OnDayChange += GenerateQuests;

        GenerateQuests(true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            GameManager.manager.time.day++;
            GenerateQuests(true);
        }
    }

    void CleanQuests()
    {
        selected_quest = null;

        GameManager.manager.ui.quest_panel.ClearListings();
        quests.Clear();
    }

    void GenerateQuests(bool night)
    {
        CleanQuests();
        // Todo: Create random quests depending of set parameteres like highest lvl member, amount of members, etc. 
        // Right now we have set quest size and levels for progression curve finishing on day 5.
        switch (GameManager.manager.time.day)
        {
            case 1:
                CreateQuest(Quest.QuestSize.ONE, 1);
                CreateQuest(Quest.QuestSize.ONE, 1);
                CreateQuest(Quest.QuestSize.ONE, 1);
                CreateQuest(Quest.QuestSize.THREE, 1);
                CreateQuest(Quest.QuestSize.THREE, 1);
                break;
            case 2:
                CreateQuest(Quest.QuestSize.ONE, 1);
                CreateQuest(Quest.QuestSize.ONE, 2);
                CreateQuest(Quest.QuestSize.THREE, 2);
                CreateQuest(Quest.QuestSize.THREE, 1);
                CreateQuest(Quest.QuestSize.THREE, 2);
                break;
            case 3:
                CreateQuest(Quest.QuestSize.ONE, 3);
                CreateQuest(Quest.QuestSize.THREE, 2);
                CreateQuest(Quest.QuestSize.THREE, 3);
                CreateQuest(Quest.QuestSize.THREE, 2);
                CreateQuest(Quest.QuestSize.FIVE, 3);
                break;
            case 4:
                CreateQuest(Quest.QuestSize.ONE, 4);
                CreateQuest(Quest.QuestSize.THREE, 3);
                CreateQuest(Quest.QuestSize.THREE, 4);
                CreateQuest(Quest.QuestSize.THREE, 5);
                CreateQuest(Quest.QuestSize.FIVE, 4);
                break;
            case 5:
                CreateQuest(Quest.QuestSize.TEN, 6);
                CreateQuest(Quest.QuestSize.ONE, 5);
                CreateQuest(Quest.QuestSize.FIVE, 5);
                CreateQuest(Quest.QuestSize.THREE, 4);
                CreateQuest(Quest.QuestSize.THREE, 5);
                break;
        }
    }

    void CreateQuest(Quest.QuestSize type, uint lvl)
    {
        Quest quest = new Quest(type, lvl);
        quests.Add(quest);
        OnQuestAdd?.Invoke(quest);
    }

    public void SelectQuest(Quest new_quest)
    {
        selected_quest = new_quest;
    }

    internal void ResetCounters()
    {
        selected_quest.Reset();
    }

    public void AddMemberToQuest(Member new_member)
    {
        selected_quest.AddMemberToParty(new_member);
    }

    public void RemoveMemberFromQuest(Member old_member)
    {
        selected_quest.RemoveMemberFromParty(old_member);
    }

    public bool IsInParty(Member member)
    {
        return selected_quest.party.Contains(member);
    }

    public void StartQuest()
    {
        Debug.Log("Starting Quest: " + selected_quest.quest_name);
        active_quests.Add(selected_quest);
        quests.Remove(selected_quest);
        selected_quest.SendParty();

        selected_quest = null;
    }

    internal void OnDungeonEnter(Member member)
    {
        foreach (Quest quest in active_quests)
        {
            quest.OnDungeonEnter(member);
        }
    }

    internal void RemoveQuest(Quest quest)
    {
        active_quests.Remove(quest);
    }

    internal Quest GetSelectedQuest()
    {
        return selected_quest;
    }

    public bool IsOnActiveQuest(Member member)
    {
        foreach(Quest q in active_quests)
        {
            foreach (Member m in q.party)
            {
                if (m == member) return true;
            }
        }

        return false;
    }
}
