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


    private Character banana;
    private Character lemon;

    private Character playerOne;
    private Character playerTwo;

    public MyGame() : base(1366, 768, false, true, 600, 400)
	{
        StartGame();
	}

    void StartGame()
    {
        LoadCharackters();

        banana.Spawn(0, lemon);
        lemon.Spawn(1, banana);
    }

    void LoadCharackters()
    {
        banana = new Character(this, new Attack(DesignerChanges.attackTime), new Boomerang(DesignerChanges.boomerangFloatTime));
        lemon = new Character(this, new Boomerang(DesignerChanges.boomerangFloatTime), new Attack(DesignerChanges.attackTime));
    }



    public void ResetGame()
    {
        DestroyAll();
        StartGame();
    }

    // For every game object, Update is called every frame, by the engine:
    void Update()
    {

    }

    static void Main()                          // Main() is the first method that's called when the program is run
	{
		new MyGame().Start();                   // Create a "MyGame" and start it
	}



    void DestroyAll()
    {
        List<GameObject> children = GetChildren();
        foreach (GameObject child in children)
        {
            child.LateDestroy();
        }
    }

}