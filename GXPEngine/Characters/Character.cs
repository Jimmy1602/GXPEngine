﻿using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GXPEngine;
using GXPEngine.Core;

public class Character : Sprite
{
    public int player_id = 0;

    public Character other;

    protected Vector2 moveVector = new Vector2();
    protected Vector2 directionVector = new Vector2();

    protected int max_move_speed;
    protected int move_speed_up;
    protected int move_slow_down;
    protected int ground_slow_down;

    protected int jump_height;
    protected int max_gravity;
    protected int gravity;

    bool grounded = true;
    public bool attacking = false;

    int damage;

    Timer attackCooldown;

    public Character(DesignerChanges design) : base("triangle.png")
    {
        Console.WriteLine(collider);
        max_move_speed = design.max_move_speed;
        move_speed_up = design.move_speed_up;
        move_slow_down = design.move_slow_down;
        ground_slow_down = design.ground_slow_down;
        jump_height = design.jump_height;
        max_gravity = design.max_gravity;
        gravity = design.gravity;

        attackCooldown = new Timer(design.attackCooldown, true);

        y = 500;
    }

    void Update()
    {
        if (attacking)
            return;
        Vector2 inputVector = MoveInputHandeling();

        Movement(inputVector);

        Attack(directionVector);

        if (!grounded)
        {
            Fall();
        }


        x += moveVector.x;
        if(inputVector.x != 0)
        {
            directionVector.x = inputVector.x;
        }
    }

    void Attack(Vector2 inputVector)
    {
        if (Input.GetKeyDown(player_id == 0 ? Key.C : Key.COMMA) && attackCooldown.cooldownDone())
        {
            Attack attack = new Attack((int)(inputVector.x), this);
            AddChild(attack);
            attacking = true;
            attackCooldown.reset();
        }
    }

    Vector2 MoveInputHandeling()
    {
        Vector2 inputVector = new Vector2();
        if (Input.GetKey(player_id == 0 ? Key.A : Key.LEFT) && inputVector.x > -1)
        {
            inputVector.x -= 1;
        }
        else if (Input.GetKey(player_id == 0 ? Key.D : Key.RIGHT) && inputVector.x < 1)
        {
            inputVector.x += 1;
        }
        else
        {
            inputVector.x = 0;
        }

        if (Input.GetKeyDown(player_id == 0 ? Key.W : Key.RIGHT_SHIFT))
        {
            inputVector.y = -1;
        }
        else
        {
            inputVector.y = 0;
        }

        return inputVector;
    }

    void Movement(Vector2 inputVector) { 
        if (inputVector.x == -1 && moveVector.x > -max_move_speed)
        {
            moveVector.x -= move_speed_up;
        }
        if (inputVector.x == 1 && moveVector.x < max_move_speed)
        {
            moveVector.x += move_speed_up;
        }

        if (inputVector.y == -1 && grounded)
        {
            moveVector.y -= jump_height;
            grounded = false;
        }

        if (grounded)
        {
            SlowDown(inputVector, ground_slow_down);
        }
        else
        {
            SlowDown(inputVector, move_slow_down);
        }
    }

    private void SlowDown(Vector2 inputVector, int slow_down)
    {
        if(moveVector.x > slow_down && inputVector.x < 1)
        {
            moveVector.x -= slow_down;
        }
        else if(moveVector.x < -slow_down && inputVector.x > -1)
        {
            moveVector.x += slow_down;
        }
        else if(inputVector.x == 0)
        {
            moveVector.x = 0;
        }
    }

    void Fall()
    {
        if(moveVector.y < max_gravity)
        {
            moveVector.y += gravity;
        }

        y += moveVector.y;

        if (y > 500)
        {
            y = 500;
            moveVector.y = 0;
            grounded = true;
        }
    }

    public void getHit(Vector2 direction)
    {
        damage += 5;
        grounded = false;
        moveVector = direction.multiplyVector(direction, damage);
        Console.WriteLine(moveVector);
    }
}

