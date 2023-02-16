using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Drawing.Text;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using GXPEngine;
using GXPEngine.Core;

public class Character : AnimationSprite
{
    public int playerId = 0;

    public Character other;

    public Vector2 moveVector = new Vector2();
    protected Vector2 directionVector = new Vector2();

    protected int max_move_speed;
    protected float move_speed_up;
    protected float move_slow_down;
    protected float ground_slow_down;

    protected float jump_height;
    protected int max_gravity;
    protected float gravity;

    bool grounded = true;
    public bool attacking = false;
    public bool canAttack = true;
    private int dashDir = 0;

    protected bool canDoubleJump = true;
    protected bool isJumpHeld = false;
    protected Timer jumpBuffer = new Timer(DesignerChanges.jumpBuffer, true);

    Timer dashCheckTimer = new Timer(DesignerChanges.dashCheckTime, true);
    public Timer dashTimer = new Timer(DesignerChanges.dashTime, true);

    float damage;

    Timer attackCooldown = new Timer(DesignerChanges.attackCooldown, true);
    Timer jumpHoldTimer = new Timer(DesignerChanges.jumpHoldTime, true);

    EasyDraw damageDisplay = new EasyDraw(200, 200, false);

    MyGame myGame;

    Attack basicAttack;
    Attack specialAttack;

    public Character(MyGame pMyGame, Attack pBasicAttack, Attack pSpecialAttack, string imageFileName = "banana-walk.png", int rows = 7, int columns = 1) : base(imageFileName, rows, columns)
    {
        myGame = pMyGame;

        basicAttack = pBasicAttack;
        specialAttack = pSpecialAttack;

        max_move_speed = DesignerChanges.maxMoveSpeed;
        move_speed_up = DesignerChanges.moveSpeedUp;
        move_slow_down = DesignerChanges.moveSlowDown;
        ground_slow_down = DesignerChanges.groundSlowDown;
        jump_height = DesignerChanges.jumpHeight;
        max_gravity = DesignerChanges.maxGravity;
        gravity = DesignerChanges.gravity;


        width = 100;
        height = 100;
        y = 600;


        SetCycle(0, 7, 20);
    }

    public void Spawn(int pPlayerId, Character pOther)
    {

        Console.WriteLine("wtf?");
        playerId = pPlayerId;

        x = playerId == 0 ? 300 : 1000;

        other = pOther;
        
        SetupUI();
        game.AddChild(this);
    }

    void Update()
    {
        if (attacking)
        {
            SetColor(1, 0, 0);
            return;
        }
        Animate();

        Vector2 inputVector = MoveInputHandeling();

        Movement(inputVector);

        if (attackCooldown.cooldownDone())
        {
            canAttack = true;
        }

        if (Input.GetKeyDown(playerId == 0 ? Key.V : Key.COMMA) && (attackCooldown.cooldownDone() || canAttack))
        {
            Attack(directionVector);
        }
        else if (canAttack)
        {
            SetColor(1, 1, 1);
        }
        else
        {
            SetColor(1, 0, 0);
        }

        if (!grounded)
        {
            Fall();
        }


        x += moveVector.x;
        if(inputVector.x != 0)
        {
            directionVector.x = inputVector.x;

            if (inputVector.x < 0)
            {
                Mirror(true, false);
            }
            else
            {
                Mirror(false, false);
            }
        }

        Die();
    }

   
    void Attack(Vector2 inputVector)
    {
        Attack attack;
        if (grounded)
        {
            attack = basicAttack;
        }
        else
        {
            attack = specialAttack;
        }

        attackCooldown.reset();
        canAttack = false;
        attack.Spawn(_mirrorX ? -1 : 1, this);
    }

    Vector2 MoveInputHandeling()
    {
        Vector2 inputVector = new Vector2();
        if (Input.GetKey(playerId == 0 ? Key.A : Key.LEFT) && inputVector.x > -1)
        {
            inputVector.x -= 1;
        }
        else if (Input.GetKey(playerId == 0 ? Key.D : Key.RIGHT) && inputVector.x < 1)
        {
            inputVector.x += 1;
        }
        else
        {
            inputVector.x = 0;
        }
        
        Dash();

        if (Input.GetKeyDown(playerId == 0 ? Key.W : Key.RIGHT_SHIFT))
        {
            inputVector.y = -1;
        }
        else
        {
            inputVector.y = 0;
        }

        return inputVector;
    }

    void Dash()
    {
        if(Input.GetKeyDown(playerId == 0 ? Key.A : Key.LEFT) && dashTimer.cooldownDone())
        {
            if(dashCheckTimer.cooldownDone())
            {
                dashDir = -1;
                dashCheckTimer.reset();
            }
            else if (dashDir == -1)
            {
                dashTimer.reset();
            }
        }
        else if (Input.GetKeyDown(playerId == 0 ? Key.D : Key.RIGHT) && dashTimer.cooldownDone())
        {
            if (dashCheckTimer.cooldownDone())
            {
                dashDir = 1;
                dashCheckTimer.reset();
            }
            else if (dashDir == 1)
            {
                dashTimer.reset();
            }
        }


        if (!dashTimer.cooldownDone())
        {
            alpha = 0.5f;
            moveVector.x = dashDir * DesignerChanges.dashSpeed;
        }
        else
        {
            alpha = 1;
        }
    }

    void Movement(Vector2 inputVector) { 
        if (inputVector.x == -1 && moveVector.x > -max_move_speed)
        {
            moveVector.x -= move_speed_up;
        }
        else if (inputVector.x == 1 && moveVector.x < max_move_speed)
        {
            moveVector.x += move_speed_up;
        }

        JumpLogic(inputVector);

        if (grounded)
        {
            canDoubleJump = true;
            SlowDown(inputVector, ground_slow_down);
        }
        else
        {
            SlowDown(inputVector, move_slow_down);
        }
    }

    private void JumpLogic(Vector2 inputVector)
    {
        if (inputVector.y == -1)
        {
            if (grounded)
            {
                canDoubleJump = true;
                Jump();
            }
            else if (canDoubleJump)
            {
                canDoubleJump = false;
                Jump();
            }
            else
            {
                jumpBuffer.reset();
            }
        }
        else if (!jumpHoldTimer.cooldownDone())
        {
            moveVector.y -= DesignerChanges.jumpHoldHeight;
        }

        if(grounded && !jumpBuffer.cooldownDone())
        {
            Jump();
        }

        if(!Input.GetKey(playerId == 0 ? Key.W : Key.RIGHT_SHIFT))
        {
            jumpHoldTimer.forceCompleteCooldown();
        }
    }

    private void Jump()
    {
        moveVector.y = -jump_height;
        grounded = false;
        jumpHoldTimer.reset();
    }

    private void SlowDown(Vector2 inputVector, float slow_down)
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

        if (y > 600)
        {
            y = 600;
            moveVector.y = 0;
            grounded = true;
        }
    }

    public void getHit(float dmg, Vector2 direction)
    {
        damage += dmg;
        UpdateUI();
        grounded = false;
        moveVector = moveVector.addVectors(moveVector, direction.multiplyVector(direction, damage));
    }

    private void SetupUI()
    {
        damageDisplay.SetOrigin(0, 0);
        damageDisplay.SetXY(playerId == 0 ? 0 : game.width - damageDisplay.width, 0);

        damageDisplay.TextAlign(CenterMode.Center, CenterMode.Center);
        damageDisplay.Fill(255);
        damageDisplay.TextSize(50);
        damageDisplay.Text(damage.ToString());
        game.AddChild(damageDisplay);
    }

    private void UpdateUI()
    {
        damageDisplay.ClearTransparent();
        damageDisplay.Text(damage.ToString());
    }

    private void Die()
    {
        if(x < -150 || x > game.width + 150 || y < -150)
        {
            myGame.ResetGame();
        }
    }
}

