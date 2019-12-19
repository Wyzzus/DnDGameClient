using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PlayerType { Player, GameMaster}

public class PlayerController : MonoBehaviour
{
    [Header("Theme Pack")]
    public string packUrl = @"C:\Users\Wyzzus\Desktop\Паки\Standart\Standart.hgd";
    public ThemePack CurrentThemePack;
    PackConstructor con;

    [Header("Game Variables")]
    public string Name;
    public int CurrentMap;
    public int CurrentAvatar;
    public List<int> Items = new List<int>();
    public List<int> Equipment = new List<int>();
    public List<Attribute> Attributes = new List<Attribute>();

    [Header("Player Vars")]
    public PlayerType Type;
    public LayerMask layerMask;
    public Vector3 NewPosition = Vector3.zero;
    public float MoveDamp = 10f;
    public Image Avatar;
    public Text PlayerName;
    public int ChosenItem;

    [Header("Player UI")]
    public Image ItemIcon;
    public Dropdown AvatarSelector;
    public GameObject ItemContainerPrefab;
    public float ContainerHeight;
    public RectTransform ItemsView;
    public RectTransform EquipmentView;
    public Text ItemDescription;
    public GameObject DescPanel;

    // Start is called before the first frame update
    void Start()
    {
        con = PackConstructor.instance;
        LoadPack();
        LoadAvatar(0);
        AddItem(0);
        AddItem(1);
        AddItem(0);
        AddItem(1);
        EquipItem(0);
        ShowItemDescription(1);
    }


    #region MediaSelectors

    public void LoadAvatar(int n)
    {
        StartCoroutine(LoadImage(CurrentThemePack.Avatars[n].AvatarImage, Avatar));
    }

    public void LoadAttributes()
    {
        foreach(Attribute at in CurrentThemePack.Attributes)
        {
            Attributes.Add(new Attribute(at));
        }
    }

    public void SetupAvatarSelector()
    {
        AvatarSelector.ClearOptions();
        foreach (Avatar ava in CurrentThemePack.Avatars)
        {
            Dropdown.OptionData data = new Dropdown.OptionData();
            data.text = ava.AvatarName;
            AvatarSelector.options.Add(data);
        }
        AvatarSelector.value = 0;
    }

    public void LoadItem(int n)
    {

    }
    
    public void ShowItem(int n)
    {
        StartCoroutine(LoadImage(CurrentThemePack.Items[n].ItemImage, ItemIcon));
    }

    #endregion

    #region Media Handlers

    public void AddItem(int n)
    {
        Items.Add(n);
        Debug.Log(CurrentThemePack.Items[n].ItemName + " was added to " + Name);

        UpdateItems();
    }

    public void RemoveItem(int n)
    {
        Items.Remove(n);

        for (int i = 0; i < Attributes.Count; i++)
            Attributes[i].AttributeValue -= CurrentThemePack.Items[n].Attributes[i].AttributeValue;

        UpdateItems();
    }

    public void RemoveChosenItem()
    {
        if(Items.Count > 0)
        {
            RemoveItem(ChosenItem);
            ChosenItem = 0;
        }
        ItemIcon.sprite = null;
        ItemDescription.text = "";
        ShowHideObject(DescPanel);
    }

    public void EquipChosenItem()
    {
        EquipItem(ChosenItem);
    }

    public void EquipItem(int n)
    {
        Items.Remove(n);
        Equipment.Add(n);

        Debug.Log(CurrentThemePack.Items[n].ItemName + " was equiped by " + Name);
        for (int i = 0; i < Attributes.Count; i++)
        {
            Attributes[i].AttributeValue += CurrentThemePack.Items[n].Attributes[i].AttributeValue;
            Debug.Log(Name + ": Attribute " + Attributes[i].AttributeName + " was changed on " + CurrentThemePack.Items[n].Attributes[i].AttributeValue);
        }

        UpdateItems();
        UpdateAttributes();
    }

    public void UnequipItem(int n)
    {
        Items.Add(n);
        Equipment.Remove(n);

        Debug.Log(CurrentThemePack.Items[n].ItemName + " was unequiped by " + Name);
        for (int i = 0; i < Attributes.Count; i++)
        {
            Attributes[i].AttributeValue += CurrentThemePack.Items[n].Attributes[i].AttributeValue;
            Debug.Log(Name + ": Attribute " + Attributes[i].AttributeName + "was changed on " + CurrentThemePack.Items[n].Attributes[i].AttributeValue);
        }

        UpdateItems();
        UpdateAttributes();
    }

    #endregion

    #region UI Handlers

    public void ShowName(string newName)
    {
        PlayerName.text = newName;
    }

    public void ClearView(RectTransform view)
    {
        view.sizeDelta = new Vector2(view.sizeDelta.x, 0);
        foreach(RectTransform child in view.GetComponentInChildren<RectTransform>())
        {
            if (child != view)
                Destroy(child.gameObject);
        }
    }

    public void UpdateItems()
    {
        ClearView(ItemsView);
        ClearView(EquipmentView);

        foreach(int n in Items)
        {
            GameObject clone = Instantiate<GameObject>(ItemContainerPrefab, ItemsView);
            clone.GetComponent<ItemContainer>().Setup(this, n);;
        }
        ItemsView.sizeDelta = new Vector2(ItemsView.sizeDelta.x, Items.Count * ContainerHeight);

        foreach (int n in Equipment)
        {
            GameObject clone = Instantiate<GameObject>(ItemContainerPrefab, EquipmentView);
            clone.GetComponent<ItemContainer>().Setup(this, n);
            clone.GetComponent<ItemContainer>().Equiped = true;
        }
        EquipmentView.sizeDelta = new Vector2(EquipmentView.sizeDelta.x, Equipment.Count * ContainerHeight);
    }

    public void UpdateAttributes()
    {

    }

    #endregion

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    public void Movement()
    {
        if(Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, layerMask))
            {
                NewPosition = new Vector3(hit.point.x, 0, hit.point.z);
            }
        }
        transform.position = Vector3.Lerp(transform.position, NewPosition, Time.deltaTime * MoveDamp);
    }

    #region ServiceMethods

    public void LoadPack()
    {
        CurrentThemePack = CurrentThemePack.LoadPack(packUrl);
        SetupAvatarSelector();
        LoadAttributes();
    }

    public IEnumerator LoadImage(string url, Image toImage)
    {
        url = con.GetPackFolder(packUrl) + "\\" + url;
        //"file:///D://SampleImage.png"
        WWW www = new WWW(url);
        while (!www.isDone)
            yield return null;
        toImage.sprite = null;
        toImage.sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0, 0));
    }

    public void ShowHideObject(GameObject obj)
    {
        obj.SetActive(!obj.activeSelf);
    }

    public void ShowItemDescription(int n)
    {
        ShowItem(n);
        string desc = CurrentThemePack.Items[n].ItemName;
        desc += "\n" + CurrentThemePack.Items[n].Description;
        desc += "\nВлияние на атрибуты:";

        foreach(Attribute at in CurrentThemePack.Items[n].Attributes)
        {
            if (at.AttributeValue != 0)
            {
                string sign = (at.AttributeValue > 0) ? "+" : "-";
                desc += "\n • " + at.AttributeName + ": " + sign + at.AttributeValue.ToString();
            }
        }

        ItemDescription.text = desc;
    }

    #endregion
}
