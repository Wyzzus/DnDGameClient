using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    public string ItemName;
    public string Description;
    public string ItemImage;

    public List<Attribute> Attributes;

    public Item()
    {
        this.ItemName = "";
        this.Description = "";
        this.ItemImage = "";
        Attributes = new List<Attribute>();
    }

    public Item(Item item)
    {
        this.ItemName = item.ItemName;
        this.Description = item.Description;
        this.ItemImage = item.ItemImage;
        Attributes = new List<Attribute>();
        foreach (Attribute at in item.Attributes)
        {
            this.Attributes.Add(new Attribute(at));
        }
    }
}
