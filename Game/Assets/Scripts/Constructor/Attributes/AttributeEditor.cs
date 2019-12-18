using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttributeEditor : MonoBehaviour
{
    [Header("Editor")]
    public InputField AttributeName;
    public InputField AttributeDescription;

    public PartAttribute CurrentEditPart;

    public void EditAttribute(Attribute attribute, PartAttribute Part, bool newImage)
    {
        this.CurrentEditPart = Part;
        AttributeName.text = attribute.AttributeName;
        AttributeDescription.text = attribute.Description;
    }

    public void SaveAttribute()
    {
        if (!CurrentEditPart)
            CurrentEditPart = PackConstructor.instance.AddAttributePart();

        if (CurrentEditPart.MyAttribute == null)
            CurrentEditPart.MyAttribute = new Attribute();

        CurrentEditPart.MyAttribute.AttributeName = AttributeName.text;
        CurrentEditPart.MyAttribute.Description = AttributeDescription.text;

        CurrentEditPart.UpdateAttribute();
        if (!PackConstructor.instance.Attributes.Contains(CurrentEditPart.MyAttribute))
            PackConstructor.instance.Attributes.Add(CurrentEditPart.MyAttribute);
    }

    public void ExitEditor()
    {
        AttributeName.text = "";
        AttributeDescription.text = "";
        CurrentEditPart = null;
    }
}
