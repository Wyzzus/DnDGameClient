using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackConstructor : MonoBehaviour
{
    #region UIConfig
    public float LocationPartHeight = 100f;
    public float LocationViewWidth = 275f;

    public float ImageWidth = 600;
    public float ImageHeight = 350;

    public float ObjWidth = 325f;
    public float ObjHeight = 300f;

    public float ObjAvatarWidth = 35f;
    public float ObjAvatarHeigth = 35f;

    public float MapMaxSize = 2000f;
    #endregion

    #region Singleton

    public static PackConstructor instance;

    public void Awake()
    {
        instance = this;
    }

    #endregion
    [Header("Pack Parts")]
    public List<Location> Locations = new List<Location>();
    public List<DndObject> DndObjects = new List<DndObject>();
    public List<string> ObjectsCategories = new List<string>();

    [Header("Editors")]
    public GameObject LocationEditorWindow;
    public GameObject LocationPartPrefab;

    public GameObject DndObjectEditorWindow;
    public GameObject DndObjectPartPrefab;

    [Header("Tables Views")]
    public RectTransform LocationView;
    public RectTransform DndObjectView;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HideEditor(GameObject EditorWindow)
    {
        EditorWindow.SetActive(false);
    }

    public void RecalculateViewSize(RectTransform View, int Count, int Offset, float PartHeight)
    {
        View.sizeDelta = new Vector2(0, (Count + Offset) * PartHeight + 50f);
    }

    #region Locations

    public void OpenLocationEditor()
    {
        LocationEditorWindow.SetActive(true);
    }

    public PartLocation AddLocationPart()
    {
        RecalculateViewSize(LocationView, Locations.Count, 1, LocationPartHeight);
        GameObject clone = Instantiate(LocationPartPrefab, LocationView);
        return clone.GetComponentInChildren<PartLocation>();
    }

    #endregion

    #region Objects


    public void OpenDndObjectEditor()
    {
        DndObjectEditorWindow.SetActive(true);
    }

    public PartDndObject AddDndObjectPart()
    {
        RecalculateViewSize(DndObjectView, DndObjects.Count, 1, LocationPartHeight);
        GameObject clone = Instantiate(DndObjectPartPrefab, DndObjectView);
        return clone.GetComponentInChildren<PartDndObject>();
    }

    #endregion
}
