using System.Collections.Generic;

public struct AoeSplash
{
    public Tile mainTile;
    public List<Tile> splashTiles;

    public AoeSplash(Tile main, List<Tile> splash)
    {
        mainTile = main;
        splashTiles = splash;
    }
}
