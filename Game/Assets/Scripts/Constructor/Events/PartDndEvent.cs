using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartDndEvent : MonoBehaviour
{
    public DndEvent MyEvent;
    public Text Name;

    public void EditDndEvent()
    {
        DndEventEditor editor = PackConstructor.instance.DndEventEditorWindow.GetComponent<DndEventEditor>();
        PackConstructor.instance.OpenDndEventEditor();
        editor.EditDndEvent(MyEvent, this, true);
    }

    public void DeleteDndEvent()
    {
        PackConstructor.instance.DndEvents.Remove(MyEvent);
        var instance = PackConstructor.instance;
        instance.RecalculateViewSize(instance.DndEventView, instance.DndEvents.Count, 0, instance.LocationPartHeight);
        Destroy(gameObject);
    }

    public void UpdateDndEvent()
    {
        Name.text = MyEvent.EventName;
    }
}
