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

    [Header("Global Config")]
    public string ImageFolder = "Images";
    public string ImageExtension = "png";

    #region Singleton

    public static PackConstructor instance;

    public void Awake()
    {
        instance = this;
    }

    #endregion
    [Header("Pack Parts")]
    public string CurrentPackFolder;
    public List<Location> Locations = new List<Location>();
    public List<DndObject> DndObjects = new List<DndObject>();
    public List<string> ObjectsCategories = new List<string>();
    public List<Avatar> Avatars = new List<Avatar>();
    public List<Attribute> Attributes = new List<Attribute>();
    public List<Item> Items = new List<Item>();
    public List<Effect> Effects = new List<Effect>();

    [Header("Editors")]
    public GameObject LocationEditorWindow;
    public GameObject LocationPartPrefab;

    public GameObject DndObjectEditorWindow;
    public GameObject DndObjectPartPrefab;

    public GameObject AvatarEditorWindow;
    public GameObject AvatarEditorPartPrefab;

    public GameObject AttributeEditorWindow;
    public GameObject AttributeEditorPartPrefab;

    public GameObject ItemEditorWindow;
    public GameObject ItemEditorPartPrefab;
    
    public GameObject EffectEditorWindow;
    public GameObject EffectEditorPartPrefab;

    [Header("Tables Views")]
    public RectTransform LocationView;
    public RectTransform DndObjectView;
    public RectTransform AvatarView;
    public RectTransform AttributeView;
    public RectTransform ItemView;
    public RectTransform EffectView;

    public InputField PackNamer;
    

    // Start is called before the first frame update
    void Start()
    {
        LoadPackTest();
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
        ThemePack tp = new ThemePack(PackNamer.text, Locations, DndObjects, ObjectsCategories, Avatars, Attributes, Items, Effects);
        tp.SavePack(packPath, packName, extension);
        OpenPack(packPath);

    }

    public void ClearView(RectTransform view)
    {
        foreach (RectTransform rt in view.GetComponentsInChildren<RectTransform>())
        {
            if(rt != view)
            {
                Destroy(rt.gameObject);
            }
        }
    }

    public void OpenPack(string path)
    {
        ThemePack tp = new ThemePack();
        //Debug.Log(path[0]);

        string[] dirs = path.Split('\\');
        string packPath = "";
        for (int i = 0; i < dirs.Length - 1; i++)
        {
            packPath += dirs[i] + "\\";
        }

        CurrentPackFolder = packPath;
        tp = tp.LoadPack(path);


        this.Locations = tp.Locations;
        this.DndObjects = tp.DndObjects;
        this.ObjectsCategories = tp.ObjectsCategories;
        this.Avatars = tp.Avatars;
        this.Attributes = tp.Attributes;
        this.Items = tp.Items;
        this.Effects = tp.Effects;


        PackNamer.text = tp.PackName;
        ShowLoadedPack();
    }

    public void LoadPackTest()
    {
        string extension = "hgd";
        //var path = StandaloneFileBrowser.OpenFilePanel("Открыть пак", Application.dataPath, extension, false);
        OpenPack("C:\\Users\\Wyzzus\\Desktop\\testKek\\testKek.hgd");
    }

    public void LoadPack()
    {
        string extension = "hgd";
        var path = StandaloneFileBrowser.OpenFilePanel("Открыть пак", Application.dataPath, extension, false);
        OpenPack(path[0]);
    }

    public void ShowLoadedPack()
    {
        ClearView(LocationView);
        ClearView(DndObjectView);
        ClearView(AvatarView);
        ClearView(AttributeView);
        ClearView(ItemView);
        ClearView(EffectView);

        foreach (Location loc in Locations)
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

        foreach (Avatar obj in Avatars)
        {
            PartAvatar pa = AddAvatarPart();
            pa.MyAvatar = obj;
            pa.UpdateAvatar();
        }

        foreach (Attribute obj in Attributes)
        {
            PartAttribute pa = AddAttributePart();
            pa.MyAttribute = obj;
            pa.UpdateAttribute();
        }

        foreach (Item obj in Items)
        {
            PartItem pi = AddItemPart();
            pi.MyItem = obj;
            pi.UpdateItem();
        }

        foreach (Effect obj in Effects)
        {
            PartEffect pe = AddEffectPart();
            pe.MyEffect = obj;
            pe.UpdateEffect();
        }
    }

    #region UI
    public void HideEditor(GameObject EditorWindow)
    {
        EditorWindow.SetActive(false);
    }

    public void RecalculateViewSize(RectTransform View, int Count, int Offset, float PartHeight)
    {
        View.sizeDelta = new Vector2(0, (Count + Offset) * PartHeight + 50f);
    }
    #endregion

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

    #region Avatars

    public void OpenAvatarEditor()
    {
        AvatarEditorWindow.SetActive(true);
    }

    public PartAvatar AddAvatarPart()
    {
        RecalculateViewSize(AvatarView, Avatars.Count, 1, LocationPartHeight);
        GameObject clone = Instantiate(AvatarEditorPartPrefab, AvatarView);
        return clone.GetComponentInChildren<PartAvatar>();
    }

    #endregion

    #region Attributes

    public void OpenAttributeEditor()
    {
        AttributeEditorWindow.SetActive(true);
    }

    public PartAttribute AddAttributePart()
    {
        RecalculateViewSize(AttributeView, Attributes.Count, 1, LocationPartHeight);
        GameObject clone = Instantiate(AttributeEditorPartPrefab, AttributeView);
        return clone.GetComponentInChildren<PartAttribute>();
    }

    #endregion

    #region Items

    public void OpenItemEditor()
    {
        ItemEditorWindow.SetActive(true);
        //ItemEditorWindow.GetComponent<ItemEditor>().ShowAttributes();
    }

    public PartItem AddItemPart()
    {
        RecalculateViewSize(ItemView, Items.Count, 1, LocationPartHeight);
        GameObject clone = Instantiate(ItemEditorPartPrefab, ItemView);
        return clone.GetComponentInChildren<PartItem>();
    }

    #endregion

    #region Effects

    public void OpenEffectEditor()
    {
        EffectEditorWindow.SetActive(true);
        //ItemEditorWindow.GetComponent<ItemEditor>().ShowAttributes();
    }

    public PartEffect AddEffectPart()
    {
        RecalculateViewSize(EffectView, Effects.Count, 1, LocationPartHeight);
        GameObject clone = Instantiate(EffectEditorPartPrefab, EffectView);
        return clone.GetComponentInChildren<PartEffect>();
    }

    #endregion

}
