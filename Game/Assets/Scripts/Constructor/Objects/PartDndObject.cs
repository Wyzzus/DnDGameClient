using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartDndObject : MonoBehaviour
{
    public DndObject MyDndObject;
    public Text Name;

    public void EditObject()
    {
        DndObjectEditor editor = PackConstructor.instance.DndObjectEditorWindow.GetComponent<DndObjectEditor>();
        PackConstructor.instance.OpenDndObjectEditor();
        editor.EditObject(MyDndObject, this);
    }

    public void Delete()
    {
        PackConstructor.instance.DndObjects.Remove(MyDndObject);
        var instance = PackConstructor.instance;
        instance.RecalculateViewSize(instance.DndObjectView, instance.DndObjects.Count, 0, instance.LocationPartHeight);
        Destroy(gameObject);
    }

    public void UpdateDndObject()
    {
        Name.text = MyDndObject.DndObjectName;
    }
}
