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
        visibleSprite = new VisibleAttackSprite("ground_effect_sprite_sheet.png", 4, 1, (byte)time);
        visibleSprite.width *= 4;
        visibleSprite.height *= 4;
        visibleSprite.x -= visibleSprite.width / 2;
        visibleSprite.y -= visibleSprite.height / 2;
        visibleSprite.x += 10;
        visibleSprite.y -= 100;


        visibleSprite.SetCycle(0, 1, 255);
    }

    public override void Spawn(int direction, Character Pcaster)
    {
        UniversalSpawn(Pcaster);

        y += offset;
        caster.canFall = false;
        caster.blockMovement = true;
        caster.moveVector.y = 1;
    }

    void Update()
    {
        if(!visible) return;


        if (!alreadyReachedGround)
        {

            if (caster.grounded)
            {
                reachedGround();
            }
            else
            {
                caster.y += speed;
                y += speed;
            }
        }
        else if(visible)
        {
            visibleSprite.Animate();
        }

        collisionHandling();

        if (visibleSprite.currentFrame == 3 && alreadyReachedGround && visible)
        {
            caster.grounded = true;
            caster.blockMovement = false;
            caster.canFall = true;
            alreadyReachedGround = false;
            visibleSprite.SetCycle(0, 1, 255);
            Die();
        }
    }

    void reachedGround()
    {
        Console.WriteLine("yeee");
        //caster.moveVector.y = 0;
        alreadyReachedGround = true;
        caster.toNextFrame();

        
        visibleSprite.SetCycle(0, 4, (byte)time);

        //attackTimer.reset();
    }

    protected override void HitPlayer(Character target)
    {
        target.getHit(damage, new Vector2(caster.x < target.x ? xKnockback : -xKnockback, -yKnockback));
    }
}
