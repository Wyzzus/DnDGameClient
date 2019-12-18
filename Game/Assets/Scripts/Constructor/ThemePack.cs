using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[System.Serializable]
public class ThemePack
{
    public string PackName = "";
    public List<Location> Locations = new List<Location>();
    public List<DndObject> DndObjects = new List<DndObject>();
    public List<string> ObjectsCategories = new List<string>();
    public List<Avatar> Avatars = new List<Avatar>();
    public List<Attribute> Attributes = new List<Attribute>();
    public List<Item> Items = new List<Item>();
    public List<Effect> Effects = new List<Effect>();
    public List<DndEvent> DndEvents = new List<DndEvent>();

    public float MinEventTime;
    public float MaxEventTime;

    public ThemePack()
    {
        Locations = new List<Location>();
        DndObjects = new List<DndObject>();
        ObjectsCategories = new List<string>();
        Avatars = new List<Avatar>();
        Attributes = new List<Attribute>();
        Items = new List<Item>();
        Effects = new List<Effect>();
        DndEvents = new List<DndEvent>();

        MinEventTime = 120;
        MaxEventTime = 180;
    }

    public ThemePack(
        string PackName, 
        List<Location> Locations, 
        List<DndObject> DndObjects, 
        List<string> ObjectsCategories, 
        List<Avatar> Avatars, 
        List<Attribute> Attributes, 
        List<Item> Items, 
        List<Effect> Effects,
        List<DndEvent> DndEvents, float MinEventTime, float MaxEventTime)
    {
        this.PackName = PackName;
        this.Locations = Locations;
        this.DndObjects = DndObjects;
        this.ObjectsCategories = ObjectsCategories;
        this.Avatars = Avatars;
        this.Attributes = Attributes;
        this.Items = Items;
        this.Effects = Effects;
        this.DndEvents = DndEvents;
        this.MinEventTime = MinEventTime;
        this.MaxEventTime = MaxEventTime;
    }

    public void SavePack(string Path, string Name, string Extension)
    {
        ThemePack pack = this;
        string packFolder = Path + "\\" + Name;
        if (Directory.Exists(packFolder))
        {
            Directory.Delete(packFolder, true);
        }
        Directory.CreateDirectory(packFolder);
        SaveImages(packFolder);
        FileStream fs = new FileStream(packFolder + "\\" + Name + "." + Extension, FileMode.Create);
        BinaryFormatter formatter = new BinaryFormatter();
        formatter.Serialize(fs, pack);
    }

    public void SaveImages(string packFolder)
    {
        string ImageFolder = packFolder + "\\" + PackConstructor.instance.ImageFolder;
        if (!Directory.Exists(ImageFolder))
        {
            Directory.CreateDirectory(ImageFolder);
        }

        foreach(Location obj in Locations)
        {
            string NewPath = ImageFolder + "\\" + obj.LocationName + "." + PackConstructor.instance.ImageExtension;
            File.Copy(obj.BackgroundImage, NewPath);
            Debug.Log(NewPath);
            obj.BackgroundImage = obj.LocationName + "." + PackConstructor.instance.ImageExtension;
        }

        foreach (DndObject obj in DndObjects)
        {
            string NewPath = ImageFolder + "\\" + obj.DndObjectName + "." + PackConstructor.instance.ImageExtension;
            File.Copy(obj.DndObjectImage, NewPath);
            Debug.Log(NewPath);
            obj.DndObjectImage = obj.DndObjectName + "." + PackConstructor.instance.ImageExtension;
        }

        foreach (Avatar obj in Avatars)
        {
            string NewPath = ImageFolder + "\\" + obj.AvatarName + "." + PackConstructor.instance.ImageExtension;
            File.Copy(obj.AvatarImage, NewPath);
            Debug.Log(NewPath);
            obj.AvatarImage = obj.AvatarName + "." + PackConstructor.instance.ImageExtension;
        }

        foreach (Item obj in Items)
        {
            string NewPath = ImageFolder + "\\" + obj.ItemName + "." + PackConstructor.instance.ImageExtension;
            File.Copy(obj.ItemImage, NewPath);
            Debug.Log(NewPath);
            obj.ItemImage = obj.ItemName + "." + PackConstructor.instance.ImageExtension;
        }


    }

    public string GetImageName(string path)
    {
        string[] dirs = path.Split('\\');
        return dirs[dirs.Length - 1];
    }

    public ThemePack LoadPack(string Path)
    {
        ThemePack pack = new ThemePack();
        if (File.Exists(Path))
        {
            FileStream fs = new FileStream(Path, FileMode.Open);
            BinaryFormatter formatter = new BinaryFormatter();
            try
            {
                pack = (ThemePack)formatter.Deserialize(fs);
            }
            catch(System.Exception e)
            {
                Debug.Log(e.Message);
            }
            finally
            {
                fs.Close();
            }
        }

        PackConstructor con = PackConstructor.instance;
        if(pack.Locations != null)
        {
            foreach (Location obj in pack.Locations)
            {
                string NewPath = con.CurrentPackFolder + con.ImageFolder + "\\" + obj.LocationName + "." + PackConstructor.instance.ImageExtension;
                obj.BackgroundImage = NewPath;
            }
        }

        if (pack.DndObjects != null)
        {
            foreach (DndObject obj in pack.DndObjects)
            {
                string NewPath = con.CurrentPackFolder + con.ImageFolder + "\\" + obj.DndObjectName + "." + PackConstructor.instance.ImageExtension;
                obj.DndObjectImage = NewPath;
            }
        }

        if(pack.Avatars != null)
        {
            foreach (Avatar obj in pack.Avatars)
            {
                string NewPath = con.CurrentPackFolder + con.ImageFolder + "\\" + obj.AvatarName + "." + PackConstructor.instance.ImageExtension;
                obj.AvatarImage = NewPath;
            }
        }

        if (pack.Items != null)
        {
            foreach (Item obj in pack.Items)
            {
                string NewPath = con.CurrentPackFolder + con.ImageFolder + "\\" + obj.ItemName + "." + PackConstructor.instance.ImageExtension;
                obj.ItemImage = NewPath;
            }
        }


        return pack;
    }
}
