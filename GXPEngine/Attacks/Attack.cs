using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

using GXPEngine;
using GXPEngine.Core;

public class Attack : Sprite
{
    int xOffSet = 100;
    protected Timer attackTimer;
    protected Character caster;

    public Attack(int direction, Character Pcaster, int attackTime, String imageFilename = "circle.png") : base(imageFilename)
    {
        if(direction != 0)
            xOffSet *= direction;



        attackTimer = new Timer(attackTime);

        caster = Pcaster;
        x = getCasterPosition().x;
        y = getCasterPosition().y;
        x += xOffSet;
    }

    void Update()
    {
        if (attackTimer.cooldownDone())
        {
            Die();
        }

        if (HitTest(caster.other))
        {
            HitPlayer(caster.other);
        }
    }

    protected Vector2 getCasterPosition()
    {
        return new Vector2(caster.x + caster.width / 2, caster.y + caster.height / 2);
    }

    void HitPlayer(Character target)
    {
        target.getHit(DesignerChanges.attackDamage, new Vector2(xOffSet/100 * DesignerChanges.attackKnockbackX, -DesignerChanges.attackKnockbackY));
        caster.attacking = false;
        Die();
    }

    protected void Die()
    {
        LateDestroy();
        caster.attacking = false;
    }
}

