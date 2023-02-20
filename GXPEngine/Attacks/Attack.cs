using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using GXPEngine;
using GXPEngine.Core;
using XmlReader;

public class Attack : Sprite
{
    int xOffSet = 100;
    protected Timer attackTimer;
    protected Character caster;

    Timer windUp = new Timer(200, false);
    Timer iFrames = new Timer(300, true);
    protected bool canHit = true;

    int cooldown = DesignerChanges.attackCooldown;

    public Attack(String imageFilename = "circle.png") : base(imageFilename)
    {
        attackTimer = new Timer(200);
    }

    public virtual void Spawn(int direction, Character Pcaster)
    {
        xOffSet = direction * 100;

        UniversalSpawn(Pcaster);

        caster.attacking = true;
        x += xOffSet;
    }

    protected void UniversalSpawn(Character Pcaster)
    {
        if (parent == null)
        {
            game.AddChild(this);
        }
        visible = true;
        attackTimer.reset();
        caster = Pcaster;
        caster.canAttack = false;
        x = getCasterPosition().x;
        y = getCasterPosition().y;
    }

    void Update()
    {
        if (!visible)
            return;

        if (attackTimer.cooldownDone())
        {
            Die();
        }

        collisionHandeling();
    }

    protected void collisionHandeling()
    {
        if (!HitTest(caster.other) || iFrames.cooldownDone())
        {
            canHit = true;
        }

        if (HitTest(caster.other) && canHit && caster.other.dashTimer.cooldownDone())
        {
            iFrames.reset();
            canHit = false;
            HitPlayer(caster.other);
        }
    }

    protected Vector2 getCasterPosition()
    {
        return new Vector2(caster.x + caster.width / 2, caster.y + caster.height / 2);
    }

    protected virtual void HitPlayer(Character target)
    {
        target.getHit(DesignerChanges.attackDamage, new Vector2(xOffSet/100 * DesignerChanges.attackKnockbackX, -DesignerChanges.attackKnockbackY));
        
    }

    public int getCooldown()
    {
        return cooldown;
    }

    protected virtual void Die()
    {
        visible = false;
        caster.attacking = false;
    }
}

