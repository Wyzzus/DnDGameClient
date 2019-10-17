using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DndObject
{
    public string DndObjectName;
    public string Description;
    public string DndObjectImage;
    public int Category = 0;

    public DndObject()
    {
        this.DndObjectName = "";
        this.Description = "";
        this.DndObjectImage = "";
        this.Category = 0;
    }

    public DndObject(DndObject dndObject)
    {
        this.DndObjectName = dndObject.DndObjectName;
        this.Description = dndObject.Description;
        this.DndObjectImage = dndObject.DndObjectImage;
        this.Category = dndObject.Category;
    }
}
