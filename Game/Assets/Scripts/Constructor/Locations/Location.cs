using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class SerVect3
{
    public float x;
    public float y;
    public float z;

    public SerVect3(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public SerVect3(Vector3 vector)
    {
        this.x = vector.x;
        this.y = vector.y;
        this.z = vector.z;
    }

    public Vector3 vect()
    {
        return new Vector3(x, y, z);
    }
}

[System.Serializable]
public class LocationObject
{
    public DndObject Object;
    public string ObjectName;
    public SerVect3 Position;
}

[System.Serializable]
public class Location
{
    public string LocationName;
    public string Description;
    public string BackgroundImage;
    public LocationObject[] LocationObjects;

    [System.NonSerialized]
    public Image Background;

    public Location()
    {
        this.LocationName = "";
        this.Description = "";
        this.BackgroundImage = "";
        this.Background = null;
        this.LocationObjects = null;
    }

    public Location(Location loc)
    {
        this.LocationName = loc.LocationName;
        this.Description = loc.Description;
        this.BackgroundImage = loc.BackgroundImage;
        this.Background = loc.Background;
        this.LocationObjects = loc.LocationObjects;
    }
}
