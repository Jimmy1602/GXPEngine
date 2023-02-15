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

    private Attack attack;
    private Boomerang boomerang;

    public MyGame() : base(1366, 768, false, true, 600, 400)     // Create a window that's 800x600 and NOT fullscreen
	{
        StartGame();
	}

    void StartGame()
    {
        LoadAttacks();

        Character cha = new Character(0, this, new Attack(DesignerChanges.attackTime), new Boomerang(DesignerChanges.boomerangFloatTime));
        cha.x = 300;
        AddChild(cha);

        Character che = new Character(1, this, new Attack(DesignerChanges.attackTime), new Boomerang(DesignerChanges.boomerangFloatTime));
        che.x = 1000;
        AddChild(che);
        che.other = cha;
        cha.other = che;
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

    void LoadAttacks()
    {
        attack = new Attack(DesignerChanges.attackTime);
        boomerang = new Boomerang(DesignerChanges.boomerangFloatTime);
    }
}