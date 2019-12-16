using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartLocation : MonoBehaviour
{
    public Location MyLocation;
    public Text Name;

    public void EditLocation()
    {
        LocationEditor editor = PackConstructor.instance.LocationEditorWindow.GetComponent<LocationEditor>();
        PackConstructor.instance.OpenLocationEditor();
        editor.EditLocation(MyLocation, this, true);
    }

    public void DeleteLocation()
    {
        PackConstructor.instance.Locations.Remove(MyLocation);
        var instance = PackConstructor.instance;
        instance.RecalculateViewSize(instance.LocationView, instance.Locations.Count, 0, instance.LocationPartHeight);
        Destroy(gameObject);
    }

    public void UpdateLocation()
    {
        Name.text = MyLocation.LocationName;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
