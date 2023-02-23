using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GXPEngine;
using GXPEngine.Core;
using XmlReader;

class VisibleSprite : AnimationSprite
{
    String currentAnim;

    int idleStartFrame;
    int idleFrames;
    byte idleFrameDelay;

    int jumpFrame;

    public int runStartFrame;
    int runFrames;
    byte runFramesDelay;

    int attackStartFrame;
    int attackHitFrame;
    int attackFrames;
    byte attackFramesDelay;

    int specialStartFrame;
    int specialHitFrame;
    int specialFrames;
    byte specialFramesDelay;

    int deadFrame;

    int targetFrame;
    int startFrame;

    bool looped = false;
    public bool animDone { get; private set; } = false;

    public bool attackHit = false;
    public bool specialHit = false;

    Character owner;

    public VisibleSprite(Character pOwner, CharacterProperties self, String imageFilename, int cols, int rows) : base(imageFilename, cols, rows, -1, false, false)
    {
        owner = pOwner;
        width = 120;
        height = 120;

        idleStartFrame = self.idleStartFrame;
        idleFrames = self.idleFrames;
        idleFrameDelay = self.idleFrameDelay;

        jumpFrame = self.jumpFrame;

        runStartFrame = self.runStartFrame;
        runFrames = self.runFrames;
        runFramesDelay = self.runFramesDelay;

        attackStartFrame = self.attackStartFrame;
        attackHitFrame = self.attackHitFrame;
        attackFrames = self.attackFrames;
        attackFramesDelay = self.attackFramesDelay;

        specialStartFrame = self.specialStartFrame;
        specialHitFrame = self.specialHitFrame;
        specialFrames = self.specialFrames;
        specialFramesDelay = self.specialFramesDelay;

        deadFrame = self.deadFrame;
    }

    public bool isMirrored()
    {
        return _mirrorX;
    }

    void Update()
    {
        if(currentFrame == targetFrame - 1)
        {
            looped = true;
        }
        else if (looped && currentFrame == startFrame)
        {
            animDone = true;
            if(currentAnim == "attack" || currentAnim == "special")
            {
                owner.attacking = false;
                owner.specialing = false;
            }
        }

        if (currentFrame == attackHitFrame)
        {
            owner.spawnAttack(false);
        }
        else if (currentFrame == specialHitFrame)
        {
            owner.spawnAttack(true);
        }
    }

    public void SetIdleAnim()
    {
        if(currentAnim != "idle")
        {
            currentAnim = "idle";
            SetCycle(idleStartFrame, idleFrames, idleFrameDelay);
        }
    }
    public void SetRunAnim()
    {
        if (currentAnim != "run")
        {
            currentAnim = "run";
            SetCycle(runStartFrame, runFrames, runFramesDelay);
        }
    }
    public void SetJumpAnim()
    {
        if (currentAnim != "jump")
        {
            currentAnim = "jump";
            SetCycle(jumpFrame, 1);
        }
    }
    public void SetAttackAnim()
    {
        if (currentAnim != "attack")
        {
            currentAnim = "attack";
            SetCycle(attackStartFrame, attackFrames, attackFramesDelay);
            targetFrame = attackStartFrame + attackFrames;
            startFrame = attackStartFrame;

            looped = false;
            animDone = false;
        }
    }
    public void SetStaticSpecialAnim()
    {
        if (currentAnim != "special")
        {
            currentAnim = "special";
            SetCycle(specialStartFrame, 1);
            targetFrame = specialStartFrame;
            startFrame = specialStartFrame;
            
            looped = false;
            animDone = false;
        }
    }

    public void SetSpecialAnim()
    {
        if (currentAnim != "special")
        {
            currentAnim = "special";
            SetCycle(specialStartFrame, specialFrames, specialFramesDelay);
            targetFrame = specialStartFrame + specialFrames;
            startFrame = specialStartFrame;

            looped = false;
            animDone = false;
        }
    }

    public void SetDeadAnim()
    {
        if (currentAnim != "dead")
        {
            currentAnim = "dead";
            SetCycle(deadFrame, 1);
        }
    }
}
