using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManagement : MonoBehaviour
{
    public GameObject quests_panel;
    public GameObject guild_panel;
    public GameObject blacksmith_panel;
    public GameObject members_list_panel;

    //quests panel stuff
    public GameObject quests_list;
    public GameObject quest_preparation;

    //members list stuff
    public GameObject member_listing;
    List<GameObject> listings;

    private void Awake()
    {
        MemberManager.OnMemberAdd += CreateListing;
    }

    void CreateListing(Member new_member)
    {
        GameObject new_listing = Instantiate(member_listing);
        Member.MemberInfo info = new_member.GetInfo();
        new_listing.transform.GetChild(1).GetComponent<Text>().text = info.name;
        new_listing.transform.SetParent(members_list_panel.transform);
    }

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

    public void OnQuestSelection()
    {
        quest_preparation.SetActive(true);
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
}
