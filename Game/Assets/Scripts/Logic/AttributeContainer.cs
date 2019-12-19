using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttributeContainer : MonoBehaviour
{
    public Attribute MyAttribute;

    public Text AttributeName;
    public InputField ValueField;

    // Update is called once per frame

    public void Setup(Attribute at)
    {
        MyAttribute = at;
    }

    void Update()
    {
        if(!ValueField.interactable)
        {
            ValueField.text = MyAttribute.AttributeValue.ToString();
        }
        AttributeName.text = MyAttribute.AttributeName;
    }

    public void SetAttribute(string value)
    {
        MyAttribute.AttributeValue = float.Parse(value);
    }

    public void BlockAttributes(bool tf)
    {
        ValueField.interactable = tf;
    }
}
