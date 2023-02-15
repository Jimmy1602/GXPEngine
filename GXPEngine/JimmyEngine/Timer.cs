using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GXPEngine;
using GXPEngine.Core;
public class Timer
{
    int cooldown;
    int passedTime = 0;

    public Timer(int Pcooldown, bool startCompleted = false)
    {
        reset();
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
            return true;
        }
        return false;
    }

    public void reset()
    {
        passedTime = Time.time;
    }

    public int timeLeft()
    {
        return Time.time - (passedTime + cooldown);
    }
}
