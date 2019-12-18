using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Attribute
{
    public string AttributeName;
    public string Description;
    public float AttributeValue;

    public Attribute()
    {
        this.AttributeName = "";
        this.Description = "";
        this.AttributeValue = 0;
    }

    public Attribute(Attribute at)
    {
        this.AttributeName = at.AttributeName;
        this.Description = at.Description;
        this.AttributeValue = at.AttributeValue;
    }
}
