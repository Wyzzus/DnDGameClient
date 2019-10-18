using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectView : MonoBehaviour
{
    public DndObject dndObject;
    public Image Avatar;

    public void Setup(DndObject dndObject)
    {
        this.dndObject = dndObject;
        StartCoroutine(LoadImage(dndObject.DndObjectImage));
    }

    public IEnumerator LoadImage(string url)
    {
        //"file:///D://SampleImage.png"
        WWW www = new WWW(url);
        while (!www.isDone)
            yield return null;
        Avatar.sprite = null;
        Avatar.sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0, 0));

        float newWidth = PackConstructor.instance.ObjAvatarWidth;
        float newHeight = PackConstructor.instance.ObjAvatarHeigth;

        float ratio = (float)www.texture.width / (float)www.texture.height;

        if (ratio > 1)
        {
            newHeight = newWidth / ratio;
            if (newHeight > PackConstructor.instance.ObjAvatarHeigth)
            {
                newHeight = PackConstructor.instance.ObjAvatarHeigth;
                newWidth = newHeight * ratio;
            }
        }
        else
        {
            newWidth = newHeight * ratio;
            if (newWidth > PackConstructor.instance.ObjAvatarWidth)
            {
                newWidth = PackConstructor.instance.ObjAvatarWidth;
                newHeight = newWidth / ratio;
            }
        }

        Avatar.rectTransform.sizeDelta = new Vector2(newWidth, newHeight);
    }


}
