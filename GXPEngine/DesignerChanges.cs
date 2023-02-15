using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using GXPEngine.Core;

public static class DesignerChanges
{
    public static int max_move_speed = 15;
    public static int move_speed_up = 1;
    public static int move_slow_down = 2;
    public static int ground_slow_down = 2;

    public static int jump_height = 25;
    public static int max_gravity = 40;
    public static int gravity = 1;

    public static int attackTime = 200;
    public static int attackCooldown = 1000;
    public static int attackDamage = 5;
    public static float attackKnockbackX = 3f;
    public static float attackKnockbackY = 3f;

    public static int boomerangFloatTime = 500;
    public static int boomerangSpeed = 10;
    public static int boomerangDamage = 2;
    public static float boomerangBaseKnockbackY = 10f;
    public static float boomerangKnockbackX = 0.3f;
    public static float boomerangKnockbackY = 0.3f;

}
