using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject guild_panel;
    public GameObject blacksmith_panel;


    //quests panel stuff
    public GameObject quests_panel;
    public GameObject quests_list;
    public GameObject quest_preparation;
    public GameObject quest_listing;
    List<GameObject> quest_listings;

    //members list stuff
    public GameObject member_info_panel;
    public GameObject members_list_panel;
    public GameObject member_listing;
    List<GameObject> member_listings;
    Member selected_member;

    //other prefabs
    public GameObject resource_prefab;
    public GameObject slot_prefab;

    void Awake()
    {
        MemberManager.OnMemberAdd += CreateMemberListing;
        QuestManager.OnQuestAdd += CreateQuestListing;
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

    public void OnQuestSelection(QuestManager.Quest new_quest)
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
        quest_preparation.SetActive(false);
        quests_list.SetActive(true);
    }

    public void CloseMemberInfoPanel()
    {
        member_info_panel.SetActive(false);
    }

    public void OnMemberClick(Member member)
    {
        if (quest_preparation.activeSelf && !GameManager.manager.quests.IsInParty(member))
            AddMemberToQuest(member);
        else
        {
            ShowMemberInfo(member);
        }
    }

    public void OnSlotClick(GameObject slot, Member member)
    {
        slot.transform.GetChild(0).GetComponent<Image>().enabled = false;
        slot.GetComponentInChildren<Text>().text = "";
        GameManager.manager.quests.RemoveMemberFromQuest(member);

        Transform send_button = quest_preparation.transform.GetChild(1).GetChild(2);
        send_button.GetComponent<Button>().interactable = false;
    }

    public void SendParty()
    {
        GameManager.manager.quests.StartQuest();
    }
    #endregion

    void CreateQuestListing(QuestManager.Quest new_quest)
    {
        GameObject new_listing = Instantiate(quest_listing);
        new_listing.transform.GetChild(0).GetComponent<Text>().text = new_quest.lvl.ToString();
        new_listing.transform.GetChild(1).GetComponent<Text>().text = new_quest.name;

        Transform rewards = new_listing.transform.GetChild(2);
        foreach (KeyValuePair<ResourceManager.ResourceType, uint> entry in new_quest.rewards)
        {
            GameObject new_resource = Instantiate(resource_prefab);
            //Todo: select image depending on resource type
            //new_resource.GetComponent<Image>().image = IMAGE;
            new_resource.transform.GetChild(0).GetComponent<Text>().text = entry.Value.ToString();
            new_resource.transform.SetParent(rewards);
        }
        new_listing.GetComponent<Button>().onClick.AddListener(delegate { OnQuestSelection(new_quest); });
        new_listing.transform.SetParent(quests_list.transform);
    }

    void CreateMemberListing(Member new_member)
    {
        GameObject new_listing = Instantiate(member_listing);
        Member.MemberInfo info = new_member.GetInfo();
        new_listing.transform.GetChild(1).GetComponent<Text>().text = info.name;

        new_listing.GetComponent<Button>().onClick.AddListener(delegate { OnMemberClick(new_member); });
        new_listing.transform.SetParent(members_list_panel.transform);
    }

    void SetupQuestPreparation(QuestManager.Quest new_quest)
    {
        //Info
        Transform info = quest_preparation.transform.GetChild(0);
        info.GetChild(0).GetComponent<Text>().text = new_quest.lvl.ToString();
        info.GetChild(1).GetComponent<Text>().text = new_quest.name;

        //Enemies
        Transform enemies = quest_preparation.transform.GetChild(2).GetChild(0);
        for (int i = 0; i < enemies.childCount; i++)
        {
            Destroy(enemies.GetChild(i).gameObject);
        }
        foreach (QuestManager.QuestEnemy enemy in new_quest.enemies)
        {
            GameObject new_enemy = Instantiate(slot_prefab);

            Image enemy_image = new_enemy.transform.GetChild(0).GetComponent<Image>();
            enemy_image.enabled = true;
            //Todo: select image depending on enemy type
            //enemy_image.image = IMAGE;

            new_enemy.transform.GetComponentInChildren<Text>().text = enemy.name;

            new_enemy.transform.SetParent(enemies);
        }

        //Members
        Transform members = quest_preparation.transform.GetChild(2).GetChild(1);
        for (int i = 0; i < members.childCount; i++)
        {
            Destroy(members.GetChild(i).gameObject);
        }
        uint size = GameManager.manager.quests.GetMemberSizeFromType(new_quest.type);
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
        foreach (KeyValuePair<ResourceManager.ResourceType, uint> entry in new_quest.rewards)
        {
            GameObject new_resource = Instantiate(resource_prefab);
            //Todo: select image depending on resource type
            //new_resource.GetComponent<Image>().image = IMAGE;
            new_resource.transform.GetChild(0).GetComponent<Text>().text = entry.Value.ToString();
            new_resource.transform.SetParent(rewards);
        }
    }

    void AddMemberToQuest(Member selected_member)
    {
        Transform members = quest_preparation.transform.GetChild(2).GetChild(1);
        for (int i = 0; i < members.childCount; i++)
        {
            Image member_image = members.GetChild(i).GetChild(0).GetComponent<Image>();
            if (member_image.enabled)
                continue;
            else
            {
                member_image.enabled = true;
                //Todo: set member image depending on member type
                //member_image.image = IMAGE;
                members.GetChild(i).GetComponentInChildren<Text>().text = selected_member.GetInfo().name;

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

    void ShowMemberInfo(Member member)
    {
        member_info_panel.SetActive(true);

        selected_member = member;
        Member.MemberInfo info = member.GetInfo();
        //Header
        Transform header = member_info_panel.transform.GetChild(0);
        header.GetComponentInChildren<Text>().text = info.name;
        //setup image
        //setup slider

        //Info
        Transform info_panel = member_info_panel.transform.GetChild(1);
        info_panel.GetChild(0).GetChild(0).GetComponent<Text>().text = info.lvl.ToString();
        info_panel.GetChild(1).GetChild(0).GetComponent<Text>().text = info.equipment_lvl.ToString();
        info_panel.GetChild(2).GetChild(0).GetComponent<Text>().text = info.GetTypeString();
        info_panel.GetChild(3).GetChild(0).GetComponent<Text>().text = info.xp.ToString();
        info_panel.GetChild(4).GetChild(0).GetComponent<Text>().text = member.GetStateString();

        //Buttons
    }
}
