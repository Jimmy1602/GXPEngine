using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GXPEngine;
using GXPEngine.Core;

public class VisibleAttackSprite : AnimationSprite
{
    int cols;

    bool looped = false;
    public bool animDone { get; private set; } = false;

    byte time;

    bool isMirrored = false;
    public bool previousMirrored = false;

    public VisibleAttackSprite(String imageFileName, int pCols, int rows, byte pTime) : base (imageFileName, pCols, rows, -1, false, false)
    {
        cols = pCols;
        time = pTime;
    }

    void Update()
    {
        if (currentFrame == cols - 1)
        {
            looped = true;
        }
        else if (looped && currentFrame == 0)
        {
            animDone = true;
        }
    }

    public void setAnim()
    {
        SetCycle(0, cols, time);
        looped = false;
        animDone = false;
    }

    public void doMirror(bool casterMirror)
    {
        isMirrored = casterMirror;
        Mirror(casterMirror, false); 

        mirrorAlign();
        previousMirrored = casterMirror;
    }

    void mirrorAlign()
    {
        if (previousMirrored == isMirrored) return;

        if (isMirrored)
        {
            x -= 78;
        }
        else
        {
            x += 78;
        }
    }
}
