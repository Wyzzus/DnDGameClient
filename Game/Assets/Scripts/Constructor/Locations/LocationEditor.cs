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
    public GameObject test;

    public Vector2 MouseClick;
    public Vector2 ResultForVector;

    [Header("Creating Objects")]
    public RectTransform ContextMenu;
    public GameObject AddMenu;
    //public 

    public PartLocation CurrentEditPart;

    public void EditLocation(Location location, PartLocation Part)
    {
        this.CurrentEditPart = Part;
        LocationName.text = location.LocationName;
        LocationDescription.text = location.Description;
        StartCoroutine(LoadImage(location.BackgroundImage));
    }

    public void AddImage()
    {
        var paths = StandaloneFileBrowser.OpenFilePanel("Добавить локацию", Application.dataPath, "", false);
        if (paths.Length > 0)
        {
            StartCoroutine(LoadImage(paths[0]));
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
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Vector2 newPos = Input.mousePosition;
            GameObject clone = Instantiate(test.gameObject, Background.transform);
            
            clone.GetComponentInChildren<RectTransform>().position = newPos;
            Vector2 local = clone.GetComponentInChildren<RectTransform>().anchoredPosition;
            float x = Map(local.x, -300, 300, -2000, 2000);
            float y = Map(local.y, -300, 300, -2000, 2000);
            Vector2 result = new Vector2(x, y);
            ContextMenu.anchoredPosition = local + Background.rectTransform.anchoredPosition;
            ShowContextMenu();
            Destroy(clone);
        }
    }

    public void ShowContextMenu()
    {
        ContextMenu.gameObject.SetActive(true);
    }

    public void AddObject()
    {
        AddMenu.SetActive(true);
    }

    public float Map(float x, float in_min, float in_max, float out_min, float out_max)
    {
        return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
    }

}
