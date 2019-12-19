using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerContainer : Container
{
    public PlayerController GM;
    public PlayerController player;

    public void Setup(PlayerController gm, PlayerController player)
    {
        this.GM = gm;
        this.player = player;
    }

    public void Update()
    {
        Name.text = player.Name;
    }

    public void Interact()
    {
        GM.ChosenPlayer = player;
        GM.ShowPlayerInfo();
    }
}
