using UnityEngine;

[System.Serializable]
public struct TileData
{
    public bool wall;
    [Space]
    public GameObject occupantPrefab;
}
