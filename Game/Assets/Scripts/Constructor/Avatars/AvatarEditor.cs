using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SFB;

public class AvatarEditor : MonoBehaviour
{
    [Header("Editor")]
    public InputField AvatarName;
    public InputField AvatarDescription;

    public string ImagePath;
    public Image Background;

    public PartAvatar CurrentEditPart;

    public void EditObject(Avatar avatar, PartAvatar Part, bool newImage)
    {
        PackConstructor con = PackConstructor.instance;
        this.CurrentEditPart = Part;
        AvatarName.text = avatar.AvatarName;
        AvatarDescription.text = avatar.Description;
        if (newImage)
        {
            StartCoroutine(LoadImage(avatar.AvatarImage));
        }
        else
        {
            StartCoroutine(LoadImage(con.CurrentPackFolder + con.ImageFolder + "\\" + avatar.AvatarName + "." + con.ImageExtension));
        }
    }

    void Start()
    {

    }

    public void AddImage()
    {
        string ext = PackConstructor.instance.ImageExtension;
        var paths = StandaloneFileBrowser.OpenFilePanel("Добавить аватар", Application.dataPath, ext, false);
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
            CurrentEditPart = PackConstructor.instance.AddAvatarPart();

        if (CurrentEditPart.MyAvatar == null)
            CurrentEditPart.MyAvatar = new Avatar();

        CurrentEditPart.MyAvatar.AvatarName = AvatarName.text;
        CurrentEditPart.MyAvatar.Description = AvatarDescription.text;

        if (Background.sprite && (ImagePath != null || ImagePath != ""))
        {
            CurrentEditPart.MyAvatar.AvatarImage = ImagePath;
        }


        CurrentEditPart.UpdateAvatar();
        if (!PackConstructor.instance.Avatars.Contains(CurrentEditPart.MyAvatar))
            PackConstructor.instance.Avatars.Add(CurrentEditPart.MyAvatar);
    }

    public void ExitEditor()
    {
        AvatarName.text = "";
        AvatarDescription.text = "";
        Background.sprite = null;
        CurrentEditPart = null;
        Background.rectTransform.sizeDelta = Vector2.zero;
    }

    public void RecalculateViewSize(RectTransform View, int Count, int Offset, float PartHeight)
    {
        View.sizeDelta = new Vector2(0, (Count + Offset) * PartHeight + 50f);
    }
}
