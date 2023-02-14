using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GXPEngine.Core;

public class DesignerChanges
{
    public int max_move_speed = 30;
    public int move_speed_up = 4;
    public int move_slow_down = 2;
    public int ground_slow_down = 5;

    public int jump_height = 70;
    public int max_gravity = 100;
    public int gravity = 5;

    public int attackTime = 200;
    public int attackCooldown = 1000;

    public Character playerOne;
    public Character playerTwo;
}
