using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceManager : MonoBehaviour
{
    [SerializeField]
    public Resource gold, crowns, shields, potions, meat, flames;
    public Text gold_txt, crowns_txt, shields_txt, potions_txt, meat_txt, flames_txt;

    void Start()
    {
        gold_txt.text = gold.GetAmount().ToString();
        crowns_txt.text = crowns.GetAmount().ToString();
        shields_txt.text = shields.GetAmount().ToString();
        potions_txt.text = potions.GetAmount().ToString();
        meat_txt.text = meat.GetAmount().ToString();
        flames_txt.text = flames.GetAmount().ToString();
    }

    public void IncreaseResource(Resource.ResourceType type, int amount)
    {
        switch (type)
        {
            case Resource.ResourceType.Gold:
                gold.Increase(amount);
                gold_txt.text = gold.GetAmount().ToString();
                break;
            case Resource.ResourceType.Crown:
                crowns.Increase(amount);
                crowns_txt.text = crowns.GetAmount().ToString();
                break;
            case Resource.ResourceType.Shield:
                shields.Increase(amount);
                shields_txt.text = shields.GetAmount().ToString();
                break;
            case Resource.ResourceType.Potion:
                potions.Increase(amount);
                potions_txt.text = potions.GetAmount().ToString();
                break;
            case Resource.ResourceType.Meat:
                meat.Increase(amount);
                meat_txt.text = meat.GetAmount().ToString();
                break;
            case Resource.ResourceType.Flame:
                flames.Increase(amount);
                flames_txt.text = flames.GetAmount().ToString();
                break;
            default:
                break;
        }
    }

    internal int GetResourcesScore()
    {
        int resources_score = 0;

        resources_score += gold.GetAmount() * 5;
        resources_score += crowns.GetAmount() * 10;
        resources_score += shields.GetAmount() * 10;
        resources_score += potions.GetAmount() * 4;
        resources_score += flames.GetAmount() * 4;
        resources_score += meat.GetAmount() * 4;

        return resources_score;
    }

    public void DecreaseResource(Resource.ResourceType type, int amount)
    {
        switch (type)
        {
            case Resource.ResourceType.Gold:
                gold.Decrease(amount);
                gold_txt.text = gold.GetAmount().ToString();
                break;
            case Resource.ResourceType.Crown:
                crowns.Decrease(amount);
                crowns_txt.text = crowns.GetAmount().ToString();
                break;
            case Resource.ResourceType.Shield:
                shields.Decrease(amount);
                shields_txt.text = shields.GetAmount().ToString();
                break;
            case Resource.ResourceType.Potion:
                potions.Decrease(amount);
                potions_txt.text = potions.GetAmount().ToString();
                break;
            case Resource.ResourceType.Meat:
                meat.Decrease(amount);
                meat_txt.text = meat.GetAmount().ToString();
                break;
            case Resource.ResourceType.Flame:
                flames.Decrease(amount);
                flames_txt.text = flames.GetAmount().ToString();
                break;
            default:
                break;
        }
    }

    internal bool HaveAmount(Resource.ResourceType type, int amount)
    {
        switch (type)
        {
            case Resource.ResourceType.Gold:
                return gold.GetAmount() >= amount;
            case Resource.ResourceType.Crown:
                return crowns.GetAmount() >= amount;
            case Resource.ResourceType.Shield:
                return shields.GetAmount() >= amount;
            case Resource.ResourceType.Potion:
                return potions.GetAmount() >= amount;
            case Resource.ResourceType.Meat:
                return meat.GetAmount() >= amount;
            case Resource.ResourceType.Flame:
                return flames.GetAmount() >= amount;
        }

        return false;
    }

    Resource GetResource(Resource.ResourceType type)
    {
        switch (type)
        {
            case Resource.ResourceType.Gold:
                return gold;
            case Resource.ResourceType.Crown:
                return crowns;
            case Resource.ResourceType.Shield:
                return shields;
            case Resource.ResourceType.Potion:
                return potions;
            case Resource.ResourceType.Meat:
                return meat;
            case Resource.ResourceType.Flame:
                return flames;
        }

        return null;
    }
}
