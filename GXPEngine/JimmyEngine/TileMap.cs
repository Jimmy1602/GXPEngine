using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using GXPEngine;
using GXPEngine.Core;
using TiledMapParser;

class TileMap : GameObject {

    protected Texture2D grass = new Texture2D("Grass.jpg");
    protected Texture2D dirt = new Texture2D("Dirt.jpg");

    public List<Vector2> enemyPath = new List<Vector2>();

    //public Tile[,] tiles;

    public int tileSize;

    

    public TileMap(int PtileSize, MyGame myGame){
        tileSize = PtileSize;
        Console.WriteLine(String.Format("Level{0}.tmx", myGame.currentLevel));
        Map levelData = MapParser.ReadMap(String.Format("Level{0}.tmx", myGame.currentLevel));
    }

    void Update(){
    }

    public Vector2 positionToTilePosition(Vector2 position)
    {
        position = position.addVectors(position, new Vector2(tileSize, tileSize));
        position = position.divideVector(position, tileSize);
        position = new Vector2((int) position.x, (int) position.y);
        position = position.subVectors(position, new Vector2(1, 1));
        return position;
    }

    /*
    public Tile positionToTile(Vector2 position)
    {
        position = position.addVectors(position, new Vector2(tileSize, tileSize));
        position = position.divideVector(position, tileSize);
        position = new Vector2((int)position.x, (int)position.y);
        position = position.subVectors(position, new Vector2(1, 1));
        return tiles[(int)position.x, (int)position.y];
    }
    */
}