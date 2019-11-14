using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetableObjectPart : MonoBehaviour
{
    public DndObject CurrentObject;
    public Text ObjName;
    public Image ObjectImage;

    public void Setup(DndObject objectToSet)
    {
        this.CurrentObject = objectToSet;
        ObjName.text = this.CurrentObject.DndObjectName;
    }

    public void ChooseObject()
    {
        PackConstructor.instance.LocationEditorWindow.GetComponentInChildren<LocationEditor>().ObjectToSet = CurrentObject;
    }
}
