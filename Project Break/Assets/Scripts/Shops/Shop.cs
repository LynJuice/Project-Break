using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    [SerializeField] Text CashDisplay;

    [SerializeField] Inventory Inv;
    public List<Item> Items;
    [SerializeField] Button[] Buttons;

    void Start()
    {
        SetupShop();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            ExitShop();
    }
    void SetupShop()
    {
        Inv.GetComponent<PlayerMovement>().CanMove = false;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;

        CashDisplay.text = Inv.Cash.ToString() + " - ¥";

        for (int i = 0; i < Items.Count; i++)
        {
            if (Items[i] == null)
                Buttons[i].gameObject.SetActive(false);
            else
                Buttons[i].GetComponentInChildren<Text>().text = Items[i].Name;

            if (Items[i] != null)
                if (Items[i].BuyCost > Inv.Cash)
                    Buttons[i].interactable = false;
        }
    }

    void ExitShop()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Inv.GetComponent<PlayerMovement>().CanMove = true;
        gameObject.SetActive(false);
    }

    public void Buy(int Z)
    {
        if (Items[Z].BuyCost < Inv.Cash)
        {
            Inv.AddItem(Items[Z]);
            Buttons[Z].interactable = false;
            SetupShop();
        }
    }
}
