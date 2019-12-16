using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Avatar
{
    public string AvatarName;
    public string Description;
    public string AvatarImage;

    public Avatar()
    {
        this.AvatarName = "";
        this.Description = "";
        this.AvatarImage = "";
    }

    public Avatar(Avatar avatar)
    {
        this.AvatarName = avatar.AvatarName;
        this.Description = avatar.Description;
        this.AvatarImage = avatar.AvatarImage;
    }
}
