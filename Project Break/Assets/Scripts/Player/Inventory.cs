using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<ScriptableItems> Items;
    public List<ScriptableDemon> Demons;
    [SerializeField] int CurrentlySelectedDemon;

    [SerializeField] PlayerMovement Player;

    void Start()
    {
        AddNewDemon(Player.CurrentDemon);
    }

    public void ChangeDemon(bool Next)
    {
        if (Demons.Count == 0)
        {
            Debug.LogError("Player Has No Demons // Add Starting Demon");
            return;
        }

        if (Next)
        {
            if (CurrentlySelectedDemon +1 < Demons.Count)
            {
                CurrentlySelectedDemon++;
                Player.CurrentDemon = Demons[CurrentlySelectedDemon];
            }
            else
            {
                CurrentlySelectedDemon = 0;
                Player.CurrentDemon = Demons[CurrentlySelectedDemon];
            }
        }
        else
        {
            if(CurrentlySelectedDemon > 0)
            {
                CurrentlySelectedDemon--;
                Player.CurrentDemon = Demons[CurrentlySelectedDemon];
            }
            else
            {
                CurrentlySelectedDemon = Demons.Count -1;
                Player.CurrentDemon = Demons[CurrentlySelectedDemon];
            }
        }
    }
    public void AddNewItem(ScriptableItems Item,int NumberOfItems)
    {
        for (int i = 0; i < NumberOfItems; i++)
        {
            Items.Add(Item);
        }
        Debug.Log("Player Got: " + NumberOfItems + " Of " + Item.Name);
    }
    public void AddNewDemon(ScriptableDemon Demon)
    {
        if (Demons.Count >= 6)
            return;

        for (int i = 0; i < Demons.Count; i++)
        {
            if (Demons[i].name == Demon.name)
            return;
        }

        Debug.Log("Player Got: " + Demon.name);
        Demons.Add(Demon);
    }
}
