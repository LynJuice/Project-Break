using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [HideInInspector] public bool Changing;
    public List<ScriptableItems> Items;
    public List<ScriptableDemon> Demons;
    [SerializeField] int CurrentlySelectedDemon;
    public PlayerMovement Player;

    [Header("Animaton")]
    [SerializeField] Animator Anim;
    [SerializeField] Image DemonIcon;

    void Start()
    {
        DemonIcon.sprite = Player.CurrentDemon.Icon;
        AddNewDemon(Player.CurrentDemon);
    }

    public void ChangeDemon(bool Next)
    {
        if (Changing == true)
            return;

        if (Demons.Count == 0)
        {
            Debug.LogError("Player Has No Demons // Add Starting Demon");
            return;
        }

        if (Demons.Count == 1)
            return;

        Changing = true;

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

        StartCoroutine(ChangeDemonIcon());
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

    IEnumerator ChangeDemonIcon()
    {
        Anim.SetTrigger("Change Icon");
        yield return new WaitForSeconds(0.2f);
        DemonIcon.sprite = Player.CurrentDemon.Icon;
        yield return new WaitForSeconds(0.2f);
        Changing = false;
    }
}