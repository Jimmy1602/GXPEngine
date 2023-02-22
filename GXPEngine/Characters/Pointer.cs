using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using GXPEngine;
using GXPEngine.Core;

public class Pointer : Sprite 
{
    Character myLord;
    public Pointer(String playerheaderfile,Character thischar) : base(playerheaderfile)
    {
        myLord = thischar;
        game.AddChild(this);

        this.SetXY(game.width/2,game.height/2);
        this.SetOrigin(this.width / 2, this.height);
        this.rotation = 90;
    }

    void Update()
    {
        int mylordcenterx = Mathf.Round(myLord.x) + Mathf.Round(myLord.width/2);
        int mylordcentery = Mathf.Round(myLord.y) + Mathf.Round(myLord.height/2);
        this.rotation = 0;
        x = mylordcenterx;
        y = myLord.y-30;
        if (myLord.x < 0)
        {
            this.rotation = 90;
            this.x = 0;
            this.y = mylordcentery;
        }
        else
        if(myLord.x> game.width)
        {
            this.rotation = 270;
            this.x = game.width;
            this.y = mylordcentery;
        }
        else
        if(myLord.y<0)
        {
            this.rotation = 180;
            this.y = 0;
        }
        else
        if (myLord.y > game.height)
        {
            this.y = game.height;
        }

    }

}