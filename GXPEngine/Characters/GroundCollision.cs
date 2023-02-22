using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GXPEngine;
using GXPEngine.Core;

public class GroundCollision : Sprite
{
    MyGame myGame;

    Character owner;

    private bool previouslyOn = false;

    public GroundCollision(Character pOwner, MyGame pMyGame) : base("CharacterRect.png", false, true, false)
    {
        owner = pOwner;
        myGame = pMyGame;

        width = owner.width;

        height = 2;

        y += owner.height;
    }

    void Update()
    {
        int number = 0;
        foreach(Plattform p in myGame.Plattforms)
        {
            if (HitTest(p) && owner.moveVector.y > 0)
            {
                previouslyOn = true;
                owner.grounded = true;
                owner.y = p.y - p.height - owner.height / 2;
            }
            else
            {
                number++;
            }
        }

        if(number == myGame.Plattforms.Count)
        {
            owner.grounded = false;

            if(previouslyOn && owner.moveVector.y > 0)
            {
                owner.moveVector.y = 0;
            }
            previouslyOn = false;
        }
    }

}
