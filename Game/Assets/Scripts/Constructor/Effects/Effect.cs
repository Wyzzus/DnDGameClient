using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Effect
{
    public string EffectName;
    public string Description;

    public List<Attribute> Attributes = new List<Attribute>();

    public Effect()
    {
        this.EffectName = "";
        this.Description = "";
        Attributes = new List<Attribute>();
    }

    public Effect(Effect effect)
    {
        this.EffectName = effect.EffectName;
        this.Description = effect.Description;
        Attributes = new List<Attribute>();
        foreach (Attribute at in effect.Attributes)
        {
            this.Attributes.Add(new Attribute(at));
        }
    }
}
