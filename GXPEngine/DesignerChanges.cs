using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using GXPEngine.Core;

public static class DesignerChanges
{
    public static int maxMoveSpeed = 15;
    public static float moveSpeedUp = 1;
    public static float moveSlowDown = 1;
    public static float groundSlowDown = 2;

    public static int dashCheckTime = 250;
    public static int dashTime = 100;
    public static int dashSpeed = 30;
    public static int jumpBuffer = 300;

    public static float jumpHeight = 10;
    public static int jumpHoldTime = 150;
    public static float jumpHoldHeight = 1;


    public static int maxGravity = 40;
    public static float gravity = 0.5f;

    public static int attackTime = 200;
    public static int attackCooldown = 1000;
    public static float attackDamage = 3;
    public static float attackKnockbackX = 2;
    public static float attackKnockbackY = 2;

    public static int boomerangFloatTime = 500;
    public static int boomerangSpeed = 10;
    public static int boomerangBackSpeed = 20;
    public static int boomerangCooldown = 1002;
    public static float boomerangDamage = 1;
    public static float boomerangKnockbackX = 0.15f;
    public static float boomerangKnockbackY = 0.15f;

}