using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Resource
{
    public enum ResourceType
    {
        Gold,
        Potion,
        Meat,
        Crown,
        Shield,
        Flame
    }
    [SerializeField]
    private ResourceType type;
    [SerializeField]
    private int amount;

    public int GetAmount()
    {
        return amount;
    }
    public ResourceType GetResourceType()
    {
        return type;
    }

    public Resource(ResourceType type, int amount)
    {
        this.type = type;
        this.amount = amount;
    }

    internal void Decrease(int v)
    {
        amount -= v;
        if (amount < 0)
            amount = 0;
    }

    internal void Increase(int v)
    {
        amount += v;
    }

    internal void SetAmount(int v)
    {
        amount = v;
    }
        
}
