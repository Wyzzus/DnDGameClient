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

    public ThemePack()
    {
        Locations = new List<Location>();
        DndObjects = new List<DndObject>();
        ObjectsCategories = new List<string>();
    }

    public ThemePack(string PackName, List<Location> Locations, List<DndObject> DndObjects, List<string> ObjectsCategories)
    {
        this.PackName = PackName;
        this.Locations = Locations;
        this.DndObjects = DndObjects;
        this.ObjectsCategories = ObjectsCategories;
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
        foreach (Location obj in pack.Locations)
        {
            string NewPath = con.CurrentPackFolder + con.ImageFolder + "\\" + obj.LocationName + "." + PackConstructor.instance.ImageExtension;
            obj.BackgroundImage = NewPath;
        }

        foreach (DndObject obj in pack.DndObjects)
        {
            string NewPath = con.CurrentPackFolder + con.ImageFolder + "\\" + obj.DndObjectName + "." + PackConstructor.instance.ImageExtension;
            obj.DndObjectImage = NewPath;
        }


        return pack;
    }
}
