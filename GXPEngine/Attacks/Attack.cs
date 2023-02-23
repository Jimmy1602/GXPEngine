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
    protected Timer attackTimer;
    protected Character caster;
    protected float damage;
    protected float xKnockback;
    protected float yKnockback;

    public int offset;
    private int baseOffset;

    Timer iFrames;
    protected bool canHit = true;

    int cooldown;

    public bool staticAnim = false;
    public bool basic = false;

    protected VisibleAttackSprite visibleSprite;

    protected int time;

    public Attack(AttackProperties self, String visibleImageFile = "attack_effect_sprite_sheet.png", int visibleSpriteCols = 3, String imageFilename = "CharacterRect.png") : base(imageFilename)
    {
        //time = (byte)self.time;
        //attackTimer = new Timer(self.time);
        iFrames = new Timer(self.iMillis, true);
        cooldown = self.cooldown;
        damage = self.damage;
        xKnockback = self.xKnockback;
        yKnockback = self.yKnockback;
        offset = self.offset;
        baseOffset = offset;

        visibleSprite = new VisibleAttackSprite(visibleImageFile, visibleSpriteCols, 1, (byte)self.time);
        basic = self.special == 0 ? true : false;
        visible = false;

    }

    public virtual void Spawn(int direction, Character Pcaster)
    {
        if(parent == null)
        {

            visibleSprite.SetXY((-width / 2) - 120, (-height / 2) - 40);
            visibleSprite.width *= 5;

        }

        offset = direction * baseOffset;
        UniversalSpawn(Pcaster);

        x += offset;

        visibleSprite.doMirror(caster.isMirrored);

        visibleSprite.setAnim();

        caster.blockMovement = true;
    }

    protected void UniversalSpawn(Character Pcaster)
    {
        if (parent == null)
        {
            AddChild(visibleSprite);
            game.AddChild(this);
        }
        visible = true;
        //attackTimer.reset();
        caster = Pcaster;
        caster.canAttack = false;



        x = getCasterPosition().x;
        y = getCasterPosition().y;
    }

    void Update()
    {
        if (!visible)
            return;

        visibleSprite.Animate();

        
        if (visibleSprite.animDone)//attackTimer.cooldownDone())
        {
            caster.blockMovement = false;
            Die();
        }
        

        collisionHandling();
    }

    protected void collisionHandling()
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
        target.getHit(damage, new Vector2(offset/100 * xKnockback, -yKnockback));
        
    }

    public int getCooldown()
    {
        return cooldown;
    }

    protected virtual void Die()
    {
        visible = false;

        if (basic)
        {
            caster.attacking = false;
        }
        else
        {
            caster.specialing = false;
        }
    }
}

