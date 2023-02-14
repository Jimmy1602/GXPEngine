using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GXPEngine;
using GXPEngine.Core;
class Timer
{
    int cooldown;
    int passedTime = 0;

    public Timer(int Pcooldown, bool startCompleted = false)
    {
        cooldown = Pcooldown;
        if (startCompleted)
        {
            passedTime = -cooldown;
        }
    }

    public void changeCooldown(int newCooldown)
    {
        cooldown = newCooldown;
    }

    public bool cooldownDone()
    {
        if(Time.time > passedTime + cooldown)
        {
            reset();
            return true;
        }
        return false;
    }

    public void reset()
    {
        passedTime = Time.time;
    }
}
