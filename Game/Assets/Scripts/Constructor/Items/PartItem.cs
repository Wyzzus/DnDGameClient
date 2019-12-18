using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartItem : MonoBehaviour
{
    public Item MyItem;
    public Text Name;

    public void EditItem()
    {
        ItemEditor editor = PackConstructor.instance.ItemEditorWindow.GetComponent<ItemEditor>();
        PackConstructor.instance.OpenItemEditor();
        editor.EditItem(MyItem, this, true);
    }

    public void DeleteItem()
    {
        PackConstructor.instance.Items.Remove(MyItem);
        var instance = PackConstructor.instance;
        instance.RecalculateViewSize(instance.ItemView, instance.Items.Count, 0, instance.LocationPartHeight);
        Destroy(gameObject);
    }

    public void UpdateItem()
    {
        Name.text = MyItem.ItemName;
    }
}
