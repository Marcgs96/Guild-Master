using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManagement : MonoBehaviour
{
    public GameObject quests_panel;
    public GameObject quests_list;
    public GameObject quest_preparation;
    public GameObject guild_panel;
    public GameObject blacksmith_panel;


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
