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

    bool alreadyReachedGround = false;

    public GroundPound(AttackProperties self) : base(self)
    {
        staticAnim = true;
        speed = self.speed;
    }

    public override void Spawn(int direction, Character Pcaster)
    {
        UniversalSpawn(Pcaster);

        y += offset;
        caster.canFall = false;
        caster.blockMovement = true;
    }

    void Update()
    {
        if(!visible) return;


        if (!alreadyReachedGround)
        {
            caster.y += speed;
            y += speed;

            if (caster.y > caster.ground)
            {
                reachedGround();
            }
        }


        collisionHandling();

        if (attackTimer.cooldownDone() && alreadyReachedGround && visible)
        {
            caster.grounded = true;
            caster.blockMovement = false;
            caster.canFall = true;
            alreadyReachedGround = false;
            width /= 3;
            height /= 3;
            Die();
        }
    }

    void reachedGround()
    {
        alreadyReachedGround = true;
        caster.y = caster.ground;
        width *= 3;
        height *= 3;
        caster.toNextFrame();

        attackTimer.reset();
    }

    protected override void HitPlayer(Character target)
    {
        target.getHit(damage, new Vector2(y < target.y ? xKnockback : -xKnockback, -yKnockback));
    }
}
