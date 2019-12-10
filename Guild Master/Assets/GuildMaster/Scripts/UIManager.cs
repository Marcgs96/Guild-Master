using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// TODO: Split UI Manager into smaller UI sections for each panel. This is getting too big.
public class UIManager : MonoBehaviour
{
    //Guild panel stuff
    public GameObject guild_panel;
    public GameObject guild_upgrade_costs;

    //Blacksmith panel stuff
    public BlackSmithPanel blacksmith_panel;

    //quests panel stuff
    public QuestPanel quest_panel;

    public GameObject final_panel;

    //members list stuff
    public GameObject members_list_panel;
    public GameObject member_listing;
    Dictionary<Member, GameObject> member_listings;
    public Sprite[] level_images;

    Member selected_member;

    //other prefabs
    public GameObject resource_prefab;
    public GameObject slot_prefab;
    public GameObject resource_cost_prefab;
    public GameObject member_result_prefab;
    public List<Texture2D> portraits;
    public List<Texture2D> resource_images;
    public List<Texture2D> backgrounds;
    public GameObject quest_result_popup;

    Queue<GameObject> popup_queue;
    GameObject current_popup = null;

    void Awake()
    {
        member_listings = new Dictionary<Member, GameObject>();
        popup_queue = new Queue<GameObject>();

        quest_panel.Init();
        blacksmith_panel.Init();

        GameManager.manager.members.OnMemberAdd += CreateMemberListing;
        GameManager.manager.buildings[(int)Building.BUILDING_TYPE.GUILD_HOUSE].OnLevelUp += OnGuildHouseLevelUp;
        UpdateGuildHallPanel();
    }

    #region ButtonFuctions
    public void ActivateQuestsPanel()
    {
        quest_panel.gameObject.SetActive(!quest_panel.gameObject.activeSelf);
        guild_panel.SetActive(false);
        blacksmith_panel.gameObject.SetActive(false);
    }

    public void ActivateGuildPanel()
    {
        guild_panel.SetActive(!guild_panel.activeSelf);
        quest_panel.gameObject.SetActive(false);
        blacksmith_panel.gameObject.SetActive(false);
    }
    public void ActivateBlacksmithPanel()
    {
        blacksmith_panel.gameObject.SetActive(!blacksmith_panel.gameObject.activeSelf);
        guild_panel.SetActive(false);
        quest_panel.gameObject.SetActive(false);
    }

    internal void RemoveMemberListing(Member old_member)
    {
        Destroy(member_listings[old_member]);
        member_listings.Remove(old_member);
        UpdateMemberCountText();
    }

    public void OnQuestSelection(Quest new_quest)
    {
        quest_panel.OnQuestSelection(new_quest);
    }

    public void CloseQuestsPanel()
    {
        quest_panel.gameObject.SetActive(false);
    }

    public void CloseGuildPanel()
    {
        guild_panel.SetActive(false);
    }

    public void CloseBlacksmithPanel()
    {
        blacksmith_panel.gameObject.SetActive(false);
    }

    public void CloseQuestPreparation()
    {
        quest_panel.CloseQuestPreparation();
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
            listing.transform.GetChild(2).GetComponent<Slider>().value = (int)member.stamina;
    }

    public void OnMemberClick(Member member)
    {
        if (quest_panel.gameObject.activeSelf)
            quest_panel.AddMemberToQuest(member);
        else if (blacksmith_panel.gameObject.activeSelf)
            blacksmith_panel.OnMemberSelect(member);
        else
            Camera.main.GetComponent<CameraControls>().SetFocus(member.gameObject);
    }

    public void OnMemberLevelUp(Member member)
    {
        member_listings[member].transform.GetChild(4).GetComponentInChildren<Image>().sprite = level_images[member.lvl-1];
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

    public void RestButton()
    {
        selected_member.ChangeState(Member.MEMBER_STATE.REST);
    }

    public void WorkButton()
    {
        selected_member.ChangeState(Member.MEMBER_STATE.WORK);
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

    void CreateMemberListing(Member new_member)
    {
        GameObject new_listing = Instantiate(member_listing);
        new_listing.transform.GetChild(1).GetComponent<Text>().text = new_member.member_name;
        new_listing.transform.GetChild(0).GetComponent<RawImage>().texture = portraits[(int)new_member.type];
        new_listing.transform.GetChild(3).GetComponent<Text>().text = new_member.action_string;
        new_listing.transform.GetChild(4).GetComponentInChildren<Image>().sprite = level_images[new_member.lvl - 1];

        new_listing.GetComponent<Button>().onClick.AddListener(delegate { OnMemberClick(new_member); });
        new_listing.transform.GetChild(5).GetComponent<Button>().onClick.AddListener(delegate { OnStateClick(new_member); });
        new_listing.transform.SetParent(members_list_panel.transform.GetChild(1));

        member_listings.Add(new_member, new_listing);

        UpdateMemberCountText();
    }

    internal void CreateQuestResultPopup(Quest quest, bool receive_rewards, bool receive_bonus, List<Member> survivors)
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
        if (receive_bonus)
        {
            GameObject bonus = popup.transform.GetChild(1).GetChild(3).gameObject;
            bonus.SetActive(true);
            bonus.transform.GetChild(0).GetComponent<Text>().text = quest.bonus_reward.GetAmount().ToString();
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
        if (current_popup)
            return;

        if(popup_queue.Count > 0)
        {
            current_popup = popup_queue.Dequeue();
            current_popup.SetActive(true);
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

    public void MemberActionChange(Member member)
    {
        member_listings[member].transform.GetChild(3).GetComponent<Text>().text = member.action_string;
    }

    public void UpdateMemberCountText()
    {
        string member_count = GameManager.manager.members.GetMemberCount().ToString() + "/" + GameManager.manager.members.GetMemberCap().ToString();
        members_list_panel.transform.GetChild(0).GetComponentInChildren<Text>().text = member_count;
        guild_panel.transform.GetChild(2).GetChild(0).GetChild(1).GetComponent<Text>().text = member_count;
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
            guild_upgrade_costs.transform.parent.GetComponent<Button>().interactable = false;
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("MainMenu");
    }

    internal void ShowFinishPanel(bool state)
    {
        CloseBlacksmithPanel();
        CloseGuildPanel();
        CloseQuestsPanel();

        Button[] buttons = transform.GetChild(0).GetChild(0).GetComponentsInChildren<Button>();
        foreach (Button button in buttons)
        {
            button.interactable = false;
        }

        final_panel.SetActive(true);
        if (state)
            final_panel.transform.GetChild(1).gameObject.SetActive(true);
        else
        {
            final_panel.transform.GetChild(2).gameObject.SetActive(true);
            final_panel.transform.GetChild(0).GetComponentInChildren<Text>().text = "Defeat";
        }
    }
}
