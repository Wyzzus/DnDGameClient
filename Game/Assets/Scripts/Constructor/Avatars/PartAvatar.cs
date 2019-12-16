using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartAvatar : MonoBehaviour
{
    public Avatar MyAvatar;
    public Text Name;

    public void EditObject()
    {
        AvatarEditor editor = PackConstructor.instance.AvatarEditorWindow.GetComponent<AvatarEditor>();
        PackConstructor.instance.OpenAvatarEditor();
        editor.EditObject(MyAvatar, this, true);
    }

    public void Delete()
    {
        PackConstructor.instance.Avatars.Remove(MyAvatar);
        var instance = PackConstructor.instance;
        instance.RecalculateViewSize(instance.AvatarView, instance.Avatars.Count, 0, instance.LocationPartHeight);
        Destroy(gameObject);
    }

    public void UpdateAvatar()
    {
        Name.text = MyAvatar.AvatarName;
    }
}
