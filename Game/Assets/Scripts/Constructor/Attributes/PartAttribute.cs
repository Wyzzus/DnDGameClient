using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartAttribute : MonoBehaviour
{
    public Attribute MyAttribute;
    public Text Name;

    public void EditAttribute()
    {
        AttributeEditor editor = PackConstructor.instance.AttributeEditorWindow.GetComponent<AttributeEditor>();
        PackConstructor.instance.OpenAttributeEditor();
        editor.EditAttribute(MyAttribute, this, true);
    }

    public void DeleteAttribute()
    {
        PackConstructor.instance.Attributes.Remove(MyAttribute);
        var instance = PackConstructor.instance;
        instance.RecalculateViewSize(instance.AttributeView, instance.Attributes.Count, 0, instance.LocationPartHeight);
        Destroy(gameObject);
    }

    public void UpdateAttribute()
    {
        Name.text = MyAttribute.AttributeName;
    }
}
