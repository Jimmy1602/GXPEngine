using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GXPEngine;
using GXPEngine.Core;

class Bar : EasyDraw
{
    EasyDraw progressBar;

    
    int max;
    int current;

    public Bar(int Pmax, int Pcurrent, int Pwidth = 500, int Pheight = 100) : base(Pwidth, Pheight, false)
    {
        max = Pmax;
        current = Pcurrent;

        createBar(Pwidth, Pheight);
    }

    void createBar(int Pwidth, int Pheight)
    {
        Clear(0);

        Fill(0, 255, 0);
        HorizontalShapeAlign = CenterMode.Min;
        VerticalShapeAlign = CenterMode.Min;
        
        Rect(0, 0, (int)(Pwidth * (current / max)), Pheight);
    }

    public void setBar(int change = 0)
    {
        current += change;
        if(current < 0)
        {
            current = 0;
        }
        else if(current > max)
        {
            current = max;
        }

        Clear(0);
        Fill(0, 255, 0);
        Rect(0, 0, (int)(width * ((float)current / max)), height);
    }
}
