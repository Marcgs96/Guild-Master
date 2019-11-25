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
    [SerializeField]
    uint gold, crowns, shields, potions, meat;
    public Text gold_txt, crowns_txt, shields_txt, potions_txt, meat_txt;

    void Start()
    {
        gold_txt.text = gold.ToString();
        crowns_txt.text = crowns.ToString();
        shields_txt.text = shields.ToString();
        potions_txt.text = potions.ToString();
        meat_txt.text = meat.ToString();
    }
    void AddResource(ResourceType type, uint amount)
    {
        switch (type)
        {
            case ResourceType.Gold:
                gold += amount;
                gold_txt.text = gold.ToString();
                break;
            case ResourceType.Crown:
                crowns += amount;
                crowns_txt.text = crowns.ToString();
                break;
            case ResourceType.Shield:
                shields += amount;
                shields_txt.text = shields.ToString();
                break;
            case ResourceType.Potion:
                potions += amount;
                potions_txt.text = potions.ToString();
                break;
            case ResourceType.Meat:
                meat += amount;
                meat_txt.text = meat.ToString();
                break;
            default:
                break;
        }
    }

    void RemoveResource(ResourceType type, uint amount)
    {
        switch (type)
        {
            case ResourceType.Gold:
                gold -= amount;
                gold_txt.text = gold.ToString();
                break;
            case ResourceType.Crown:
                crowns -= amount;
                crowns_txt.text = crowns.ToString();
                break;
            case ResourceType.Shield:
                shields -= amount;
                shields_txt.text = shields.ToString();
                break;
            case ResourceType.Potion:
                potions -= amount;
                potions_txt.text = potions.ToString();
                break;
            case ResourceType.Meat:
                meat -= amount;
                meat_txt.text = meat.ToString();
                break;
            default:
                break;
        }
    }

    uint GetResource(ResourceType type)
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

        return 0;
    }
}
