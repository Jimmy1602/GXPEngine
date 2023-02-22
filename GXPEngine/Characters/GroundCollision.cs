using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GXPEngine;
using GXPEngine.Core;

public class GroundCollision : Sprite
{
    Character owner;

    public GroundCollision(Character pOwner) : base("BlackSquare.png", false, true, false)
    {
        owner = pOwner;

        width = owner.width;

        height = 2;

        y += owner.height;
    }

    void Update()
    {
        Console.WriteLine(GetCollisions().ToString());
    }

}
