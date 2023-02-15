using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
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

        x += xOffSet;

        attackTimer = new Timer(attackTime);

        caster = Pcaster;
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

