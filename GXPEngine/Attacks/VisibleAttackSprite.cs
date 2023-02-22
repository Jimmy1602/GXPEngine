using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GXPEngine;
using GXPEngine.Core;

public class VisibleAttackSprite : AnimationSprite
{
    
    public VisibleAttackSprite(String imageFileName, int cols, int rows) : base (imageFileName, cols, rows, -1, false, false)
    {
    }
}
