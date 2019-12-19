using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GMEffectContainer : Container
{
    public PlayerController GM;
    public PlayerController player;
    public int MyEffect;

    public void Setup(PlayerController GM, PlayerController player, int n)
    {
        this.player = player;
        this.GM = GM;
        MyEffect = n;
        Name.text = GM.CurrentThemePack.Effects[n].EffectName;
    }

    public void DeleteEffect()
    {
        player.RemoveEffect(MyEffect);
        GM.ShowPlayerInfo();
    }
}
