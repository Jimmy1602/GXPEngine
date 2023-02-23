using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using GXPEngine;
using GXPEngine.Core;
using XmlReader;

public class Boomerang : Attack
{
    private Vector2 position = new Vector2();
    private Vector2 moveVector = new Vector2();
    private int speed;
    private int backSpeed;

    public Boomerang(AttackProperties self) : base(self, "boomerang.png", 1)
    {
        attackTimer = new Timer(time, true);
        Console.WriteLine(time);
        speed = self.speed;
        backSpeed = self.backSpeed;
    }

    public override void Spawn(int direction, Character Pcaster)
    {
        if(parent != null)
        {
            visibleSprite.width = 1;
            visibleSprite.height = 1;
        }

        UniversalSpawn(Pcaster);
        attackTimer.reset();
        moveVector.x = direction * speed + caster.moveVector.x;
    }

    void Update()
    {
        if (!visible)
            return;

        position = new Vector2(x, y);
        if (attackTimer.cooldownDone())
        {
            moveBack();
        }
        else
        {
            moveAway();
        }

        collisionHandling();
    }

    void moveAway()
    {
        x += moveVector.x;
    }

    void moveBack()
    {
        if(moveTowards(getCasterPosition(), backSpeed, backSpeed))
        {
            Die();
        }
        moveVector = moveVector.subVectors(getCasterPosition(), position);
        moveVector = moveVector.setMagnetude(moveVector, backSpeed);
    }

    protected override void HitPlayer(Character target)
    {
        target.getHit(DesignerChanges.boomerangDamage, new Vector2(moveVector.x * DesignerChanges.boomerangKnockbackX, moveVector.y * DesignerChanges.boomerangKnockbackY));
    }

    protected override void Die()
    {
        caster.canAttack = true;
        visible = false;
    }
}
