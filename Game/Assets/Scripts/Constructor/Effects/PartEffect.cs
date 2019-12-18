using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartEffect : MonoBehaviour
{
    public Effect MyEffect;
    public Text Name;

    public void EditEffect()
    {
        EffectEditor editor = PackConstructor.instance.EffectEditorWindow.GetComponent<EffectEditor>();
        PackConstructor.instance.OpenEffectEditor();
        editor.EditEffect(MyEffect, this, true);
    }

    public void DeleteEffect()
    {
        PackConstructor.instance.Effects.Remove(MyEffect);
        var instance = PackConstructor.instance;
        instance.RecalculateViewSize(instance.EffectView, instance.Effects.Count, 0, instance.LocationPartHeight);
        Destroy(gameObject);
    }

    public void UpdateEffect()
    {
        Name.text = MyEffect.EffectName;
    }
}
