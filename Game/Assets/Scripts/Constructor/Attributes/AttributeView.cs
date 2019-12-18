using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttributeView : MonoBehaviour
{

    public Attribute MyAttribute;

    public Text Name;
    public InputField Value;

    public void Setup(Attribute at)
    {
        MyAttribute = at;
        Name.text = at.AttributeName;
        Value.text = at.AttributeValue.ToString();
    }

    public void UpdateAttribute()
    {
        MyAttribute.AttributeValue = int.Parse(Value.text);
    }
}
