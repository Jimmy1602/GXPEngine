using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GXPEngine;
using GXPEngine.Core;

public class Boomerang : Attack
{
    private Vector2 position = new Vector2();
    private Vector2 moveVector = new Vector2();
    private int speed;

    public Boomerang(int direction, Character Pcaster, int attackTime) : base(direction, Pcaster, attackTime)
    {
        x = caster.x; y = caster.y;
        speed = DesignerChanges.boomerangSpeed;
        moveVector.x = direction * speed + caster.moveVector.x;
    }


    void Update()
    {
        position = new Vector2(x, y);
        if (attackTimer.cooldownDone())
        {
            moveBack();
        }
        else
        {
            moveAway();
        }

        if (HitTest(caster.other))
        {
            HitPlayer(caster.other);
        }
    }

    void moveAway()
    {
        x += moveVector.x;
    }

    void moveBack()
    {
        if(moveTowards(new Vector2(caster.x, caster.y), speed * 2, speed * 2))
        {
            Die();
        }
        moveVector = moveVector.subVectors(position, caster.moveVector);
        moveVector = moveVector.setMagnetude(moveVector, 1);
    }

    void HitPlayer(Character target)
    {
        target.getHit(DesignerChanges.boomerangDamage, new Vector2(moveVector.x * DesignerChanges.boomerangKnockbackX, moveVector.y * DesignerChanges.boomerangKnockbackY));
    }

    new void Die()
    {
        caster.canAttack = true;
        LateDestroy();
    }
}
