using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceManager : MonoBehaviour
{
    public enum ResourceType
    {
        Gold,
        Crown,
        Shield,
        Potion,
        Meat,
    }

    [System.Serializable]
    public struct Resource
    {
        public ResourceType type;
        public uint amount;

        public Resource(ResourceType type, uint amount)
        {
            this.type = type;
            this.amount = amount;
        }
    }

    [SerializeField]
    Resource gold, crowns, shields, potions, meat;
    public Text gold_txt, crowns_txt, shields_txt, potions_txt, meat_txt;

    void Start()
    {
        gold_txt.text = gold.amount.ToString();
        crowns_txt.text = crowns.amount.ToString();
        shields_txt.text = shields.amount.ToString();
        potions_txt.text = potions.amount.ToString();
        meat_txt.text = meat.amount.ToString();
    }

    public void IncreaseResource(ResourceType type, uint amount)
    {
        switch (type)
        {
            case ResourceType.Gold:
                gold.amount += amount;
                gold_txt.text = gold.amount.ToString();
                break;
            case ResourceType.Crown:
                crowns.amount += amount;
                crowns_txt.text = crowns.amount.ToString();
                break;
            case ResourceType.Shield:
                shields.amount += amount;
                shields_txt.text = shields.amount.ToString();
                break;
            case ResourceType.Potion:
                potions.amount += amount;
                potions_txt.text = potions.amount.ToString();
                break;
            case ResourceType.Meat:
                meat.amount += amount;
                meat_txt.text = meat.amount.ToString();
                break;
            default:
                break;
        }
    }

    public void DecreaseResource(ResourceType type, uint amount)
    {
        switch (type)
        {
            case ResourceType.Gold:
                gold.amount -= amount;
                gold_txt.text = gold.amount.ToString();
                break;
            case ResourceType.Crown:
                crowns.amount -= amount;
                crowns_txt.text = crowns.amount.ToString();
                break;
            case ResourceType.Shield:
                shields.amount -= amount;
                shields_txt.text = shields.amount.ToString();
                break;
            case ResourceType.Potion:
                potions.amount -= amount;
                potions_txt.text = potions.amount.ToString();
                break;
            case ResourceType.Meat:
                meat.amount -= amount;
                meat_txt.text = meat.amount.ToString();
                break;
            default:
                break;
        }
    }

    internal bool HaveAmount(ResourceType type, uint amount)
    {
        switch (type)
        {
            case ResourceType.Gold:
                return gold.amount >= amount;
            case ResourceType.Crown:
                return crowns.amount >= amount;
            case ResourceType.Shield:
                return shields.amount >= amount;
            case ResourceType.Potion:
                return potions.amount >= amount;
            case ResourceType.Meat:
                return meat.amount >= amount;
        }

        return false;
    }

    Resource GetResource(ResourceType type)
    {
        switch (type)
        {
            case ResourceType.Gold:
                return gold;
            case ResourceType.Crown:
                return crowns;
            case ResourceType.Shield:
                return shields;
            case ResourceType.Potion:
                return potions;
            case ResourceType.Meat:
                return meat;
        }

        return gold;
    }
}
