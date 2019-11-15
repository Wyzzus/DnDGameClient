using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SFB;
using UnityEngine.UI;

public class PackConstructor : MonoBehaviour
{
    [Header ("UIConfig")]
    public float LocationPartHeight = 100f;
    public float LocationViewWidth = 275f;

    public float ImageWidth = 600;
    public float ImageHeight = 350;

    public float ObjWidth = 325f;
    public float ObjHeight = 300f;

    public float ObjAvatarWidth = 35f;
    public float ObjAvatarHeigth = 35f;

    public float MapMaxSize = 4000f;

    #region Singleton

    public static PackConstructor instance;

    public void Awake()
    {
        instance = this;
    }

    #endregion
    [Header("Pack Parts")]
    public List<Location> Locations = new List<Location>();
    public List<DndObject> DndObjects = new List<DndObject>();
    public List<string> ObjectsCategories = new List<string>();

    [Header("Editors")]
    public GameObject LocationEditorWindow;
    public GameObject LocationPartPrefab;

    public GameObject DndObjectEditorWindow;
    public GameObject DndObjectPartPrefab;

    [Header("Tables Views")]
    public RectTransform LocationView;
    public RectTransform DndObjectView;

    public InputField PackNamer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SavePack()
    {
        string packName = "NewThemePack";
        string extension = "hgd";
        var paths = StandaloneFileBrowser.SaveFilePanel("Сохранить пак", Application.dataPath, packName, extension);
        string[] dirs = paths.Split('\\');
        string packPath = "";
        for(int i = 0; i < dirs.Length - 2; i++)
        {
            packPath += dirs[i] + "\\";
        }
        packPath += dirs[dirs.Length - 2];
        packName = dirs[dirs.Length - 1].Replace("." + extension, "");
        ThemePack tp = new ThemePack(PackNamer.text, Locations, DndObjects, ObjectsCategories);
        tp.SavePack(packPath, packName, extension);
    }

    public void LoadPack()
    {
        string extension = "hgd";
        var path = StandaloneFileBrowser.OpenFilePanel("Открыть пак", Application.dataPath, extension, false);
        ThemePack tp = new ThemePack();
        tp = tp.LoadPack(path[0]);
        Debug.Log(path[0]);
        this.Locations = tp.Locations;
        this.DndObjects = tp.DndObjects;
        this.ObjectsCategories = tp.ObjectsCategories;
        PackNamer.text = tp.PackName;
        ShowLoadedPack();
    }

    public void ShowLoadedPack()
    {
        foreach(Location loc in Locations)
        {
            PartLocation pl = AddLocationPart();
            pl.MyLocation = loc;
            pl.UpdateLocation();
        }

        foreach (DndObject obj in DndObjects)
        {
            PartDndObject po = AddDndObjectPart();
            po.MyDndObject = obj;
            po.UpdateDndObject();
        }
    }

    public void HideEditor(GameObject EditorWindow)
    {
        EditorWindow.SetActive(false);
    }

    public void RecalculateViewSize(RectTransform View, int Count, int Offset, float PartHeight)
    {
        View.sizeDelta = new Vector2(0, (Count + Offset) * PartHeight + 50f);
    }

    #region Locations

    public void OpenLocationEditor()
    {
        LocationEditorWindow.SetActive(true);
        LocationEditorWindow.GetComponent<LocationEditor>().RefreshObjectScrollPanel();
    }

    public PartLocation AddLocationPart()
    {
        RecalculateViewSize(LocationView, Locations.Count, 1, LocationPartHeight);
        GameObject clone = Instantiate(LocationPartPrefab, LocationView);
        return clone.GetComponentInChildren<PartLocation>();
    }

    #endregion

    #region Objects


    public void OpenDndObjectEditor()
    {
        DndObjectEditorWindow.SetActive(true);
        DndObjectEditorWindow.GetComponent<DndObjectEditor>().UpdateCategories();
        DndObjectEditorWindow.GetComponent<DndObjectEditor>().ShowCategories();
    }

    public PartDndObject AddDndObjectPart()
    {
        RecalculateViewSize(DndObjectView, DndObjects.Count, 1, LocationPartHeight);
        GameObject clone = Instantiate(DndObjectPartPrefab, DndObjectView);
        return clone.GetComponentInChildren<PartDndObject>();
    }

    #endregion
}
