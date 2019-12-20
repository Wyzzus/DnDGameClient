using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using SFB;

public enum PlayerType { Player, GameMaster}

public class PlayerController : NetworkBehaviour
{
    #region Theme Pack

    [Header("Theme Pack")]
    public string packUrl = @"C:\Users\Wyzzus\Desktop\Паки\Standart\Standart.hgd";
    public ThemePack CurrentThemePack;
    PackConstructor con;

    #endregion

    #region Game Variables

    [Header("Game Variables")]
    public List<PlayerController> Players = new List<PlayerController>();
    public PlayerController ChosenPlayer;

    [SyncVar] public string Name;
    public int CurrentMap;
    public int CurrentAvatar;
    public List<int> Items = new List<int>();
    public List<int> Equipment = new List<int>();
    public List<Attribute> Attributes = new List<Attribute>();
    public List<int> CurrentEffects = new List<int>();
    public string CurrentRoll;

    public Image Map;

    public IEnumerator RollRoutine;


    #endregion

    #region Player Vars

    [Header("Player Vars")]
    public PlayerType Type;
    public LayerMask layerMask;
    public Vector3 NewPosition = Vector3.zero;
    public float MoveDamp = 10f;
    public Image Avatar;
    public int AvatarNum;
    public Text PlayerName;
    public int ChosenItem;

    #endregion

    #region Player UI

    [Header("Player UI")]
    public GameObject PlayerUI;
    public List<AttributeContainer> attributeContainers = new List<AttributeContainer>();
    public Image ItemIcon;
    public Dropdown AvatarSelector;

    public GameObject ItemContainerPrefab;
    public GameObject AttributeContainerPrefab;
    public GameObject EffectContainerPrefab;

    public float ItemContainerHeight;
    public float AttributeContainerHeight;

    public RectTransform ItemsView;
    public RectTransform EquipmentView;
    public RectTransform AttributeView;
    public RectTransform EffectView;

    public Text RollText;
    public Text ItemDescription;
    public Text EffectDescription;

    public GameObject ItemDescPanel;
    public GameObject EffectDescPanel;

    #endregion

    #region GM UI

    [Header("GM UI")]
    public GameObject GMUI;
    public Image GMItemIcon;
    public Dropdown GMAvatarSelector;
    public Dropdown LocationSelector;
    public Text GMRollText;

    public RectTransform PlayersView;
    public RectTransform GMItemsView;
    public RectTransform GMAttributesView;
    public RectTransform GMEffectsView;

    public RectTransform AddItemMenu;

    public GameObject PlayerContainerPrefab;
    public GameObject GMItemContainerPrefab;
    public GameObject GMAttributeContainerPrefab;
    public GameObject GMEffectContainerPrefab;
    public GameObject GMAddItemPrefab;

    #endregion

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
    }

    public void StartPack()
    {
        var path = StandaloneFileBrowser.OpenFilePanel("Открыть пак", Application.dataPath, "hgd", false);
        packUrl = path[0];
        LoadPack();
        LoadAvatar(0);
        ShowItemDescription(ChosenItem);
        UpdateEffects();
        UpdateAttributes();

        Players.Add(this);

        ShowPlayers();
        LoadLocation(1);
    }

    void Start()
    {
        Map = GameObject.FindGameObjectWithTag("Map").GetComponent<Image>();
        con = PackConstructor.instance;
        
    }

    public void SetupType()
    {
        if (isServer)
            Type = PlayerType.GameMaster;
        else
            Type = PlayerType.Player;

        switch (Type)
        {
            case PlayerType.GameMaster:
                GMUI.SetActive(true);
                PlayerUI.SetActive(false);
                break;
            case PlayerType.Player:
                GMUI.SetActive(false);
                PlayerUI.SetActive(true);
                break;
        }

        if (!isLocalPlayer)
        {
            GMUI.SetActive(false);
            PlayerUI.SetActive(false);
        }
    }

    void Update()
    {
        SetupType();
        if(isLocalPlayer)
            Movement();

        PlayerName.text = Name;

        RollText.text = CurrentRoll;
        GMRollText.text = CurrentRoll;
        if (ChosenItem == -1)
            ItemDescPanel.SetActive(false);
    }

    public void Movement()
    {
        if (Input.GetMouseButtonDown(1))
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

    #region Player Methods

    #region MediaSelectors

    public void LoadAvatar(int n)
    {
        if (n >= 0)
        {
            StartCoroutine(LoadImage(CurrentThemePack.Avatars[n].AvatarImage, Avatar));
            AvatarNum = n;
        }
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
        GMAvatarSelector.ClearOptions();

        List<Dropdown.OptionData> Opts = new List<Dropdown.OptionData>();
        foreach (Avatar ava in CurrentThemePack.Avatars)
        {
            Dropdown.OptionData data = new Dropdown.OptionData();
            data.text = ava.AvatarName;
            Opts.Add(data);
        }

        AvatarSelector.AddOptions(Opts);
        GMAvatarSelector.AddOptions(Opts);
        AvatarSelector.value = 0;
        GMAvatarSelector.value = 0;
    }

    public void SetupLocationSelector()
    {
        LocationSelector.ClearOptions();

        List<Dropdown.OptionData> Opts = new List<Dropdown.OptionData>();
        foreach (Location loc in CurrentThemePack.Locations)
        {
            Dropdown.OptionData data = new Dropdown.OptionData();
            data.text = loc.LocationName;
            Opts.Add(data);
        }

        LocationSelector.AddOptions(Opts);
        LocationSelector.value = 0;
    }

    public void LoadItem(int n)
    {

    }
    
    public void ShowItem(int n)
    {
        if(n >= 0)
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
        Debug.Log(CurrentThemePack.Items[n].ItemName + " was removed from " + Name);

        UpdateItems();
    }

    public void RemoveChosenItem()
    {
        if(Items.Count > 0)
        {
            RemoveItem(ChosenItem);
            ChosenItem = -1;
        }
        ItemIcon.sprite = null;
        ItemDescription.text = "";
        ShowHideObject(ItemDescPanel);
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
            if (CurrentThemePack.Items[n].Attributes[i].AttributeValue != 0)
            {
                Attributes[i].AttributeValue += CurrentThemePack.Items[n].Attributes[i].AttributeValue;
                Debug.Log(Name + ": Attribute " + Attributes[i].AttributeName + " was changed on " + CurrentThemePack.Items[n].Attributes[i].AttributeValue);
            }
        }

        UpdateItems();
        UpdateAttributes();
        ShowHideObject(ItemDescPanel);
        ChosenItem = -1;
    }

    public void UnequipItem(int n)
    {
        Items.Add(n);
        Equipment.Remove(n);

        Debug.Log(CurrentThemePack.Items[n].ItemName + " was unequiped by " + Name);
        for (int i = 0; i < Attributes.Count; i++)
        {
            if (CurrentThemePack.Items[n].Attributes[i].AttributeValue != 0)
            {
                Attributes[i].AttributeValue -= CurrentThemePack.Items[n].Attributes[i].AttributeValue;
                Debug.Log(Name + ": Attribute " + Attributes[i].AttributeName + " was changed on " + CurrentThemePack.Items[n].Attributes[i].AttributeValue);
            }
        }

        UpdateItems();
        UpdateAttributes();
    }

    public void AddEffect(int n)
    {
        CurrentEffects.Add(n);

        Debug.Log(Name + " was affected by " + CurrentThemePack.Effects[n].EffectName);
        for (int i = 0; i < Attributes.Count; i++)
        {
            if (CurrentThemePack.Effects[n].Attributes[i].AttributeValue != 0)
            {
                Attributes[i].AttributeValue += CurrentThemePack.Effects[n].Attributes[i].AttributeValue;
                Debug.Log(Name + ": Attribute " + Attributes[i].AttributeName + " was changed on " + CurrentThemePack.Effects[n].Attributes[i].AttributeValue);
            }
        }

        UpdateEffects();
    }

    public void RemoveEffect(int n)
    {
        CurrentEffects.Remove(n);

        Debug.Log(Name + " was shuffled off " + CurrentThemePack.Effects[n].EffectName);
        for (int i = 0; i < Attributes.Count; i++)
        {
            if (CurrentThemePack.Effects[n].Attributes[i].AttributeValue != 0)
            {
                Attributes[i].AttributeValue -= CurrentThemePack.Effects[n].Attributes[i].AttributeValue;
                Debug.Log(Name + ": Attribute " + Attributes[i].AttributeName + " was changed on " + CurrentThemePack.Effects[n].Attributes[i].AttributeValue);
            }
        }

        UpdateEffects();
    }

    public void Roll(int n)
    {
        if (RollRoutine != null)
            StopCoroutine(RollRoutine);
        RollRoutine = Dice(n);
        StartCoroutine(RollRoutine);
    }

    public IEnumerator Dice(int n)
    {
        WaitForSeconds delay = new WaitForSeconds(.2f);
        CurrentRoll = "Идет бросок";

        for (int i = 1; i <= 3; i++)
        {
            yield return delay;
            CurrentRoll += ".";
        }
        yield return delay;
        int res = 1;
        if (Input.GetKey(KeyCode.K))
            res = Random.Range(1, 3);
        else if (Input.GetKey(KeyCode.L))
            res = Random.Range(n - 2, n + 1);
        else
            res = Random.Range(1, n + 1);
        CurrentRoll = "Из " + n + " выпало " + res.ToString(); 
    }
    
    #endregion

    #region UI Handlers

    [ClientRpc]
    public void RpcShowName(string newName)
    {
        Name = newName;
        CmdSetNewName(Name);
    }

    [Command]
    public void CmdSetNewName(string newName)
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

        Items.Sort();
        Equipment.Sort();

        foreach(int n in Items)
        {
            GameObject clone = Instantiate<GameObject>(ItemContainerPrefab, ItemsView);
            clone.GetComponent<ItemContainer>().Setup(this, n);;
        }
        ItemsView.sizeDelta = new Vector2(ItemsView.sizeDelta.x, Items.Count * ItemContainerHeight);

        foreach (int n in Equipment)
        {
            GameObject clone = Instantiate<GameObject>(ItemContainerPrefab, EquipmentView);
            clone.GetComponent<ItemContainer>().Setup(this, n);
            clone.GetComponent<ItemContainer>().Equiped = true;
        }
        EquipmentView.sizeDelta = new Vector2(EquipmentView.sizeDelta.x, Equipment.Count * ItemContainerHeight);

        ChosenItem = -1;
    }

    public void UpdateAttributes()
    {
        ClearView(AttributeView);
        attributeContainers.Clear();
        foreach (Attribute at in Attributes)
        {
            GameObject clone = Instantiate<GameObject>(AttributeContainerPrefab, AttributeView);
            clone.GetComponent<AttributeContainer>().Setup(at);
            attributeContainers.Add(clone.GetComponent<AttributeContainer>());
        }
        AttributeView.sizeDelta = new Vector2(AttributeView.sizeDelta.x, Attributes.Count * AttributeContainerHeight);
    }

    public void UpdateEffects()
    {
        ClearView(EffectView);
        CurrentEffects.Sort();
        foreach (int n in CurrentEffects)
        {
            GameObject clone = Instantiate<GameObject>(EffectContainerPrefab, EffectView);
            clone.GetComponent<EffectContainer>().Setup(this, n); ;
        }
        EffectView.sizeDelta = new Vector2(EffectView.sizeDelta.x, CurrentEffects.Count * ItemContainerHeight);
    }

    #endregion
    
    #region ServiceMethods

    public void LoadPack()
    {
        CurrentThemePack = CurrentThemePack.LoadPack(packUrl);
        SetupAvatarSelector();
        LoadAttributes();
        SetupLocationSelector();
        LoadLocation(0);
    }

    public void IncreaseAttribute(int n, float value)
    {
        Attributes[n].AttributeValue += value;
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

    public IEnumerator LoadImage(string url, Sprite toImage)
    {
        url = con.GetPackFolder(packUrl) + "\\" + url;
        //"file:///D://SampleImage.png"
        WWW www = new WWW(url);
        while (!www.isDone)
            yield return null;
        toImage = null;
        toImage = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0, 0));
    }

    public void ShowHideObject(GameObject obj)
    {
        obj.SetActive(!obj.activeSelf);
    }

    public void ShowItemDescription(int n)
    {
        if (n >= 0)
        {
            ShowItem(n);
            string desc = CurrentThemePack.Items[n].ItemName;
            desc += "\n" + CurrentThemePack.Items[n].Description;
            desc += "\nВлияние на атрибуты:";

            foreach (Attribute at in CurrentThemePack.Items[n].Attributes)
            {
                if (at.AttributeValue != 0)
                {
                    string sign = (at.AttributeValue > 0) ? "+" : "-";
                    desc += "\n • " + at.AttributeName + ": " + sign + at.AttributeValue.ToString();
                }
            }

            ItemDescription.text = desc;
        }
    }

    public void ShowEffectDescription(int n)
    {
        if (CurrentEffects.Count > 0)
        {
            string desc = CurrentThemePack.Effects[n].EffectName;
            desc += "\n" + CurrentThemePack.Effects[n].Description;
            desc += "\nВлияние на атрибуты:";

            foreach (Attribute at in CurrentThemePack.Effects[n].Attributes)
            {
                if (at.AttributeValue != 0)
                {
                    string sign = (at.AttributeValue > 0) ? "+" : "-";
                    desc += "\n • " + at.AttributeName + ": " + sign + at.AttributeValue.ToString();
                }
            }

            EffectDescription.text = desc;
        }
    }

    public void BlockAttributeEdit(bool tf)
    {
        foreach(AttributeContainer ac in attributeContainers)
        {
            ac.BlockAttributes(tf);
        }
    }

    #endregion

    #endregion

    #region GM Methods

    public void ShowPlayerInfo()
    {
        ShowAttributes(ChosenPlayer);
        ShowItems(ChosenPlayer);
        ShowEffects(ChosenPlayer);
    }

    public void ShowPlayers()
    {
        ClearView(PlayersView);

        foreach(PlayerController pc in Players)
        {
            GameObject clone = Instantiate<GameObject>(PlayerContainerPrefab, PlayersView);
            clone.GetComponent<PlayerContainer>().Setup(this, pc);
        }
    }

    public void ShowAttributes(PlayerController pc)
    {
        ClearView(GMAttributesView);

        foreach (Attribute n in pc.Attributes)
        {
            GameObject clone = Instantiate<GameObject>(GMAttributeContainerPrefab, GMAttributesView);
            clone.GetComponent<GMAttributeContainer>().Setup(n);
        }
    }

    public void ShowItems(PlayerController pc)
    {
        ClearView(GMItemsView);

        foreach (int n in pc.Items)
        {
            GameObject clone = Instantiate<GameObject>(GMItemContainerPrefab, GMItemsView);
            clone.GetComponent<GMItemContainer>().Setup(this, pc, n, false);
        }

        foreach (int n in pc.Equipment)
        {
            GameObject clone = Instantiate<GameObject>(GMItemContainerPrefab, GMItemsView);
            clone.GetComponent<GMItemContainer>().Setup(this, pc, n, true);
        }
    }

    public void ShowEffects(PlayerController pc)
    {
        ClearView(GMEffectsView);

        foreach (int n in pc.CurrentEffects)
        {
            GameObject clone = Instantiate<GameObject>(GMEffectContainerPrefab, GMEffectsView);
            clone.GetComponent<GMEffectContainer>().Setup(this, pc, n);
        }
    }

    public void ShowSelectableItems()
    {
        ClearView(AddItemMenu);

        for(int i = 0; i < CurrentThemePack.Items.Count; i++)
        {
            GameObject clone = Instantiate<GameObject>(GMAddItemPrefab, AddItemMenu);
            clone.GetComponent<GMItemContainer>().Setup(this, ChosenPlayer, i, false);
        }
    }

    public void LoadLocation(int n)
    {
        if (n >= 0)
        {
            StartCoroutine(LoadImage(CurrentThemePack.Locations[n].BackgroundImage, Map));
        }
    }

    #endregion
}
