using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// TODO: Split UI Manager into smaller UI sections for each panel. This is getting too big.
public class UIManager : MonoBehaviour
{
    //Guild panel stuff
    public GameObject guild_panel;
    public GameObject guild_upgrade_costs;

    //Blacksmith panel stuff
    public GameObject blacksmith_panel;
    public GameObject blacksmith_upgrade_costs;

    //quests panel stuff
    public GameObject quests_panel;
    public GameObject quests_list;
    public GameObject quest_preparation;
    public GameObject quest_listing;
    Dictionary<Quest, GameObject> quest_listings;
    public GameObject quest_inventory_resources;
    public GameObject quest_provisions_resources;

    public GameObject quest_result_popup;

    //members list stuff
    public GameObject members_list_panel;
    public GameObject member_listing;
    Dictionary<Member, GameObject> member_listings;
    Member selected_member;

    //other prefabs
    public GameObject resource_prefab;
    public GameObject slot_prefab;
    public GameObject resource_cost_prefab;
    public GameObject member_result_prefab;
    public List<Texture2D> portraits;
    public List<Texture2D> resource_images;

    Queue<GameObject> popup_queue;
    GameObject current_popup = null;

    void Awake()
    {
        member_listings = new Dictionary<Member, GameObject>();
        quest_listings = new Dictionary<Quest, GameObject>();
        popup_queue = new Queue<GameObject>();

        MemberManager.OnMemberAdd += CreateMemberListing;
        QuestManager.OnQuestAdd += CreateQuestListing;
        GameManager.manager.buildings[(int)Building.BUILDING_TYPE.GUILD_HOUSE].OnLevelUp += OnGuildHouseLevelUp;
        GameManager.manager.buildings[(int)Building.BUILDING_TYPE.BLACKSMITH].OnLevelUp += OnBlacksmithLevelUp;

        UpdateBlacksmithPanel();
        UpdateGuildHallPanel();
    }

    #region ButtonFuctions
    public void ActivateQuestsPanel()
    {
        quests_panel.SetActive(!quests_panel.activeSelf);
        guild_panel.SetActive(false);
        blacksmith_panel.SetActive(false);
    }
    public void ActivateGuildPanel()
    {
        guild_panel.SetActive(!guild_panel.activeSelf);
        quests_panel.SetActive(false);
        blacksmith_panel.SetActive(false);
    }
    public void ActivateBlacksmithPanel()
    {
        blacksmith_panel.SetActive(!blacksmith_panel.activeSelf);
        guild_panel.SetActive(false);
        quests_panel.SetActive(false);
    }

    internal void RemoveMemberListing(Member old_member)
    {
        Destroy(member_listings[old_member]);
        member_listings.Remove(old_member);
        UpdateMemberCountText();
    }

    public void OnQuestSelection(Quest new_quest)
    {
        quest_preparation.SetActive(true);

        SetupQuestPreparation(new_quest);
        GameManager.manager.quests.SelectQuest(new_quest);

        quests_list.SetActive(false);
    }

    public void CloseQuestsPanel()
    {
        quests_panel.SetActive(false);
    }

    public void CloseGuildPanel()
    {
        guild_panel.SetActive(false);
    }

    public void CloseBlacksmithPanel()
    {
        blacksmith_panel.SetActive(false);
    }

    public void CloseQuestPreparation()
    {
        if(GameManager.manager.quests.selected_quest != null)ResetInventory();
        quest_preparation.SetActive(false);
        quests_list.SetActive(true);
    }

    public void UpgradeBuildingButton(Building building)
    {
        building.LevelUp();
    }

    internal void OnMemberStaminaChange(Member member)
    {
        GameObject listing;
        member_listings.TryGetValue(member, out listing);

        if(listing)
        {
            listing.transform.GetChild(2).GetComponent<Slider>().value = (int)member.stamina;
        }
    }

    public void OnMemberClick(Member member)
    {
        if (quest_preparation.activeSelf && !GameManager.manager.quests.IsInParty(member))
            AddMemberToQuest(member);
        else if (!quest_preparation.activeSelf)
            Camera.main.GetComponent<CameraControls>().SetFocus(member.gameObject);
    }

    public void OnStateClick(Member member)
    {
        if (member.state == Member.MEMBER_STATE.QUEST)
            return;

        if (member.state == Member.MEMBER_STATE.REST)
            member.ChangeState(Member.MEMBER_STATE.WORK);
        else if (member.state == Member.MEMBER_STATE.WORK)
            member.ChangeState(Member.MEMBER_STATE.REST);

        UpdateStateButtonText(member);
    }

    public void OnSlotClick(GameObject slot, Member member)
    {
        slot.transform.GetChild(0).GetComponent<RawImage>().enabled = false;
        slot.GetComponentInChildren<Text>().text = "";
        GameManager.manager.quests.RemoveMemberFromQuest(member);

        Transform send_button = quest_preparation.transform.GetChild(1).GetChild(2);
        send_button.GetComponent<Button>().interactable = false;
    }

    public void RestButton()
    {
        selected_member.ChangeState(Member.MEMBER_STATE.REST);
    }

    public void WorkButton()
    {
        selected_member.ChangeState(Member.MEMBER_STATE.WORK);
    }

    public void SendParty()
    {
        SendQuestResources();
        RemoveQuestListing(GameManager.manager.quests.GetSelectedQuest());
        GameManager.manager.quests.StartQuest();
        CloseQuestPreparation();
    }

    public void KnightRecruitButton()
    {
        GameManager.manager.members.AddMember(Member.MEMBER_TYPE.KNIGHT);
    }
    public void MageRecruitButton()
    {
        GameManager.manager.members.AddMember(Member.MEMBER_TYPE.MAGE);
    }
    public void HunterRecruitButton()
    {
        GameManager.manager.members.AddMember(Member.MEMBER_TYPE.HUNTER);
    }
    #endregion

    void CreateQuestListing(Quest new_quest)
    {
        GameObject new_listing = Instantiate(quest_listing);
        new_listing.transform.GetChild(0).GetComponent<Text>().text = new_quest.lvl.ToString();
        new_listing.transform.GetChild(1).GetComponent<Text>().text = new_quest.quest_name;

        Transform rewards = new_listing.transform.GetChild(2);
        foreach (Resource resource in new_quest.rewards)
        {
            GameObject new_resource = Instantiate(resource_prefab);
            new_resource.GetComponent<RawImage>().texture = resource_images[(int)resource.GetResourceType()];
            new_resource.transform.GetChild(0).GetComponent<Text>().text = resource.GetAmount().ToString();
            new_resource.transform.SetParent(rewards);
        }
        new_listing.GetComponent<Button>().onClick.AddListener(delegate { OnQuestSelection(new_quest); });
        new_listing.transform.SetParent(quests_list.transform);

        quest_listings.Add(new_quest, new_listing);
    }

    void RemoveQuestListing(Quest old_quest)
    {
        GameObject listing;
        quest_listings.TryGetValue(old_quest, out listing);
        quest_listings.Remove(old_quest);

        Destroy(listing);
    }

    void CreateMemberListing(Member new_member)
    {
        GameObject new_listing = Instantiate(member_listing);
        new_listing.transform.GetChild(1).GetComponent<Text>().text = new_member.member_name;
        new_listing.transform.GetChild(0).GetComponent<RawImage>().texture = portraits[(int)new_member.type];
        new_listing.transform.GetChild(3).GetComponent<Text>().text = new_member.action_string;
        new_listing.transform.GetChild(4).GetComponentInChildren<Text>().text = new_member.lvl.ToString();

        new_listing.GetComponent<Button>().onClick.AddListener(delegate { OnMemberClick(new_member); });
        new_listing.transform.GetChild(5).GetComponent<Button>().onClick.AddListener(delegate { OnStateClick(new_member); });
        new_listing.transform.SetParent(members_list_panel.transform.GetChild(1));

        member_listings.Add(new_member, new_listing);

        UpdateMemberCountText();
    }

    internal void CreateQuestResultPopup(Quest quest, bool receive_rewards, List<Member> survivors)
    {
        GameObject popup = Instantiate(quest_result_popup);

        popup.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(OnPopUpClose);

        if (receive_rewards)
        {
            popup.transform.GetChild(0).GetComponentInChildren<Text>().text = "Victory";
            Transform rewards = popup.transform.GetChild(1).GetChild(1);

            foreach (Resource resource in quest.rewards)
            {
                GameObject new_resource = Instantiate(resource_prefab);
                new_resource.GetComponent<RawImage>().texture = resource_images[(int)resource.GetResourceType()];
                new_resource.transform.GetChild(0).GetComponent<Text>().text = resource.GetAmount().ToString();
                new_resource.transform.SetParent(rewards);
            }
        }

        Transform members = popup.transform.GetChild(1).GetChild(0);
        foreach (Member member in quest.party)
        {
            GameObject new_member_result = Instantiate(member_result_prefab);
            new_member_result.transform.GetChild(0).GetComponent<RawImage>().texture = portraits[(int)member.type];
            new_member_result.transform.GetChild(1).GetComponent<Text>().text = member.member_name;
            if (survivors.Contains(member))
                new_member_result.transform.GetChild(2).GetComponent<Text>().text = "Survived the dungeon";
            else
                new_member_result.transform.GetChild(2).GetComponent<Text>().text = "Couldn't make it out alive";

            new_member_result.transform.SetParent(members);
        }
        popup.transform.SetParent(this.transform);
        popup.GetComponent<RectTransform>().localPosition = Vector3.zero;
        popup_queue.Enqueue(popup);
        CheckPopups();
    }

    private void CheckPopups()
    {
        if(current_popup == null && popup_queue.Count > 0)
        {
            GameObject popup = popup_queue.Dequeue();
            current_popup = popup;
            popup.SetActive(true);
        }
    }

    public void OnPopUpClose()
    {
        Destroy(current_popup);
        current_popup = null;
        CheckPopups();
    }

    public void UpdateStateButtonText(Member member)
    {
        if (member.state == Member.MEMBER_STATE.WORK)
            member_listings[member].transform.GetChild(5).GetComponentInChildren<Text>().text = "Work";
        else if (member.state == Member.MEMBER_STATE.REST)
            member_listings[member].transform.GetChild(5).GetComponentInChildren<Text>().text = "Rest";
        else if (member.state == Member.MEMBER_STATE.QUEST)
            member_listings[member].transform.GetChild(5).GetComponentInChildren<Text>().text = "Quest";
    }

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
        Transform enemies = quest_preparation.transform.GetChild(2).GetChild(0);
        for (int i = 0; i < enemies.childCount; i++)
        {
            Destroy(enemies.GetChild(i).gameObject);
        }
        foreach (Quest.QuestEnemy enemy in new_quest.enemies)
        {
            GameObject new_enemy = Instantiate(slot_prefab);

            RawImage enemy_image = new_enemy.transform.GetChild(0).GetComponent<RawImage>();
            enemy_image.enabled = true;
            enemy_image.texture = portraits[(int)(enemy.type + (int)Member.MEMBER_TYPE.TOTAL)];

            new_enemy.transform.GetComponentInChildren<Text>().text = enemy.name;

            new_enemy.transform.SetParent(enemies);
        }

        //Members
        Transform members = quest_preparation.transform.GetChild(2).GetChild(1);
        for (int i = 0; i < members.childCount; i++)
        {
            Destroy(members.GetChild(i).gameObject);
        }

        uint size = new_quest.GetMemberSizeFromType(new_quest.type);
        for(int i = 0; i < size; i++)
        {
            GameObject new_member_slot = Instantiate(slot_prefab);
            GameObject member_image_go = new_member_slot.transform.GetChild(0).gameObject;
            member_image_go.AddComponent<Button>();

            new_member_slot.transform.SetParent(members);
        }

        //Rewards
        Transform rewards = quest_preparation.transform.GetChild(1).GetChild(1);
        for(int i = 0; i<rewards.childCount; i++)
        {
            Destroy(rewards.GetChild(i).gameObject);
        }
        foreach (Resource resource in new_quest.rewards)
        {
            GameObject new_resource = Instantiate(resource_prefab);
            new_resource.GetComponent<RawImage>().texture = resource_images[(int)resource.GetResourceType()];
            new_resource.transform.GetChild(0).GetComponent<Text>().text = resource.GetAmount().ToString();
            new_resource.transform.SetParent(rewards);
        }
    }

    void AddMemberToQuest(Member selected_member)
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
                member_image.texture = portraits[(int)selected_member.type];
                members.GetChild(i).GetComponentInChildren<Text>().text = selected_member.member_name;

                GameObject member_image_go = members.GetChild(i).transform.GetChild(0).gameObject;
                Button member_slot_button = member_image_go.GetComponent<Button>();
                member_slot_button.onClick.RemoveAllListeners();
                member_slot_button.onClick.AddListener(delegate { OnSlotClick(members.GetChild(i).gameObject, selected_member); });

                GameManager.manager.quests.AddMemberToQuest(selected_member);
                if(i == members.childCount - 1)
                {
                    Transform send_button = quest_preparation.transform.GetChild(1).GetChild(2);
                    send_button.GetComponent<Button>().interactable = true;
                }

                return;
            }
        }
    }

    public void MemberActionChange(Member member)
    {
        member_listings[member].transform.GetChild(3).GetComponent<Text>().text = member.action_string;
    }

    public void UpdateMemberCountText()
    {
        string member_count = GameManager.manager.members.GetMemberCount().ToString() + "/" + GameManager.manager.members.GetMemberCap().ToString();
        members_list_panel.transform.GetChild(0).GetComponentInChildren<Text>().text = member_count;
    }

    public void UpdateStateIcon(int state, Member member)
    {
        member_listings[member].transform.GetChild(6).GetComponent<RawImage>().texture = resource_images[state];
    }

    public void OnGuildHouseLevelUp(uint lvl)
    {
        UpdateGuildHallPanel();
    }


    public void UpdateGuildHallPanel()
    {
        Building guild_hall = GameManager.manager.buildings[(int)Building.BUILDING_TYPE.GUILD_HOUSE];
        guild_panel.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = guild_hall.GetLevel().ToString();

        for (int i = 0; i < guild_upgrade_costs.transform.childCount; i++)
        {
            Destroy(guild_upgrade_costs.transform.GetChild(i).gameObject);
        }

        if(!guild_hall.IsMaxLevel())
        {
            List<Resource> resources = guild_hall.GetResourcesCost();
            foreach (Resource resource in resources)
            {
                GameObject resource_go = Instantiate(resource_cost_prefab);
                resource_go.GetComponent<RawImage>().texture = resource_images[(int)resource.GetResourceType()];
                resource_go.transform.GetChild(0).GetComponent<Text>().text = resource.GetAmount().ToString();
                resource_go.transform.SetParent(guild_upgrade_costs.transform);
            }
        }
        else
            guild_upgrade_costs.transform.parent.GetComponent<Button>().enabled = false;
    }

    public void OnBlacksmithLevelUp(uint level)
    {
        UpdateBlacksmithPanel();
    }

    public void UpdateBlacksmithPanel()
    {
        Building blacksmith = GameManager.manager.buildings[(int)Building.BUILDING_TYPE.BLACKSMITH];
        blacksmith_panel.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = blacksmith.GetLevel().ToString();

        for (int i = 0; i < blacksmith_upgrade_costs.transform.childCount; i++)
        {
            Destroy(blacksmith_upgrade_costs.transform.GetChild(i).gameObject);
        }  

        if(!blacksmith.IsMaxLevel())
        {
            List<Resource> resources = blacksmith.GetResourcesCost();
            foreach (Resource resource in resources)
            {
                GameObject resource_go = Instantiate(resource_cost_prefab);
                resource_go.GetComponent<RawImage>().texture = resource_images[(int)resource.GetResourceType()];
                resource_go.transform.GetChild(0).GetComponent<Text>().text = resource.GetAmount().ToString();
                resource_go.transform.SetParent(blacksmith_upgrade_costs.transform);
            }
        }
        else
            blacksmith_upgrade_costs.transform.parent.GetComponent<Button>().enabled = false;
    }

    public void IncreaseResourceInInventory(int type)
    {
        int resource_value;
        Resource.ResourceType enum_type = (Resource.ResourceType)type;

        switch (enum_type)
        {
            case Resource.ResourceType.Potion:
                GameManager.manager.quests.selected_quest.provisions[0].Increase(1);
                resource_value = GameManager.manager.resources.potions.GetAmount() - GameManager.manager.quests.selected_quest.provisions[0].GetAmount();
                quest_provisions_resources.transform.GetChild(0).GetComponentInChildren<Text>().text = resource_value.ToString();
                quest_inventory_resources.transform.GetChild(0).GetComponentInChildren<Text>().text = GameManager.manager.quests.selected_quest.provisions[0].GetAmount().ToString();
                break;
            case Resource.ResourceType.Meat:
                GameManager.manager.quests.selected_quest.provisions[1].Increase(1);
                resource_value = GameManager.manager.resources.meat.GetAmount() - GameManager.manager.quests.selected_quest.provisions[1].GetAmount();
                quest_provisions_resources.transform.GetChild(1).GetComponentInChildren<Text>().text = resource_value.ToString();
                quest_inventory_resources.transform.GetChild(1).GetComponentInChildren<Text>().text = GameManager.manager.quests.selected_quest.provisions[1].GetAmount().ToString();
                break;
            case Resource.ResourceType.Flame:
                GameManager.manager.quests.selected_quest.provisions[2].Increase(1);
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

        switch (enum_type)
        {
            case Resource.ResourceType.Potion:
                GameManager.manager.quests.selected_quest.provisions[0].Decrease(1);
                resource_value = GameManager.manager.resources.potions.GetAmount() - GameManager.manager.quests.selected_quest.provisions[0].GetAmount();
                quest_provisions_resources.transform.GetChild(0).GetComponentInChildren<Text>().text = resource_value.ToString();
                quest_inventory_resources.transform.GetChild(0).GetComponentInChildren<Text>().text = GameManager.manager.quests.selected_quest.provisions[0].GetAmount().ToString();
                break;
            case Resource.ResourceType.Meat:
                GameManager.manager.quests.selected_quest.provisions[1].Decrease(1);
                resource_value = GameManager.manager.resources.meat.GetAmount() - GameManager.manager.quests.selected_quest.provisions[1].GetAmount();
                quest_provisions_resources.transform.GetChild(1).GetComponentInChildren<Text>().text = resource_value.ToString();
                quest_inventory_resources.transform.GetChild(1).GetComponentInChildren<Text>().text = GameManager.manager.quests.selected_quest.provisions[1].GetAmount().ToString();
                break;
            case Resource.ResourceType.Flame:
                GameManager.manager.quests.selected_quest.provisions[2].Decrease(1);
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

    public void ResetInventory()
    {
        GameManager.manager.quests.selected_quest.provisions[0].SetAmount(0);
        GameManager.manager.quests.selected_quest.provisions[1].SetAmount(0);
        GameManager.manager.quests.selected_quest.provisions[2].SetAmount(0);
    }
}
