using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourcePopUp : MonoBehaviour
{

    public int type;
    string text_info = "";

    private void Start()
    {
        switch (type)
        {
            case 1:
                text_info = "Gold is a resource required to upgrade buildings such as the Guild House and the Blacksmith. It can also be used to buy upgrades for the heroes equipment. Obtainable via quests.";
                break;
            case 2:
                text_info = "Crowns are a resource required to upgrade the Guildhall. Obtainable via quests.";
                break;
            case 3:
                text_info = "Shields are a resource required to upgrade the Blacksmith. Obtainable via quests.";
                break;
            case 4:
                text_info = "Potions are a resource the player can assign to a quest, allowing a member to heal 50% stamina when its stamina drops below 25%. Obtainable via mage work.";
                break;
            case 5:
                text_info = "Meats are a resource the player can assign to a quest, during the quest, a food check can happen, each member needs to eat 1 piece of meat, if there is no food left, stamina will decrease a 25% to each member who cant eat. Obtainable via hunter work.";
                break;
            case 6:
                text_info = "Flames are a resource the player can assign to the quests to increase the rewards gained on succes. Obtainable via warrior work.";
                break;
            default:
                break;
        }

        transform.GetChild(1).GetChild(0).GetComponent<Text>().text = text_info;
    }

    public void ShowPopUp()
    {
        transform.GetChild(1).gameObject.SetActive(true);
    }

    public void HidePopUp()
    {
        transform.GetChild(1).gameObject.SetActive(false);
    }
}
