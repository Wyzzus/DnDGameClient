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

    [Header("Editors")]
    public GameObject LocationEditorWindow;
    public GameObject LocationPartPrefab;

    [Header("Tables Views")]
    public RectTransform LocationView;

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

    #region Locations

    public void OpenLocationEditor()
    {
        LocationEditorWindow.SetActive(true);
    }

    public void CreateLocation()
    {

    }

    public void RecalculateViewSize(RectTransform View, int Count, int Offset, float PartHeight)
    {
        View.sizeDelta = new Vector2(0, (Count + Offset) * PartHeight + 50f);
    }

    public PartLocation AddLocationPart()
    {
        RecalculateViewSize(LocationView, Locations.Count, 1, LocationPartHeight);
        GameObject clone = Instantiate(LocationPartPrefab, LocationView);
        return clone.GetComponentInChildren<PartLocation>();
    }

    #endregion
}
