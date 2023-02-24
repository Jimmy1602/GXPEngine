using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GXPEngine;
using GXPEngine.Core;
using TiledMapParser;
using XmlReader;


public class RoomController
{
    Random random = new Random();
    MyGame gameLeader;
    public RoomController(MyGame pMyGame)
    {
        this.gameLeader = pMyGame;
    }
    public void CreateRoom(int RoomID)
    {
        Room newroom;
        switch (RoomID)
        {
            case 0:
                newroom = new Room(this.gameLeader, "Background.png", "Plattform.png");
                newroom.addPlatform("Plattform.png", 280, 400, 350);
                newroom.addPlatform("Plattform.png", gameLeader.width-280, 400, 350);
                break;
            case 1:
                newroom = new Room(this.gameLeader, "background2.png", "Platform2.png");
                newroom.addPlatform("Platform2.png", gameLeader.width/2, 350, 450);
                break;
            case 2:
                newroom = new Room(this.gameLeader, "background3.png", "Platform3.png");
                break;
            case 3:
                newroom = new Room(this.gameLeader, "Background.png", "Plattform.png");
                newroom.addPlatform("Plattform.png", 280, 400, 350);
                newroom.addPlatform("Plattform.png", gameLeader.width - 280, 400, 350);
                break;
        }
    }
    public void CreateRandomRoom()
    {
        //CreateRoom(1);
        CreateRoom(random.Next(0, 3));
    }

}
public class Room 
{
    MyGame gameLeader;
    public Room(MyGame pMyGame, String BackgroundImage, String PlatformImage)
    {
        this.gameLeader = pMyGame;
        Sprite background = new Sprite(BackgroundImage, false, false, false);
        Plattform plattform = new Plattform(PlatformImage, gameLeader.width / 2, gameLeader.height - 100);
        gameLeader.Plattforms.Add(plattform);
        background.width = gameLeader.width; background.height = gameLeader.height;

        gameLeader.AddChild(background);
        gameLeader.AddChild(plattform);

        //addPlatform("Plattform.png",400,400,600);
    }

    public void addPlatform(String image, int px, int py,int width)
    {
        Plattform plattform = new Plattform(image, px, py, width);

        gameLeader.Plattforms.Add(plattform);

        gameLeader.AddChild(plattform);
    }
} 