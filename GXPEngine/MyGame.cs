using System;                                   // System contains a lot of default C# libraries 
using GXPEngine;                                // GXPEngine contains the engine
using System.Drawing;                           // System.Drawing contains drawing tools such as Color definitions
using System.Collections.Generic;
using GXPEngine.Core;
using System.Drawing.Text;
using TiledMapParser;
using System.Diagnostics.SymbolStore;

public class MyGame : Game {
    public int currentLevel;
    DesignerChanges designerChanges = new DesignerChanges();

    public MyGame() : base(1920, 1280, false, true, 600, 400)     // Create a window that's 800x600 and NOT fullscreen
	{
        Character cha = new Character(designerChanges);
        cha.player_id = 0;
        AddChild(cha);
        Character che = new Character(designerChanges);

        che.player_id = 1;
        AddChild(che);
	}

    // For every game object, Update is called every frame, by the engine:
    void Update() {
        
	}

	static void Main()                          // Main() is the first method that's called when the program is run
	{
		new MyGame().Start();                   // Create a "MyGame" and start it
	}

    public void changeLevel(int changeTo)
    {
        destroyAll();

        loadLevel(changeTo);
    }

    void loadLevel(int levelNum)
    {
        currentLevel = levelNum;

        //Level myLevel = new Level(this);
        //AddChild(myLevel);
        


        //Console.WriteLine(levelData);

        /*
        SetupLevels setupLevels = new SetupLevels();
        List<Vector2> corners = new List<Vector2>();

        for (int i = 0; i < setupLevels.pathes.GetLength(1); i++)
        {
            corners.Add(setupLevels.pathes[levelNum, i]);
        }

        */
    }

    void destroyAll()
    {
        List<GameObject> children = GetChildren();
        foreach (GameObject child in children)
        {
            child.LateDestroy();
        }
    }
}