using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using GXPEngine;
using GXPEngine.Core;

public class Character : AnimationSprite
{
    public int player_id = 0;

    public Character other;

    public Vector2 moveVector = new Vector2();
    protected Vector2 directionVector = new Vector2();

    protected int max_move_speed;
    protected int move_speed_up;
    protected int move_slow_down;
    protected int ground_slow_down;

    protected int jump_height;
    protected int max_gravity;
    protected int gravity;

    protected string singleAttackType;
    protected string specialAttackType;

    bool grounded = true;
    public bool attacking = false;
    public bool canAttack = true;

    int damage;

    Timer attackCooldown;

    EasyDraw damageDisplay = new EasyDraw(200, 200, false);

    MyGame myGame;

    Attack basicAttack;
    Attack specialAttack;

    public Character(int pPlayerId, MyGame pMyGame, Attack pBasicAttack, Attack pSpecialAttack, int rows = 4, int columns = 1, string imageFileName = "lemonster-stand.png") : base(imageFileName, rows, columns)
    {
        player_id = pPlayerId;
        myGame = pMyGame;

        basicAttack = pBasicAttack;
        specialAttack = pSpecialAttack;

        max_move_speed = DesignerChanges.max_move_speed;
        move_speed_up = DesignerChanges.move_speed_up;
        move_slow_down = DesignerChanges.move_slow_down;
        ground_slow_down = DesignerChanges.ground_slow_down;
        jump_height = DesignerChanges.jump_height;
        max_gravity = DesignerChanges.max_gravity;
        gravity = DesignerChanges.gravity;

        attackCooldown = new Timer(DesignerChanges.attackCooldown, true);

        singleAttackType = "normal";
        specialAttackType = "boomerang";

   
        y = 600;
        width = width * 2;
        height = height * 2;
        y = 600;

        SetupUI();

    }

    void Update()
    {
        if (attacking)
        {
            SetColor(1, 0, 0);
            return;
        }

        Vector2 inputVector = MoveInputHandeling();

        Movement(inputVector);

        if (attackCooldown.cooldownDone())
        {
            canAttack = true;
        }

        if (Input.GetKeyDown(player_id == 0 ? Key.V : Key.COMMA) && (attackCooldown.cooldownDone() || canAttack))
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

        if (y > 600)
        {
            y = 600;
            moveVector.y = 0;
            grounded = true;
        }
    }

    public void getHit(int dmg, Vector2 direction)
    {
        damage += dmg;
        UpdateUI();
        grounded = false;
        moveVector = direction.multiplyVector(direction, damage);
    }

    private void SetupUI()
    {
        damageDisplay.SetOrigin(0, 0);
        damageDisplay.SetXY(player_id == 0 ? 0 : game.width - damageDisplay.width, 0);

        Console.WriteLine(damageDisplay.y);

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

