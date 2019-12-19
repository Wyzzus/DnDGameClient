using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GMItemContainer : Container
{
    public PlayerController GM;
    public PlayerController player;
    public int MyItem;
    public bool Equiped;

    public void Setup(PlayerController GM, PlayerController player, int n, bool equiped)
    {
        MyItem = n;
        this.GM = GM;
        this.player = player;
        Equiped = equiped;
        string eq = (Equiped) ? " (e)" : "";
        Name.text = GM.CurrentThemePack.Items[n].ItemName + eq;
    }

    public void DeleteItem()
    {
        player.RemoveItem(MyItem);
        GM.ShowPlayerInfo();
    }

    public void AddItem()
    {
        player.AddItem(MyItem);
        GM.ShowPlayerInfo();
    }
}
