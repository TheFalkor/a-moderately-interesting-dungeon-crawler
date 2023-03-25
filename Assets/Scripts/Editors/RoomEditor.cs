#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class RoomEditor : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Text currentIndexText;
    [SerializeField] private Text roomNameText;

    private CombatRoomSO[] allRooms;

    [Header("Runtime Variables")]
    private Transform combatParent;
    private bool isAddingWall = true;
    private bool isDrawing = false;
    private int index = 0;

    [Header("Memory")]
    private CombatRoomSO currentRoom;
    private TileData[] startState = new TileData[70];
    private Tile[] allTimes = new Tile[70];


    void Start()
    {
        allRooms = GetAllInstances<CombatRoomSO>();
        LoadRoom(0);
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            isDrawing = true;
            isAddingWall = true;
        }
        else if (Input.GetKey(KeyCode.Mouse1))
        {
            isDrawing = true;
            isAddingWall = false;
        }
        else
            isDrawing = false;

        if (isDrawing)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit && hit.transform.GetComponent<EditableTile>())
            {
                hit.transform.GetComponent<EditableTile>().Trigger();
            }
        }
    }

    private void LoadRoom(int index)
    {
        currentRoom = allRooms[index];
        currentIndexText.text = index.ToString();

        if (currentRoom.roomName != "")
            roomNameText.text = currentRoom.roomName;
        else
            roomNameText.text = "<no name>";

        for (int i = 0; i < startState.Length; i++)
        {
            startState[i].wall = currentRoom.tiles[i].wall;
            startState[i].occupantPrefab = currentRoom.tiles[i].occupantPrefab;
        }

        GridManager.instance.GenerateCombat(currentRoom, false);

        if (combatParent == null)
        {
            combatParent = GameObject.Find("Combat Parent").transform;

            int count = 0;
            foreach (Transform tr in combatParent.GetChild(0))
            {
                EditableTile et = (EditableTile)tr.gameObject.AddComponent(typeof(EditableTile));
                et.Initialize(this);

                allTimes[count] = tr.GetComponent<Tile>();
                count++;
            }
        }
    }

    public void ToggleWall(Vector2Int position)
    {
        if (!allTimes[position.y * 10 + position.x].IsWalkable() == isAddingWall)
            return;

        allTimes[position.y * 10 + position.x].Setup(isAddingWall);

        foreach (Tile t in allTimes)
            t.UpdateTileset();
    }

    public void NextRoom()
    {
        index = ++index % allRooms.Length;
        LoadRoom(index);
    }

    public void PreviousRoom()
    {
        index = --index < 0 ? allRooms.Length - 1 : index;
        LoadRoom(index);
    }

    public void SaveRoom()
    {
        for (int i = 0; i < startState.Length; i++)
            currentRoom.tiles[i].wall = !allTimes[i].IsWalkable();

        EditorUtility.SetDirty(currentRoom);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    public void RevertChanges()
    {
        for (int i = 0; i < startState.Length; i++)
            allTimes[i].Setup(currentRoom.tiles[i].wall);

        foreach (Tile t in allTimes)
            t.UpdateTileset();
    }

    private static T[] GetAllInstances<T>() where T : ScriptableObject
    {
        string[] guids = AssetDatabase.FindAssets("t:" + typeof(T).Name);  //FindAssets uses tags check documentation for more info
        T[] a = new T[guids.Length];
        for (int i = 0; i < guids.Length; i++)         //probably could get optimized 
        {
            string path = AssetDatabase.GUIDToAssetPath(guids[i]);
            a[i] = AssetDatabase.LoadAssetAtPath<T>(path);
        }

        return a;
    }

}

#endif