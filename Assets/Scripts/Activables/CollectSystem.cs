using System.Collections.Generic;
using UnityEngine;

public class CollectSystem : MonoBehaviour
{
    [SerializeField] Inventory inventory;

    public void CollectItem(ICollectable collectable)
    {
        inventory.AddItem(collectable);
    }
}
