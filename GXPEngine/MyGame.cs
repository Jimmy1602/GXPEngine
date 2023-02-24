using System;                                   // System contains a lot of default C# libraries 
using GXPEngine;                                // GXPEngine contains the engine
//using System.Drawing;                           // System.Drawing contains drawing tools such as Color definitions
using System.Collections.Generic;
//using GXPEngine.Core;
//using System.Drawing.Text;
//using TiledMapParser;
//using System.Diagnostics.SymbolStore;
using XmlReader;
//using System.Xml.Serialization;
//using System.Drawing.Imaging;

public class MyGame : Game {
    private cardreader cardreader = new cardreader();
    public bool isCharacterSelect = true;
    public bool isEndScreen = false;

    private Character playerOne = null;
    private Character playerTwo = null;

    private CharacterSheet characterData;
    private AttackSheet attackData;

    public Sprite Backgroundp1;
    public Sprite Backgroundp2;

    public List<Plattform> Plattforms = new List<Plattform>();

    public Sound soundSelectCard;
    public Sound soundDeath;
    public Sound soundCountdown;
    public Sound soundPunch;
    public Sound soundWhoosh;

    public Sound soundKatana;
    public Sound soundKetchup;
    public Sound soundBoomerang;

    public MyGame() : base(1366, 768, false, false, 600, 400)
	{

        characterData = xmlReader.ReadCharacterMap("Characters.xml");
        attackData = xmlReader.ReadAttackMap("Attacks.xml");
        soundSelectCard = new Sound("select_card.wav");
        soundDeath = new Sound("death.wav");
        soundCountdown = new Sound("countdown.wav");
        soundPunch = new Sound("punch.wav");
        soundWhoosh = new Sound("whoosh.wav");

        soundKatana = new Sound("katana.wav");
        soundKetchup = new Sound("ketchup.wav");
        soundBoomerang = new Sound("boomerang.wav");

        StartGame();
    }


    void StartGame()
    {
        // If you make a new plattform add it to the Plattforms List/Array, the player uses this array to check if it is grounded
        // also dont try to change plattform height pls it breaks it :(
        // also if the player moves down to fast it I'll fall throught the plattform, I hope that doesnt happen with normal movement. If that happens with some down spike its a feature :)

        RoomController roomControl = new RoomController(this);
        roomControl.CreateRandomRoom();


        isCharacterSelect = true;
        Backgroundp1 = new Sprite("bkplayer1.png"); Backgroundp1.SetXY(width / 2, height / 2);
        Backgroundp2 = new Sprite("bkplayer2.png"); Backgroundp2.SetXY(width / 2, height / 2);
        AddChild(Backgroundp2); AddChild(Backgroundp1);

    }

    void Update()
    {
        if (isEndScreen)
        {
            EndScreenUpdate();
        }
        else if (isCharacterSelect)
        {
            characterSelect();
        }
        else
        {
            Backgroundp1.visible = false; Backgroundp2.visible = false;
        }
    }

    void characterSelect()
    {   

        if (playerOne != null && playerTwo != null)
        {
            playerOne.Spawn(0, playerTwo);
            playerTwo.Spawn(1, playerOne);

            isCharacterSelect = false;
        }

        bool selectforplayertwo = (playerOne != null && playerTwo == null);

        if (selectforplayertwo)
        {
            Backgroundp1.visible = false; Backgroundp2.visible = true;
        }
        else
        {
            Backgroundp1.visible = true; Backgroundp2.visible = false;
        }

        int id = cardreader.readcard(selectforplayertwo);
        if (id == -1)
            return;


        if (playerOne == null)
        {
            soundSelectCard.Play();
            playerOne = LoadCharacters(id, playerOne);
            Console.WriteLine("Player One selected: " + id.ToString());

        }
        else if (playerTwo == null)
        {
            soundSelectCard.Play();
            playerTwo = LoadCharacters(id, playerTwo);
            Console.WriteLine("Player Two selected: " + id.ToString());
            soundCountdown.Play();
        }
    }

    AnimationSprite blender;
    Character loser;
    EasyDraw endScreenText;
    Timer endScreenTimer;

    public void EndScreen(Character loser)
    {
        isEndScreen= true;
        
        
        blender = new AnimationSprite("Blender.png", 2, 1, -1, false, false);
        blender.SetOrigin(blender.width/2, blender.height/2);
        blender.width /= 2; blender.height /= 2;
        blender.x = width / 2;
        blender.y = height / 2 - 50;
        blender.SetCycle(0, 2, 5);

        endScreenText = new EasyDraw(1200, 150, false);
        endScreenText.SetOrigin(endScreenText.width/2, endScreenText.height/2);
        endScreenText.SetXY(width/2, endScreenText.height /2);

        endScreenText.TextAlign(CenterMode.Center, CenterMode.Center);
        endScreenText.Fill(255);
        endScreenText.TextSize(100);
        endScreenText.Text(loser.playerId != 0 ? "Player One Won!!" : "Player Two Won!!");

        this.loser = loser;
        this.loser.blockMovement = true;
        this.loser.x = width/2 - 0; //this.loser.y = (height / 2) + 30;
        this.loser.pointer.LateDestroy();

        AddChild(blender);
        game.AddChild(endScreenText);

        endScreenTimer = new Timer(1000);
    }

    void EndScreenUpdate()
    {
        loser.Turn(Time.deltaTime);
        loser.y = (height / 2) + 50;
        blender.Animate();

        if (endScreenTimer.cooldownDone() && (Input.GetKeyDown(Key.V) || Input.GetKeyDown(Key.P)))
        {
            ResetGame();
        }
    }

    Character LoadCharacters(int id, Character player)
    {
        switch (id)
        {
            case 0: // apple
                player = new Character(characterData, id, this, new Attack(attackData.attacks[0]), new Attack(attackData.attacks[0]), "apple_sprite_sheet.png", 7, 2);
                break;
            case 1: // banana
                player = new Character(characterData, id, this, new Attack(attackData.attacks[0]), new Boomerang(attackData.attacks[1]), "banana_sprite_sheet.png", 22, 1);
                break;
            case 2: // lemon
                player = new Character(characterData, id, this, new Attack(attackData.attacks[0]), new Dash(attackData.attacks[3]), "lemon_sprite_sheet.png", 10, 2);
                break;
            case 3: // tomato
                player = new Character(characterData, id, this, new Attack(attackData.attacks[0]), new Boomerang(attackData.attacks[1]), "tomato_sprite_sheet.png", 7, 3);
                break;
            case 4: // orange
                player = new Character(characterData, id, this, new Attack(attackData.attacks[0]), new GroundPound(attackData.attacks[2]), "orange_sprite_sheet.png", 4, 6);
                break;
            case 5: // strawberry
                player = new Character(characterData, id, this, new Attack(attackData.attacks[0]), new Attack(attackData.attacks[0]), "strawberry_sprite_sheet.png", 7, 2);
                break;
            case 6: // raspberry
                player = new Character(characterData, id, this, new Attack(attackData.attacks[0]), new GroundPound(attackData.attacks[2]), "raspberry_sprite_sheet.png", 4, 3);
                break;
            case 7: // melon bosss
                player = new Character(characterData, id, this, new Attack(attackData.attacks[0]), new Attack(attackData.attacks[0]), "melon_sprite_sheet.png", 4, 1);
                break;
        }

        return player;
    }



    public void ResetGame()
    {
        isEndScreen= false;
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