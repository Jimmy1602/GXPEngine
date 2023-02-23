using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GXPEngine;
using GXPEngine.Core;

public class Plattform : Sprite
{

    Sprite visibleSprite;
    public Plattform(String imageFileName, int pX, int pY, int pWidth = 1000) : base("BlackSquare.png")//"CharacterRect.png")
    {
        visibleSprite = new Sprite(imageFileName);

        width = pWidth;
        height = 51;
       
        x = pX;
        y = pY;

        visibleSprite.width = 100;
        visibleSprite.height = 200;
        visibleSprite.y = 50;
        AddChild(visibleSprite);
    }
}
