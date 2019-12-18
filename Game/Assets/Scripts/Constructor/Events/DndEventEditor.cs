using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DndEventEditor : MonoBehaviour
{
    [Header("Editor")]
    public InputField DndEventName;
    public InputField DndEventDescription;
    public InputField DndEventRoll;

    public InputField Min;
    public InputField Max;

    public PartDndEvent CurrentEditPart;

    public void EditDndEvent(DndEvent dndEvent, PartDndEvent Part, bool newImage)
    {
        this.CurrentEditPart = Part;
        DndEventName.text = dndEvent.EventName;
        DndEventDescription.text = dndEvent.EventDescription;
        DndEventRoll.text = dndEvent.MaxRoll.ToString();
    }

    public void SaveDndEvent()
    {
        if (!CurrentEditPart)
            CurrentEditPart = PackConstructor.instance.AddDndEventPart();

        if (CurrentEditPart.MyEvent == null)
            CurrentEditPart.MyEvent = new DndEvent();

        CurrentEditPart.MyEvent.EventName = DndEventName.text;
        CurrentEditPart.MyEvent.EventDescription = DndEventDescription.text;
        CurrentEditPart.MyEvent.MaxRoll = (DndEventRoll.text != "") ? int.Parse(DndEventRoll.text) : 6;

        if(Min.text != "" && float.Parse(Min.text) > 0)
            PackConstructor.instance.MinEventTime = float.Parse(Min.text);

        if (Max.text != "" && float.Parse(Max.text) > 0)
            PackConstructor.instance.MaxEventTime = float.Parse(Max.text);

        CurrentEditPart.UpdateDndEvent();
        if (!PackConstructor.instance.DndEvents.Contains(CurrentEditPart.MyEvent))
            PackConstructor.instance.DndEvents.Add(CurrentEditPart.MyEvent);
    }

    public void ExitEditor()
    {
        DndEventName.text = "";
        DndEventDescription.text = "";
        DndEventRoll.text = "6";
        CurrentEditPart = null;
    }
}
