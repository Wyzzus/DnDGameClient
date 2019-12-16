using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SFB;

public class DndObjectEditor : MonoBehaviour
{
    [Header("Editor")]
    public InputField DndObjectName;
    public InputField DndObjectDescription;

    public InputField CategoryName;

    public string ImagePath;
    public Image Background;

    public PartDndObject CurrentEditPart;

    public GameObject CategoryPart;
    public RectTransform CategoriesView;
    public Dropdown CategoryDropdown;

    public void EditObject(DndObject dndObject, PartDndObject Part, bool newImage)
    {
        PackConstructor con = PackConstructor.instance;
        this.CurrentEditPart = Part;
        DndObjectName.text = dndObject.DndObjectName;
        DndObjectDescription.text = dndObject.Description;
        CategoryDropdown.value = dndObject.Category;
        if(newImage)
        {
            StartCoroutine(LoadImage(dndObject.DndObjectImage));
        }
        else
        {
            StartCoroutine(LoadImage(con.CurrentPackFolder + con.ImageFolder + "\\" + dndObject.DndObjectName + "." + con.ImageExtension));
        }
    }

    void Start()
    {
        UpdateCategories();
    }

    public void AddImage()
    {
        string ext = PackConstructor.instance.ImageExtension;
        var paths = StandaloneFileBrowser.OpenFilePanel("Добавить объект", Application.dataPath, ext, false);
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

        float newWidth = PackConstructor.instance.ObjWidth;
        float newHeight = PackConstructor.instance.ObjHeight;

        float ratio = (float)www.texture.width / (float)www.texture.height;

        if (ratio > 1)
        {
            newHeight = newWidth / ratio;
            if (newHeight > PackConstructor.instance.ObjHeight)
            {
                newHeight = PackConstructor.instance.ObjHeight;
                newWidth = newHeight * ratio;
            }
        }
        else
        {
            newWidth = newHeight * ratio;
            if (newWidth > PackConstructor.instance.ObjWidth)
            {
                newWidth = PackConstructor.instance.ObjWidth;
                newHeight = newWidth / ratio;
            }
        }

        Background.rectTransform.sizeDelta = new Vector2(newWidth, newHeight);
        ImagePath = url;
    }

    public void SaveObject()
    {
        if (!CurrentEditPart)
            CurrentEditPart = PackConstructor.instance.AddDndObjectPart();

        if (CurrentEditPart.MyDndObject == null)
            CurrentEditPart.MyDndObject = new DndObject();

        CurrentEditPart.MyDndObject.DndObjectName = DndObjectName.text;
        CurrentEditPart.MyDndObject.Description = DndObjectDescription.text;
        if (CategoryDropdown.options.Count > 0)
            CurrentEditPart.MyDndObject.Category = CategoryDropdown.value;
        else
            CurrentEditPart.MyDndObject.Category = -1;

        if (Background.sprite && (ImagePath != null || ImagePath != ""))
        {
            CurrentEditPart.MyDndObject.DndObjectImage = ImagePath;
        }


        CurrentEditPart.UpdateDndObject();
        if (!PackConstructor.instance.DndObjects.Contains(CurrentEditPart.MyDndObject))
            PackConstructor.instance.DndObjects.Add(CurrentEditPart.MyDndObject);
    }

    public void ExitEditor()
    {
        DndObjectName.text = "";
        DndObjectDescription.text = "";
        Background.sprite = null;
        CategoryDropdown.value = 0;
        CurrentEditPart = null;
        Background.rectTransform.sizeDelta = Vector2.zero;
    }

    public void AddCategory()
    {
        PackConstructor.instance.ObjectsCategories.Add(CategoryName.text);
        int cnt = PackConstructor.instance.ObjectsCategories.Count;
        GameObject clone = Instantiate(CategoryPart, CategoriesView);
        clone.GetComponentInChildren<CategoryPart>().Setup(CategoryName.text);
        RecalculateViewSize(CategoriesView, cnt, 0, 30f);
        UpdateCategories();
    }

    public void ShowCategories()
    {
        foreach (RectTransform child in CategoriesView.GetComponentInChildren<RectTransform>())
        {
            Destroy(child.gameObject);
        }
        foreach (string cat in PackConstructor.instance.ObjectsCategories)
        {
            GameObject clone = Instantiate(CategoryPart, CategoriesView);
            clone.GetComponentInChildren<CategoryPart>().Setup(cat);
        }
        int cnt = PackConstructor.instance.ObjectsCategories.Count;
        RecalculateViewSize(CategoriesView, cnt, 0, 30f);

    }

    public void UpdateCategories()
    {
        CategoryDropdown.ClearOptions();

        foreach (string cat in PackConstructor.instance.ObjectsCategories)
        {
            Dropdown.OptionData data = new Dropdown.OptionData();
            data.text = cat;
            CategoryDropdown.options.Add(data);
        }
    }

    public void RecalculateViewSize(RectTransform View, int Count, int Offset, float PartHeight)
    {
        View.sizeDelta = new Vector2(0, (Count + Offset) * PartHeight + 50f);
    }
}
