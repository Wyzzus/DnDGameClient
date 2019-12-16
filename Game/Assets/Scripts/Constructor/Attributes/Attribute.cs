using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Attribute
{
    public string AttributeName;
    public float AttributeValue;

    public Attribute(Attribute at)
    {
        this.AttributeName = at.AttributeName;
        this.AttributeValue = at.AttributeValue;
    }
}
