using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SFB;

public class LocationEditor : MonoBehaviour
{
    [Header ("Editor")]
    public InputField LocationName;
    public InputField LocationDescription;
    public string ImagePath;
    public Image Background;
    public ObjectView DndObjectView;

    public Vector2 MouseClick;
    public Vector2 ResultForVector;

    [Header("Creating Objects")]
    public RectTransform ChooseScroll;
    public GameObject SetablePart;
    public float PartHeight;
    public DndObject ObjectToSet;
    public List<LocationObject> LocationObjects = new List<LocationObject>();
    //public 

    public PartLocation CurrentEditPart;

    public void EditLocation(Location location, PartLocation Part)
    {
        this.CurrentEditPart = Part;
        LocationName.text = location.LocationName;
        LocationDescription.text = location.Description;
        StartCoroutine(LoadImage(location.BackgroundImage));
        SetupLocationObjects();
    }

    public void AddImage()
    {
        var paths = StandaloneFileBrowser.OpenFilePanel("Добавить локацию", Application.dataPath, "", false);
        if (paths.Length > 0)
        {
            if(paths[0].Length > 0)
            {
                StartCoroutine(LoadImage(paths[0]));
            }
        }
    }

    public IEnumerator LoadImage(string url)
    {
        //"file:///D://SampleImage.png"
        WWW www = new WWW(url);
        while (!www.isDone)
            yield return null;
        Background.sprite = null;
        Background.sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0, 0));

        float newWidth = 600f;
        float newHeight = 350f;

        float ratio = (float)www.texture.width / (float)www.texture.height;

        if (ratio > 1)
        {
            newHeight = newWidth / ratio;
            if (newHeight > PackConstructor.instance.ImageHeight)
            {
                newHeight = PackConstructor.instance.ImageHeight;
                newWidth = newHeight * ratio;
            }
        }
        else
        {
            newWidth = newHeight * ratio;
            if (newWidth > PackConstructor.instance.ImageWidth)
            {
                newWidth = PackConstructor.instance.ImageWidth;
                newHeight = newWidth / ratio;
            }
        }

        Background.rectTransform.sizeDelta = new Vector2(newWidth, newHeight);
        ImagePath = url;
    }

    public void SaveLocation()
    {
        if (!CurrentEditPart)
            CurrentEditPart = PackConstructor.instance.AddLocationPart();

        if (CurrentEditPart.MyLocation == null)
            CurrentEditPart.MyLocation = new Location();

        CurrentEditPart.MyLocation.LocationName = LocationName.text;
        CurrentEditPart.MyLocation.Description = LocationDescription.text;

        if (Background.sprite && (ImagePath != null || ImagePath != ""))
        {
            CurrentEditPart.MyLocation.BackgroundImage = ImagePath;
        }

        CurrentEditPart.MyLocation.LocationObjects.Clear();
        foreach (LocationObject lo in LocationObjects)
        {
            CurrentEditPart.MyLocation.LocationObjects.Add(lo);
        }
        
        CurrentEditPart.UpdateLocation();
        if(!PackConstructor.instance.Locations.Contains(CurrentEditPart.MyLocation))
            PackConstructor.instance.Locations.Add(CurrentEditPart.MyLocation);
    }

    public void ExitEditor()
    {
        LocationName.text = "";
        LocationDescription.text = "";
        Background.sprite = null;
        Background.rectTransform.sizeDelta = Vector2.zero;
        CurrentEditPart = null;
        LocationObjects.Clear();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Awake()
    {

        RefreshObjectScrollPanel();
    }

    public void RefreshObjectScrollPanel()
    {
        foreach (RectTransform child in ChooseScroll.GetComponentInChildren<RectTransform>())
        {
            Destroy(child.gameObject);
        }
        foreach (DndObject obj in PackConstructor.instance.DndObjects)
        {
            GameObject clone = Instantiate<GameObject>(SetablePart, ChooseScroll);
            clone.GetComponent<SetableObjectPart>().Setup(obj);
            Debug.Log(obj.DndObjectName);
        }
        ChooseScroll.sizeDelta = new Vector2(ChooseScroll.sizeDelta.x, PackConstructor.instance.DndObjects.Count * PartHeight);
    }

    public void ClearField()
    {
        foreach(RectTransform child in Background.transform.GetComponentInChildren<RectTransform>())
        {
            Destroy(child.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (ObjectToSet != null)
            {
                AddObjectOnLocation();
            }
        }
    }

    public void AddObjectOnLocation()
    {
        PackConstructor pc = PackConstructor.instance;
        Vector2 newPos = Input.mousePosition;
        ObjectView clone = Instantiate(DndObjectView, Background.transform);

        RectTransform cloneRect = clone.GetComponentInChildren<RectTransform>();
        cloneRect.position = newPos;
        float clampedX = Mathf.Clamp(cloneRect.localPosition.x, -pc.ImageWidth / 2f, pc.ImageWidth / 2f);
        float clampedY = Mathf.Clamp(cloneRect.localPosition.y, -pc.ImageHeight / 2f, pc.ImageHeight / 2f);
        Vector2 clampedLocal = new Vector2(clampedX, clampedY);
        cloneRect.localPosition = clampedLocal;
        Vector2 local = clone.GetComponentInChildren<RectTransform>().anchoredPosition;

        float x = Map(local.x, -pc.ImageWidth / 2f, pc.ImageWidth / 2f, -pc.MapMaxSize / 2f, pc.MapMaxSize / 2f);
        float y = Map(local.y, -pc.ImageWidth / 2f, pc.ImageWidth / 2f, -pc.MapMaxSize / 2f, pc.MapMaxSize / 2f);
        Vector2 result = new Vector2(x, y);
        LocationObject lo = new LocationObject();
        lo.Object = ObjectToSet;
        lo.Position = new SerVect3(cloneRect.localPosition.x, cloneRect.localPosition.y, 0);
        lo.ObjectName = ObjectToSet.DndObjectName;
        clone.Setup(ObjectToSet, lo);
        LocationObjects.Add(lo);
    }

    public void SetupLocationObjects()
    {
        PackConstructor pc = PackConstructor.instance;
        foreach (LocationObject loc in CurrentEditPart.MyLocation.LocationObjects)
        {
            ObjectView clone = Instantiate(DndObjectView, Background.transform);
            LocationObjects.Add(loc);
            RectTransform cloneRect = clone.GetComponentInChildren<RectTransform>();
            //float x = Map(loc.Position.x,  -pc.MapMaxSize / 2f, pc.MapMaxSize / 2f, -pc.ImageWidth / 2f, pc.ImageWidth / 2f);
            //float y = Map(loc.Position.y,  -pc.MapMaxSize / 2f, pc.MapMaxSize / 2f, -pc.ImageWidth / 2f, pc.ImageWidth / 2f);
            Vector2 clampedLocal = new Vector2(loc.Position.x, loc.Position.y);
            cloneRect.localPosition = clampedLocal;
            clone.Setup(loc.Object, loc);
        }
    }
   
    public float Map(float x, float in_min, float in_max, float out_min, float out_max)
    {
        return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
    }

}
