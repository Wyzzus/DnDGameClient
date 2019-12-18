using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EffectEditor : MonoBehaviour
{
    [Header("Editor")]
    public InputField EffectName;
    public InputField EffectDescription;

    public PartEffect CurrentEditPart;

    public List<Attribute> EffectAttributes;

    public GameObject AttributePart;
    public RectTransform AttributesView;

    public void EditEffect(Effect effect, PartEffect Part, bool newImage)
    {
        PackConstructor con = PackConstructor.instance;
        this.CurrentEditPart = Part;
        EffectName.text = effect.EffectName;
        EffectDescription.text = effect.Description;

        ShowAttributes();
    }

    void Awake()
    {

    }

    public void SaveEffect()
    {
        if (!CurrentEditPart)
            CurrentEditPart = PackConstructor.instance.AddEffectPart();

        if (CurrentEditPart.MyEffect == null)
            CurrentEditPart.MyEffect = new Effect();

        CurrentEditPart.MyEffect.EffectName = EffectName.text;
        CurrentEditPart.MyEffect.Description = EffectDescription.text;

        CurrentEditPart.MyEffect.Attributes.Clear();
        foreach (Attribute at in EffectAttributes)
        {
            CurrentEditPart.MyEffect.Attributes.Add(new Attribute(at));
        }

        for (int i = 0; i < EffectAttributes.Count; i++)
        {
            CurrentEditPart.MyEffect.Attributes[i].AttributeValue = EffectAttributes[i].AttributeValue;
        }


        CurrentEditPart.UpdateEffect();
        if (!PackConstructor.instance.Effects.Contains(CurrentEditPart.MyEffect))
            PackConstructor.instance.Effects.Add(CurrentEditPart.MyEffect);

        foreach (Attribute at in EffectAttributes)
        {
            //at.AttributeValue = 0;
        }
    }

    public void ExitEditor()
    {
        EffectName.text = "";
        EffectDescription.text = "";
        CurrentEditPart = null;
    }

    public void ShowAttributes()
    {
        foreach (RectTransform child in AttributesView.GetComponentsInChildren<RectTransform>())
        {
            if (child != AttributesView)
            {
                Destroy(child.gameObject);
            }
        }

        EffectAttributes.Clear();

        var ats = (NeedUpdateAttributeStruct()) ? PackConstructor.instance.Attributes : CurrentEditPart.MyEffect.Attributes;

        foreach (Attribute at in ats)
        {
            EffectAttributes.Add(new Attribute(at));
        }

        foreach (Attribute at in EffectAttributes)
        {
            GameObject clone = Instantiate(AttributePart, AttributesView);
            clone.GetComponentInChildren<AttributeView>().Setup(at);
        }
        int cnt = EffectAttributes.Count;
        RecalculateViewSize(AttributesView, cnt, 0, 40f);

    }

    public bool NeedUpdateAttributeStruct()
    {
        if (CurrentEditPart == null)
        {
            Debug.Log("Current");
            return true;
        }
        if (PackConstructor.instance.Attributes.Count == CurrentEditPart.MyEffect.Attributes.Count)
        {
            int cnt = 0;
            for (int i = 0; i < PackConstructor.instance.Attributes.Count; i++)
            {
                if (PackConstructor.instance.Attributes[i].AttributeName == CurrentEditPart.MyEffect.Attributes[i].AttributeName)
                {
                    cnt++;
                }
            }
            if (cnt != PackConstructor.instance.Attributes.Count)
                return true;
            else
                return false;
        }
        else
        {
            return true;
        }
    }

    public void RecalculateViewSize(RectTransform View, int Count, int Offset, float PartHeight)
    {
        View.sizeDelta = new Vector2(0, (Count + Offset) * PartHeight + 50f);
    }
}
