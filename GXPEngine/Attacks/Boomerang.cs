using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using GXPEngine;
using GXPEngine.Core;

public class Boomerang : Attack
{
    private Vector2 position = new Vector2();
    private Vector2 moveVector = new Vector2();
    private int speed;

    

    public Boomerang(int attackTime) : base(attackTime)
    {
        speed = DesignerChanges.boomerangSpeed;
    }

    public override void Spawn(int direction, Character Pcaster)
    {
        UniversalSpawn(Pcaster);
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

        collisionHandeling();
    }

    void moveAway()
    {
        x += moveVector.x;
    }

    void moveBack()
    {
        if(moveTowards(getCasterPosition(), DesignerChanges.boomerangBackSpeed, DesignerChanges.boomerangBackSpeed))
        {
            Die();
        }
        moveVector = moveVector.subVectors(getCasterPosition(), position);
        moveVector = moveVector.setMagnetude(moveVector, DesignerChanges.boomerangBackSpeed);
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
