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
using TiledMapParser;
using XmlReader;

public class Character : Sprite
{
    public int playerId = 0;

    public int ground = 600;

    int maxOutside = 250;

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

    public bool grounded {  set;  get; } = true;
    public bool attacking = false;
    public bool specialing = false;
    public bool canAttack = true;
    private int dashDir = 0;

    protected bool canDoubleJump = true;
    protected bool isJumpHeld = false;
    protected Timer jumpBuffer = new Timer(DesignerChanges.jumpBuffer, true);

    Timer dashCheckTimer;
    public Timer dashTimer;
    int dashSpeed;

    float damage;

    public bool canFall { set; private get; } = true;

    Timer attackCooldown = new Timer(DesignerChanges.attackCooldown, true);
    Timer jumpHoldTimer;

    EasyDraw damageDisplay = new EasyDraw(200, 200, false);

    MyGame myGame;

    VisibleSprite visibleSprite;

    Attack basicAttack;
    Attack specialAttack;

    GroundCollision groundCollision;

    public bool isMirrored { get; private set; } = false;

    public bool blockMovement { set; private get; } = false;


    public Character(CharacterSheet characterdata, int characterId, MyGame pMyGame, Attack pBasicAttack, Attack pSpecialAttack, string imageFileName, int columns, int rows) : base("CharacterRect.png", false, true, false)
    {

        myGame = pMyGame;
        groundCollision = new GroundCollision(this, myGame);
        AddChild(groundCollision);

        basicAttack = pBasicAttack;
        specialAttack = pSpecialAttack;

        CharacterProperties self = characterdata.characters[characterId];
        visibleSprite = new VisibleSprite(this, self, imageFileName, columns, rows);
        AddChild(visibleSprite);
        AddChild(groundCollision);
        max_move_speed = self.maxMoveSpeed;
        move_speed_up = self.moveSpeedUp;
        move_slow_down = self.moveSlowDown;
        ground_slow_down = self.groundSlowDown;

        dashCheckTimer = new Timer(self.dashCheckTime, true);
        dashTimer = new Timer(self.dashTime, true);
        dashSpeed = self.dashSpeed;

        jump_height = self.jumpHeight;
        jumpHoldTimer = new Timer(self.jumpHoldTime, true);

        max_gravity = self.maxGravity;
        gravity = self.gravity;
        


        width = 100;
        height = 100;
        //y = ground;
        y = 400;
    }

    public Pointer pointer;

    public void Spawn(int pPlayerId, Character pOther)
    {
        playerId = pPlayerId;

        x = playerId == 0 ? 300 : 1000;

        other = pOther;

        string playerheaderfile = "player1.png"; if (pPlayerId == 1) { playerheaderfile = "player2.png"; }
        pointer = new Pointer(playerheaderfile,this);

        //header.SetXY(this.x,this.y);

        SetupUI();
        game.AddChild(this);
    }

    void Update()
    {
        if (myGame.isEndScreen)
            return;

        if (playerId == 0)
        {
        }

        if(!blockMovement)
        {
            //if (myGame.soundCountdown)
            //{
                Vector2 inputVector = MoveInputHandeling();
                Movement(inputVector);
                Mirror(inputVector);
                x += moveVector.x;
            //}
                if (!grounded && canFall)
                {
                    Fall();
                }
        }

        if (attackCooldown.cooldownDone())
        {
            canAttack = true;
        }

        if (Input.GetKeyDown(playerId == 0 ? Key.V : Key.P) && (attackCooldown.cooldownDone() || canAttack))
        {
            Attack(directionVector);
        }


        



        animationLogic();
        visibleSprite.Animate();

        Die();
    }

    void Mirror(Vector2 inputVector)
    {
        if(inputVector.x != 0)
        {
            directionVector.x = inputVector.x;

            if (inputVector.x < 0)
            {
                visibleSprite.Mirror(true, false);
                isMirrored = true;
            }
            else
            {
                visibleSprite.Mirror(false, false);
                isMirrored = false;
            }
        }
    }
   
    void Attack(Vector2 inputVector)
    {
        //Attack attack;
        if (grounded)
        {
            attacking = true;
            //attack = basicAttack;
        }
        else
        {
            specialing = true;
            //attack = specialAttack;
        }
        //attack.Spawn(visibleSprite.isMirrored() ? -1 : 1, this);
    }

    public void spawnAttack(bool special)
    {
        Attack attack = special ? specialAttack : basicAttack;

        if (!attack.visible)
        {
            myGame.soundWhoosh.Play();
            attack.Spawn(visibleSprite.isMirrored() ? -1 : 1, this);
        }
    }

    Vector2 MoveInputHandeling()
    {
        Vector2 inputVector = new Vector2();
        if (Input.GetKey(playerId == 0 ? Key.A : Key.J) && inputVector.x > -1)
        {
            inputVector.x -= 1;
        }
        else if (Input.GetKey(playerId == 0 ? Key.D : Key.L) && inputVector.x < 1)
        {
            inputVector.x += 1;
        }
        else
        {
            inputVector.x = 0;
        }
        
        Dash();

        if (Input.GetKeyDown(playerId == 0 ? Key.W : Key.I))
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
        if(Input.GetKeyDown(playerId == 0 ? Key.A : Key.J) && dashTimer.cooldownDone())
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
        else if (Input.GetKeyDown(playerId == 0 ? Key.D : Key.L) && dashTimer.cooldownDone())
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
            moveVector.x = dashDir * dashSpeed;
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

        if(!Input.GetKey(playerId == 0 ? Key.W : Key.I))
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

        /*
        if (y > ground)
        {
            y = ground;
            moveVector.y = 0;
            grounded = true;
        }
        */
    }

    public void getHit(float dmg, Vector2 direction)
    {
        damage += dmg;
        UpdateUI();
        grounded = false;
        //moveVector = moveVector.addVectors(moveVector, direction.multiplyVector(direction, damage));
        moveVector = direction.multiplyVector(direction, damage);
        myGame.soundPunch.Play();
    }

    private void SetupUI()
    {
        damageDisplay.SetOrigin(0, 0);
        damageDisplay.SetXY(playerId == 0 ? 0 : game.width - damageDisplay.width, 0);

        damageDisplay.TextAlign(CenterMode.Center, CenterMode.Center);
        damageDisplay.Fill(0);
        damageDisplay.TextSize(70);
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
        if(x < -maxOutside || x > game.width + maxOutside ||  y > game.height + maxOutside)
        {
            myGame.soundDeath.Play();
            myGame.EndScreen(this);
        }
    }


    private void animationLogic()
    {
        if (!attacking && !specialing && moveVector.x == 0 && grounded)
        {
            visibleSprite.SetIdleAnim();
        }
        else if (grounded && !attacking && !specialing)
        {
            visibleSprite.SetRunAnim();
        }
        else if ((!grounded || !dashTimer.cooldownDone()) && !attacking && !specialing)
        {
            visibleSprite.SetJumpAnim();
        }
        else if (specialing)
        {
            if(specialAttack.staticAnim)
            {
                visibleSprite.SetStaticSpecialAnim();
            }
            else
            {
                visibleSprite.SetSpecialAnim();
            }
        }
        else if (attacking)
        {
            visibleSprite.SetAttackAnim();
        }
    }

    public void toNextFrame()
    {
        visibleSprite.NextFrame();
    }
}

