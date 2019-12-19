using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GMAttributeContainer : Container
{
    public Attribute MyAttribute;
    public PlayerController GM;

    public Text Value; 

    public void Setup(Attribute at)
    {
        MyAttribute = at;
        Name.text = MyAttribute.AttributeName;
        Value.text = MyAttribute.AttributeValue.ToString();
    }

    public void Increase()
    {
        MyAttribute.AttributeValue += 0.5f;
        Value.text = MyAttribute.AttributeValue.ToString();
    }

    public void Decrease()
    {
        MyAttribute.AttributeValue -= 0.5f;
        Value.text = MyAttribute.AttributeValue.ToString();
    }
}
