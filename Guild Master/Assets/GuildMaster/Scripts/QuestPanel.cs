using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestPanel : MonoBehaviour
{
    public GameObject quests_list;
    public GameObject quest_preparation;
    public GameObject quest_inventory_resources;
    public GameObject quest_provisions_resources;

    public GameObject quest_listing_prefab;
    public Text quest_success;

    Dictionary<Quest, GameObject> quest_listings;
    Dictionary<Enemy, GameObject> enemy_slots;

    public void Init()
    {
        quest_listings = new Dictionary<Quest, GameObject>();
        enemy_slots = new Dictionary<Enemy, GameObject>();

        QuestManager.OnQuestAdd += CreateQuestListing;
    }

    internal void ClearListings()
    {
        foreach (KeyValuePair<Quest, GameObject> quest in quest_listings)
        {
            Destroy(quest.Value);
        }
        quest_listings.Clear();

        CloseQuestPreparation();
    }

    internal void OnQuestSelection(Quest new_quest)
    {
        quest_preparation.SetActive(true);

        SetupQuestPreparation(new_quest);
        GameManager.manager.quests.selected_quest = new_quest;

        quests_list.SetActive(false);
    }

    internal void CloseQuestPreparation()
    {
        if (GameManager.manager.quests.selected_quest != null)
        {
            GameManager.manager.quests.selected_quest.Reset();
        }
        Transform send_button = quest_preparation.transform.GetChild(1).GetChild(2);
        send_button.GetComponent<Button>().interactable = false;

        quest_preparation.SetActive(false);
        quests_list.SetActive(true);
    }

    //Quest list
    public void CreateQuestListing(Quest new_quest)
    {
        GameObject new_listing = Instantiate(quest_listing_prefab);
        new_listing.transform.GetChild(0).GetComponent<Text>().text = new_quest.lvl.ToString();
        new_listing.transform.GetChild(1).GetComponent<Text>().text = new_quest.quest_name;

        Transform rewards = new_listing.transform.GetChild(2);
        foreach (Resource resource in new_quest.rewards)
        {
            GameObject new_resource = Instantiate(GameManager.manager.ui.resource_prefab);
            new_resource.GetComponent<RawImage>().texture = GameManager.manager.ui.resource_images[(int)resource.GetResourceType()];
            new_resource.transform.GetChild(0).GetComponent<Text>().text = resource.GetAmount().ToString();
            new_resource.transform.SetParent(rewards);
        }
        new_listing.GetComponent<Button>().onClick.AddListener(delegate { OnQuestSelection(new_quest); });
        new_listing.transform.SetParent(quests_list.transform);

        quest_listings.Add(new_quest, new_listing);
    }

    void RemoveQuestListing(Quest old_quest)
    {
        Destroy(quest_listings[old_quest]);
        quest_listings.Remove(old_quest);
    }

    //QUEST PREPARATION
    void SetupQuestPreparation(Quest new_quest)
    {
        //Info
        Transform info = quest_preparation.transform.GetChild(0);
        info.GetChild(0).GetComponent<Text>().text = new_quest.lvl.ToString();
        info.GetChild(1).GetComponent<Text>().text = new_quest.quest_name;

        //Resources Info
        quest_provisions_resources.transform.GetChild(0).GetComponentInChildren<Text>().text = GameManager.manager.resources.potions.GetAmount().ToString();
        quest_inventory_resources.transform.GetChild(0).GetComponentInChildren<Text>().text = new_quest.provisions[0].GetAmount().ToString();
        quest_provisions_resources.transform.GetChild(1).GetComponentInChildren<Text>().text = GameManager.manager.resources.meat.GetAmount().ToString();
        quest_inventory_resources.transform.GetChild(1).GetComponentInChildren<Text>().text = new_quest.provisions[1].GetAmount().ToString();
        quest_provisions_resources.transform.GetChild(2).GetComponentInChildren<Text>().text = GameManager.manager.resources.flames.GetAmount().ToString();
        quest_inventory_resources.transform.GetChild(2).GetComponentInChildren<Text>().text = new_quest.provisions[2].GetAmount().ToString();

        //Enemies
        foreach (KeyValuePair<Enemy, GameObject> enemy in enemy_slots)
        {
            Destroy(enemy.Value);
        }
        enemy_slots.Clear();

        Transform enemies = quest_preparation.transform.GetChild(2).GetChild(0);
        foreach (Enemy enemy in new_quest.enemies)
        {
            GameObject new_enemy = Instantiate(GameManager.manager.ui.slot_prefab);

            RawImage enemy_image = new_enemy.transform.GetChild(0).GetComponent<RawImage>();
            enemy_image.enabled = true;
            enemy_image.texture = GameManager.manager.ui.portraits[(int)(enemy.type + (int)Member.MEMBER_TYPE.TOTAL)];

            new_enemy.transform.GetComponentInChildren<Text>().text = enemy.name;

            new_enemy.transform.SetParent(enemies);

            enemy_slots.Add(enemy, new_enemy);
        }

        //Members
        Transform members = quest_preparation.transform.GetChild(2).GetChild(1);
        for (int i = 0; i < members.childCount; i++)
        {
            Destroy(members.GetChild(i).gameObject);
        }

        int size = (int)new_quest.size;
        for (int i = 0; i < size; i++)
        {
            GameObject new_member_slot = Instantiate(GameManager.manager.ui.slot_prefab);
            GameObject member_image_go = new_member_slot.transform.GetChild(0).gameObject;
            member_image_go.AddComponent<Button>();

            new_member_slot.transform.SetParent(members);
        }

        //Rewards
        Transform rewards = quest_preparation.transform.GetChild(1).GetChild(1);
        for (int i = 0; i < rewards.childCount; i++)
        {
            Destroy(rewards.GetChild(i).gameObject);
        }
        foreach (Resource resource in new_quest.rewards)
        {
            GameObject new_resource = Instantiate(GameManager.manager.ui.resource_prefab);
            new_resource.GetComponent<RawImage>().texture = GameManager.manager.ui.resource_images[(int)resource.GetResourceType()];
            new_resource.transform.GetChild(0).GetComponent<Text>().text = resource.GetAmount().ToString();
            new_resource.transform.SetParent(rewards);
        }
        quest_preparation.transform.GetChild(1).GetChild(3).GetChild(0).GetComponent<Text>().text = new_quest.bonus_reward.GetAmount().ToString();

        UpdateSuccess(0);
    }

    internal void UpdateSuccess(int total_success)
    {
        quest_success.text = total_success.ToString() + "%";
    }

    internal void AddMemberToQuest(Member member)
    {
        if (quest_preparation.activeSelf && !GameManager.manager.quests.IsInParty(member))
            AddMember(member);        
    }

    void AddMember(Member selected_member)
    {
        if (GameManager.manager.quests.IsOnActiveQuest(selected_member))
            return;

        Transform members = quest_preparation.transform.GetChild(2).GetChild(1);
        for (int i = 0; i < members.childCount; i++)
        {
            RawImage member_image = members.GetChild(i).GetChild(0).GetComponent<RawImage>();
            if (member_image.enabled)
                continue;
            else
            {
                member_image.enabled = true;
                member_image.texture = GameManager.manager.ui.portraits[(int)selected_member.type];
                members.GetChild(i).GetComponentInChildren<Text>().text = selected_member.member_name;

                GameObject member_image_go = members.GetChild(i).transform.GetChild(0).gameObject;
                Button member_slot_button = member_image_go.GetComponent<Button>();
                member_slot_button.onClick.RemoveAllListeners();
                member_slot_button.onClick.AddListener(delegate { OnSlotClick(members.GetChild(i).gameObject, selected_member); });

                GameManager.manager.quests.AddMemberToQuest(selected_member);
                if (GameManager.manager.quests.selected_quest.IsFull())
                {
                    Transform send_button = quest_preparation.transform.GetChild(1).GetChild(2);
                    send_button.GetComponent<Button>().interactable = true;
                }

                return;
            }
        }
    }

    internal void OnEnemyCounter(Enemy enemy, bool state)
    {
        enemy_slots[enemy].transform.GetChild(2).gameObject.SetActive(state);
    }

    public void OnSlotClick(GameObject slot, Member member)
    {
        slot.transform.GetChild(0).GetComponent<RawImage>().enabled = false;
        slot.GetComponentInChildren<Text>().text = "";
        GameManager.manager.quests.RemoveMemberFromQuest(member);

        Transform send_button = quest_preparation.transform.GetChild(1).GetChild(2);
        send_button.GetComponent<Button>().interactable = false;
    }

    public void SendParty()
    {
        SendQuestResources();
        RemoveQuestListing(GameManager.manager.quests.selected_quest);
        GameManager.manager.quests.StartQuest();
        CloseQuestPreparation();
    }

    //QUEST PREPARATION RESOURCE MANAGEMENT
    public void IncreaseResourceInInventory(int type)
    {
        int resource_value;
        Resource.ResourceType enum_type = (Resource.ResourceType)type;


        switch (enum_type)
        {
            case Resource.ResourceType.Potion:
                if (GameManager.manager.resources.potions.GetAmount() == 0)
                    return;
                GameManager.manager.quests.selected_quest.AddResource(enum_type, 1);
                resource_value = GameManager.manager.resources.potions.GetAmount() - GameManager.manager.quests.selected_quest.provisions[0].GetAmount();
                quest_provisions_resources.transform.GetChild(0).GetComponentInChildren<Text>().text = resource_value.ToString();
                quest_inventory_resources.transform.GetChild(0).GetComponentInChildren<Text>().text = GameManager.manager.quests.selected_quest.provisions[0].GetAmount().ToString();
                break;
            case Resource.ResourceType.Meat:
                if (GameManager.manager.resources.meat.GetAmount() == 0)
                    return;
                GameManager.manager.quests.selected_quest.AddResource(enum_type, 1);
                resource_value = GameManager.manager.resources.meat.GetAmount() - GameManager.manager.quests.selected_quest.provisions[1].GetAmount();
                quest_provisions_resources.transform.GetChild(1).GetComponentInChildren<Text>().text = resource_value.ToString();
                quest_inventory_resources.transform.GetChild(1).GetComponentInChildren<Text>().text = GameManager.manager.quests.selected_quest.provisions[1].GetAmount().ToString();
                break;
            case Resource.ResourceType.Flame:
                if (GameManager.manager.resources.flames.GetAmount() == 0)
                    return;
                GameManager.manager.quests.selected_quest.AddResource(enum_type, 1);
                resource_value = GameManager.manager.resources.flames.GetAmount() - GameManager.manager.quests.selected_quest.provisions[2].GetAmount();
                quest_provisions_resources.transform.GetChild(2).GetComponentInChildren<Text>().text = resource_value.ToString();
                quest_inventory_resources.transform.GetChild(2).GetComponentInChildren<Text>().text = GameManager.manager.quests.selected_quest.provisions[2].GetAmount().ToString();
                break;
        }
    }

    public void IncreaseResourceInResources(int type)
    {
        int resource_value;
        Resource.ResourceType enum_type = (Resource.ResourceType)type;
        GameManager.manager.quests.selected_quest.RemoveResource(enum_type, 1);

        switch (enum_type)
        {
            case Resource.ResourceType.Potion:
                resource_value = GameManager.manager.resources.potions.GetAmount() - GameManager.manager.quests.selected_quest.provisions[0].GetAmount();
                quest_provisions_resources.transform.GetChild(0).GetComponentInChildren<Text>().text = resource_value.ToString();
                quest_inventory_resources.transform.GetChild(0).GetComponentInChildren<Text>().text = GameManager.manager.quests.selected_quest.provisions[0].GetAmount().ToString();
                break;
            case Resource.ResourceType.Meat:
                resource_value = GameManager.manager.resources.meat.GetAmount() - GameManager.manager.quests.selected_quest.provisions[1].GetAmount();
                quest_provisions_resources.transform.GetChild(1).GetComponentInChildren<Text>().text = resource_value.ToString();
                quest_inventory_resources.transform.GetChild(1).GetComponentInChildren<Text>().text = GameManager.manager.quests.selected_quest.provisions[1].GetAmount().ToString();
                break;
            case Resource.ResourceType.Flame:
                resource_value = GameManager.manager.resources.flames.GetAmount() - GameManager.manager.quests.selected_quest.provisions[2].GetAmount();
                quest_provisions_resources.transform.GetChild(2).GetComponentInChildren<Text>().text = resource_value.ToString();
                quest_inventory_resources.transform.GetChild(2).GetComponentInChildren<Text>().text = GameManager.manager.quests.selected_quest.provisions[2].GetAmount().ToString();
                break;
        }
    }

    public void SendQuestResources()
    {
        GameManager.manager.resources.DecreaseResource(Resource.ResourceType.Potion, GameManager.manager.quests.selected_quest.provisions[0].GetAmount());
        GameManager.manager.resources.DecreaseResource(Resource.ResourceType.Meat, GameManager.manager.quests.selected_quest.provisions[1].GetAmount());
        GameManager.manager.resources.DecreaseResource(Resource.ResourceType.Flame, GameManager.manager.quests.selected_quest.provisions[2].GetAmount());
    }

    private void OnDisable()
    {
        if(quest_preparation.activeSelf)
            CloseQuestPreparation();
    }
}