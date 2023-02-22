using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GXPEngine;
using GXPEngine.Core;
using XmlReader;

class GroundPound : Attack
{
    int speed;

    public GroundPound(AttackProperties self) : base(self)
    {
        staticAnim = true;
        speed = self.speed;
    }

    public override void Spawn(int direction, Character Pcaster)
    {
        UniversalSpawn(Pcaster);

        y += offset;
    }

    void Update()
    {
        if(!visible) return;

        caster.y += speed;
        y += speed;

        if (caster.grounded)
        {
            Die();
        }

        collisionHandling();
    }
}
