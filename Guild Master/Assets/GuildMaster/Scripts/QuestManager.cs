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
    Quest selected_quest;

    public delegate void QuestAdded(Quest new_quest);
    public static event QuestAdded OnQuestAdd;

    void Start()
    {
        active_quests = new List<Quest>();
        quests = new List<Quest>();
        GenerateQuests();
    }

    void CleanQuests()
    {
        selected_quest = null;
    }

    void GenerateQuests()
    {
        //Todo: Create random quests depending of set parameteres like highest lvl member, amount of members, etc.
        CreateQuest(Quest.QuestType.BOUNTY, 1);
        CreateQuest(Quest.QuestType.ADVENTURE, 2);
    }

    void CreateQuest(Quest.QuestType type, uint lvl)
    {
        Quest quest = new Quest(type,lvl);
        quests.Add(quest);
        OnQuestAdd?.Invoke(quest);
    }

    public void SelectQuest(Quest new_quest)
    {
        if(selected_quest != null)
            selected_quest.party.Clear();
        selected_quest = new_quest;
    }

    public void AddMemberToQuest(Member new_member)
    {
        selected_quest.party.Add(new_member);
    }

    public void RemoveMemberFromQuest(Member old_member)
    {
        selected_quest.party.Remove(old_member);
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
}
