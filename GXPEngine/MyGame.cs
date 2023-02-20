using System;                                   // System contains a lot of default C# libraries 
using GXPEngine;                                // GXPEngine contains the engine
using System.Drawing;                           // System.Drawing contains drawing tools such as Color definitions
using System.Collections.Generic;
using GXPEngine.Core;
using System.Drawing.Text;
using TiledMapParser;
using System.Diagnostics.SymbolStore;
using XmlReader;
using System.Xml.Serialization;
using System.Drawing.Imaging;

public class MyGame : Game {
    private cardreader cardreader = new cardreader();
    public bool isCharacterSelect = true;

    private Character playerOne = null;
    private Character playerTwo = null;

    private CharacterSheet characterData;
    private AttackSheet attackData;

    public MyGame() : base(1366, 768, false, true, 600, 400)
	{
        Sprite background = new Sprite("CharacterRectWhite.png", false, false, false);
        background.width = width; background.height = height;
        AddChild(background);

        characterData = xmlReader.ReadCharacterMap("Characters.xml");
        attackData = xmlReader.ReadAttackMap("Attacks.xml");
        
        StartGame();
    }

    void StartGame()
    {
        isCharacterSelect = true;
    }

    void Update()
    {
        if (isCharacterSelect)
            characterSelect();
    }

    void characterSelect()
    {
        if(playerOne != null && playerTwo != null)
        {
            playerOne.Spawn(0, playerTwo);
            playerTwo.Spawn(1, playerOne);

            isCharacterSelect = false;
        }

        bool selectforplayertwo = (playerOne != null && playerTwo == null);
        int id = cardreader.readcard(selectforplayertwo);
        if (id == -1)
            return;


        if (playerOne == null)
        {

            playerOne = LoadCharacters(id, playerOne);
            Console.WriteLine("Player One selected: " + id.ToString());

        }
        else if (playerTwo == null)
        {
            
            playerTwo = LoadCharacters(id, playerTwo);
            Console.WriteLine("Player Two selected: " + id.ToString());

        }
    }


    Character LoadCharacters(int id, Character player)
    {
        switch (id)
        {
            case 0:
                player = new Character(characterData, id, this, new Attack(), new Attack(), "lemon_sprite_sheet.png", 7, 6);
                break;
            case 1:
                player = new Character(characterData, id, this, new Attack(), new Boomerang(), "lemon_sprite_sheet.png", 7, 6);
                break;
            case 2:
                player = new Character(characterData, id, this, new Boomerang(), new Attack(), "lemon_sprite_sheet.png", 7, 6);
                break;
        }

        return player;
    }



    public void ResetGame()
    {
        DestroyAll();

        playerOne = null;
        playerTwo = null;

        StartGame();
    }

    // For every game object, Update is called every frame, by the engine:
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