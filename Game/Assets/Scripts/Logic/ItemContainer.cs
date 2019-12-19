using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemContainer : MonoBehaviour
{
    public PlayerController MyController;
    public int MyItem;
    public bool Equiped;
    public Text Name;

    public void Setup(PlayerController pc, int n)
    {
        MyController = pc;
        MyItem = n;
        Name.text = MyController.CurrentThemePack.Items[n].ItemName;
    }

    public void Interact()
    {
        if (Equiped)
        {
            MyController.UnequipItem(MyItem);
        }
        else
        {
            MyController.ChosenItem = MyItem;
            MyController.ShowItemDescription(MyItem);
            MyController.ItemDescPanel.SetActive(true);
        }
    }
}
