#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditableTile : MonoBehaviour
{
    [Header("References")]
    private Tile tile;
    private RoomEditor editor;

    public void Initialize(RoomEditor editor)
    {
        TilesetManager.instance.randomizePattern = false;

        tile = GetComponent<Tile>();

        this.editor = editor;
    }

    public void Trigger()
    {
        editor.ToggleWall(tile.GetPosition());
    }

    
}
#endif
