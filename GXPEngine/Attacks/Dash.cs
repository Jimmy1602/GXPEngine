using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GXPEngine;
using GXPEngine.Core;
using XmlReader;

public class Dash : Attack
{
    int direction;
    int speed;

    public Dash(AttackProperties self) : base(self)
    {
        attackTimer = new Timer(self.time, true);
        speed = self.speed;
        y += height / 2;
    }

    public override void Spawn(int direction, Character Pcaster)
    {
        if (parent== null)
        {
            Pcaster.AddChild(this);
        }

        attackTimer.reset();
        this.direction = direction;

        offset = direction * baseOffset;
        UniversalSpawn(Pcaster);

        caster.blockMovement= true;
        x = offset + width/2;
    }

    void Update()
    {
        if (!visible)
            return;

        if (attackTimer.cooldownDone())
        {
            Console.WriteLine("hiiii");

            caster.blockMovement = false;
            Die();
        }
        else
        {
            caster.x += direction * speed;
        }

        collisionHandling();
    }
}
