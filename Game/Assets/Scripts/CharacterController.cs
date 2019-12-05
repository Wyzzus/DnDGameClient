using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class CharacterController : MonoBehaviour
{

    public bool selected;
    public string[] ClassesList;
    public string[] SkinList;
    public Dropdown ClassesMenu;
    public Dropdown SkinMenu;
    public GameObject Character;
    Vector2 startPosition;
    SpriteRenderer Skin;

    public RectTransform me;


    private void Start()
    {
        Skin = GetComponent<SpriteRenderer>();
        ClassesList = Directory.GetDirectories("Assets/Resources/Sprites/Classes");
        ClassesMenu.options.Clear();
        foreach (string option in ClassesList)
        {
            ClassesMenu.options.Add(new Dropdown.OptionData(option.Substring(option.LastIndexOf(@"\") + 1)));
        }
        startPosition = me.anchoredPosition3D;
        UpdateSkinMenu();
    }

    public void UpdateSkinMenu()
    {
        SkinList = Directory.GetFiles("Assets/Resources/Sprites/Classes/" + ClassesMenu.options[ClassesMenu.value].text, "*.jpg");
        SkinMenu.options.Clear();
        foreach (var option in SkinList)
        {
            SkinMenu.options.Add(new Dropdown.OptionData(option.Substring(option.LastIndexOf(@"\") + 1)));
        }
    }

    public void setSkin()
    {
        Debug.Log(SkinMenu.options[SkinMenu.value].text);
        Skin.sprite = Resources.Load<Sprite>("Sprites/Classes/" + ClassesMenu.options[ClassesMenu.value].text + "/" +
            SkinMenu.options[SkinMenu.value].text.Remove(SkinMenu.options[SkinMenu.value].text.LastIndexOf(".")));
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            float t = 0;
            t = Time.deltaTime * 2f;
            me.anchoredPosition3D = new Vector2(Input.mousePosition.x - Screen.width / 2, Input.mousePosition.y - Screen.height / 2);
            //me.anchoredPosition3D = Vector2.Lerp(startPosition, new Vector2(Input.mousePosition.x - Screen.width / 2, Input.mousePosition.y - Screen.height / 2), Time.deltaTime);
            startPosition = me.anchoredPosition;
        }
    }
}