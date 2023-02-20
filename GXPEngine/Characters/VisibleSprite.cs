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

    int runStartFrame;
    int runFrames;
    byte runFramesDelay;

    int attackStartFrame;
    int attackFrames;
    byte attackFramesDelay;

    int specialStartFrame;
    int specialFrames;
    byte specialFramesDelay;

    int deadFrame;

    int targetFrame;
    public bool animDone { get; private set; } = false;


    public VisibleSprite(CharacterProperties self, String imageFilename, int cols, int rows) : base(imageFilename, cols, rows, -1, false, false)
    {
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
        attackFrames = self.attackFrames;
        attackFramesDelay = self.attackFramesDelay;

        specialStartFrame = self.specialStartFrame;
        specialFrames = self.specialFrames;
        specialFramesDelay = self.specialFramesDelay;

        deadFrame = self.deadFrame;
    }

    void Update()
    {
        if(currentFrame == targetFrame - 1)
        {
            Console.WriteLine("fuck ywaaaa");
        }
    }

    public void SetIdleAnim()
    {
        if(currentAnim != "idle")
        {
            currentAnim = "idle";
            SetCycle(idleStartFrame, idleFrames, idleFrameDelay);
            targetFrame = idleStartFrame + idleFrames;
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
        }
    }
    public void SetSpecialAnim()
    {
        if (currentAnim != "special")
        {
            currentAnim = "special";
            SetCycle(specialStartFrame, specialFrames, specialFramesDelay);
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
