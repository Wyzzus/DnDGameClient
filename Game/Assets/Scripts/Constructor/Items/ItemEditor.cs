using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SFB;

public class ItemEditor : MonoBehaviour
{
    [Header("Editor")]
    public InputField ItemName;
    public InputField ItemDescription;

    public string ImagePath;
    public Image Background;

    public PartItem CurrentEditPart;

    public List<Attribute> ItemAttributes;

    public GameObject AttributePart;
    public RectTransform AttributesView;

    public void EditItem(Item item, PartItem Part, bool newImage)
    {
        PackConstructor con = PackConstructor.instance;
        this.CurrentEditPart = Part;
        ItemName.text = item.ItemName;
        ItemDescription.text = item.Description;

        if (newImage)
        {
            StartCoroutine(LoadImage(item.ItemImage));
        }
        else
        {
            StartCoroutine(LoadImage(con.CurrentPackFolder + con.ImageFolder + "\\" + item.ItemName + "." + con.ImageExtension));
        }
        ShowAttributes();
    }

    void Awake()
    {

    }

    public void AddImage()
    {
        string ext = PackConstructor.instance.ImageExtension;
        var paths = StandaloneFileBrowser.OpenFilePanel("Добавить предмет", Application.dataPath, ext, false);
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
            CurrentEditPart = PackConstructor.instance.AddItemPart();

        if (CurrentEditPart.MyItem == null)
            CurrentEditPart.MyItem = new Item();

        CurrentEditPart.MyItem.ItemName = ItemName.text;
        CurrentEditPart.MyItem.Description = ItemDescription.text;

        CurrentEditPart.MyItem.Attributes.Clear();
        foreach(Attribute at in ItemAttributes)
        {
            CurrentEditPart.MyItem.Attributes.Add(new Attribute(at));
        }

        for(int i = 0; i < ItemAttributes.Count; i++)
        {
            CurrentEditPart.MyItem.Attributes[i].AttributeValue = ItemAttributes[i].AttributeValue;
        }

        if (Background.sprite && (ImagePath != null || ImagePath != ""))
        {
            CurrentEditPart.MyItem.ItemImage = ImagePath;
        }


        CurrentEditPart.UpdateItem();
        if (!PackConstructor.instance.Items.Contains(CurrentEditPart.MyItem))
            PackConstructor.instance.Items.Add(CurrentEditPart.MyItem);

        foreach(Attribute at in ItemAttributes)
        {
            //at.AttributeValue = 0;
        }
    }

    public void ExitEditor()
    {
        ItemName.text = "";
        ItemDescription.text = "";
        Background.sprite = null;
        CurrentEditPart = null;
        Background.rectTransform.sizeDelta = Vector2.zero;
    }

    public void ShowAttributes()
    {
        foreach (RectTransform child in AttributesView.GetComponentsInChildren<RectTransform>())
        {
            if(child != AttributesView)
            {
                Destroy(child.gameObject);
            }
        }

        ItemAttributes.Clear();

        var ats = (NeedUpdateAttributeStruct()) ? PackConstructor.instance.Attributes : CurrentEditPart.MyItem.Attributes;

        foreach (Attribute at in ats)
        {
            ItemAttributes.Add(new Attribute(at));
        }

        foreach (Attribute at in ItemAttributes)
        {
            GameObject clone = Instantiate(AttributePart, AttributesView);
            clone.GetComponentInChildren<AttributeView>().Setup(at);
        }
        int cnt = ItemAttributes.Count;
        RecalculateViewSize(AttributesView, cnt, 0, 40f);

    }

    public bool NeedUpdateAttributeStruct()
    {
        if (CurrentEditPart == null)
        {
            Debug.Log("Current");
            return true;
        }
        if (PackConstructor.instance.Attributes.Count == CurrentEditPart.MyItem.Attributes.Count)
        {
            int cnt = 0;
            for (int i = 0; i < PackConstructor.instance.Attributes.Count; i++)
            {
                if (PackConstructor.instance.Attributes[i].AttributeName == CurrentEditPart.MyItem.Attributes[i].AttributeName)
                {
                    cnt++;
                }
            }
            if (cnt != PackConstructor.instance.Attributes.Count)
                return true;
            else
                return false;
        }
        else
        {
            return true;
        }
    }

    public void RecalculateViewSize(RectTransform View, int Count, int Offset, float PartHeight)
    {
        View.sizeDelta = new Vector2(0, (Count + Offset) * PartHeight + 50f);
    }
}
