using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CategoryPart : MonoBehaviour
{
    public Text Name;
    public string Category;

    public void Setup(string Name)
    {
        this.Category = Name;
        this.Name.text = Name;
    }

    public void Delete()
    {
        PackConstructor.instance.ObjectsCategories.Remove(this.Category);
        PackConstructor.instance.DndObjectEditorWindow.GetComponentInChildren<DndObjectEditor>().UpdateCategories();
        Destroy(gameObject);
    }
}
