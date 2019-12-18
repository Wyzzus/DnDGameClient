using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DndEvent
{
    public string EventName;
    public string EventDescription;
    public int MaxRoll;

    public DndEvent()
    {
        this.EventName = "";
        this.EventDescription = "";
        this.MaxRoll = 6;
    }

    public DndEvent(DndEvent obj)
    {
        this.EventName = obj.EventName;
        this.EventDescription = obj.EventDescription;
        this.MaxRoll = obj.MaxRoll;
    }
}
