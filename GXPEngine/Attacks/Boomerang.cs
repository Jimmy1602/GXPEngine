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

    Timer IFrames = new Timer(400, true);
    private bool canHit = true;

    public Boomerang(int direction, Character Pcaster, int attackTime) : base(direction, Pcaster, attackTime)
    {
        x = GetCasterPosition().x;
        y = GetCasterPosition().y;
        speed = DesignerChanges.boomerangSpeed;
        moveVector.x = direction * speed + caster.moveVector.x;
    }


    void Update()
    {
        position = new Vector2(x, y);
        if (attackTimer.cooldownDone())
        {
            MoveBack();
        }
        else
        {
            MoveAway();
        }

        if (IFrames.cooldownDone() || !HitTest(caster.other))
        {
            canHit = true;
        }

        if (HitTest(caster.other) && canHit)
        {
            IFrames.reset();
            canHit = false;
            HitPlayer(caster.other);
        }
    }

    void MoveAway()
    {
        x += moveVector.x;
    }

    void MoveBack()
    {
        if(moveTowards(GetCasterPosition(), speed * 2, speed * 2))
        {
            Die();
        }
        moveVector = moveVector.subVectors(GetCasterPosition(), position);
        moveVector = moveVector.setMagnetude(moveVector, speed * 2);
    }

    void HitPlayer(Character target)
    {

        target.GetHit(DesignerChanges.boomerangDamage, new Vector2(moveVector.x * DesignerChanges.boomerangKnockbackX, moveVector.y - DesignerChanges.boomerangBaseKnockbackY * DesignerChanges.boomerangKnockbackY));
    }

    new void Die()
    {
        caster.canAttack = true;
        LateDestroy();
    }
}
