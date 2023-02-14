using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GXPEngine;
using GXPEngine.Core;

class Attack : Sprite
{
    int xOffSet = 100;
    Timer attackTime;
    DesignerChanges design = new DesignerChanges();
    Character caster;

    public Attack(int direction, Character Pcaster) : base("circle.png")
    {
        if(direction != 0)
            xOffSet *= direction;

        x += xOffSet;

        attackTime = new Timer(design.attackTime);

        caster = Pcaster;
    }

    void Update()
    {
        if (attackTime.cooldownDone())
        {
            caster.attacking = false;
            LateDestroy();
        }
        
    }
}

