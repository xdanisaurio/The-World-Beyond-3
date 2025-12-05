using System.Collections.Generic;
using UnityEngine;

public class Inventory: MonoBehaviour
{
    Dictionary<string, int> items = new();
    public void AddItem(ICollectable collectable)
    {
        if (!items.ContainsKey(collectable.Id))
        {
            items.Add(collectable.Id, collectable.Ammount);
        }
        else
        {
            items[collectable.Id] += collectable.Ammount;
        }
    }
    public bool Spend(string id, int ammount)
    {
        if (items.ContainsKey(id) && items[id] >= ammount)
        {
            items[id] -= ammount;
            return true;
        }
        else
        {
            return false;
        }
    }
}