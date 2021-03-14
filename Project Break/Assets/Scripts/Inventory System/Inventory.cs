using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public int Cash;
    public List<Item> Items;

    public void AddItem(Item item)
    {
        Items.Add(item);
    }
}
