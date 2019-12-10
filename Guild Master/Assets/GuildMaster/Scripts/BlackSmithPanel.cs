using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlackSmithPanel : MonoBehaviour
{
    public GameObject blacksmith_upgrade_costs;
    public GameObject buttons_go;
    public Text level_text;
    public RawImage member_image;
    public Text member_name;
    public Text member_level;
    public List<int> upgrade_costs;
    public GameObject tooltip;

    Building blacksmith;
    List<Button> upgrade_buttons;
    Member selected_member;

    internal void Init()
    {
        upgrade_buttons = new List<Button>();

        blacksmith = GameManager.manager.buildings[(int)Building.BUILDING_TYPE.BLACKSMITH];
        blacksmith.OnLevelUp += OnBlacksmithLevelUp;

        for (int i = 0; i < buttons_go.transform.childCount; i++)
        {
            upgrade_buttons.Add(buttons_go.transform.GetChild(i).GetComponent<Button>());
            upgrade_buttons[i].transform.GetChild(0).GetChild(0).GetComponent<Text>().text = upgrade_costs[i].ToString();
        }

        UpdateBlacksmithPanel();
    }

    public void OnBlacksmithLevelUp(uint level)
    {
        UpdateBlacksmithPanel();
    }

    public void OnMemberSelect(Member member)
    {
        tooltip.SetActive(false);
        selected_member = member;

        member_image.texture = GameManager.manager.ui.portraits[(int)selected_member.type];
        member_image.enabled = true;
        member_name.text = selected_member.member_name;
        member_name.enabled = true;
        member_level.text = "Level " + selected_member.lvl.ToString();
        member_level.enabled = true;

        UpgradeButtonSetup();
    }

    public void OnUpgradeButtonClick(int upgrade)
    {
        if(selected_member != null && GameManager.manager.resources.HaveAmount(Resource.ResourceType.Gold, upgrade_costs[upgrade-2]))
        {
            GameManager.manager.resources.DecreaseResource(Resource.ResourceType.Gold, upgrade_costs[upgrade - 2]);
            selected_member.lvl = (uint) upgrade;
            member_level.text = "Level " + selected_member.lvl.ToString();
            GameManager.manager.ui.OnMemberLevelUp(selected_member);
            UpgradeButtonSetup();
        }
    }

    private void UpgradeButtonSetup()
    {
        for (int i = 0; i < blacksmith.GetLevel() - 1; i++)
        {
            if (blacksmith.GetLevel() > selected_member.lvl && selected_member.lvl == (i+1))
                upgrade_buttons[i].interactable = true;
            else
                upgrade_buttons[i].interactable = false;
        }
    }

    public void UpdateBlacksmithPanel()
    {
        level_text.text = blacksmith.GetLevel().ToString();

        for (int i = 0; i < blacksmith_upgrade_costs.transform.childCount; i++)
        {
            Destroy(blacksmith_upgrade_costs.transform.GetChild(i).gameObject);
        }

        if (!blacksmith.IsMaxLevel())
        {
            List<Resource> resources = blacksmith.GetResourcesCost();
            foreach (Resource resource in resources)
            {
                GameObject resource_go = Instantiate(GameManager.manager.ui.resource_cost_prefab);
                resource_go.GetComponent<RawImage>().texture = GameManager.manager.ui.resource_images[(int)resource.GetResourceType()];
                resource_go.transform.GetChild(0).GetComponent<Text>().text = resource.GetAmount().ToString();
                resource_go.transform.SetParent(blacksmith_upgrade_costs.transform);
            }
        }
        else
            blacksmith_upgrade_costs.transform.parent.GetComponent<Button>().interactable = false;

        if (selected_member != null)
            UpgradeButtonSetup();
    }

    private void OnDisable()
    {
        selected_member = null;
        member_image.texture = GameManager.manager.ui.backgrounds[0];
        member_name.enabled = false;
        member_level.enabled = false;
        tooltip.SetActive(true);

        foreach (Button button in upgrade_buttons)
        {
            button.interactable = false;
        }
    }
}