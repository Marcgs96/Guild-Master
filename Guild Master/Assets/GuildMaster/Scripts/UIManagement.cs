using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManagement : MonoBehaviour
{
    public GameObject quests_panel;
    public GameObject quests_list;
    public GameObject quest_preparation;


    public void ActivateQuestsPanel()
    {
        quests_panel.SetActive(!quests_panel.activeSelf);
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

    public void CloseQuestPreparation()
    {
        quest_preparation.SetActive(false);
        quests_list.SetActive(true);
    }
}
