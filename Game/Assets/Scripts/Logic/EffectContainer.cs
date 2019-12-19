using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EffectContainer : MonoBehaviour
{
    public PlayerController MyController;
    public int MyEffect;
    public Text Name;

    public void Setup(PlayerController pc, int n)
    {
        MyController = pc;
        MyEffect = n;
        Name.text = MyController.CurrentThemePack.Effects[n].EffectName;
    }

    public void Interact()
    {
        MyController.ShowEffectDescription(MyEffect);
    }
}
