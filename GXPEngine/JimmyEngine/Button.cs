using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GXPEngine;
using GXPEngine.Core;

class Button : Sprite
{
    EasyDraw buttonText;

    bool justTurnedVisible;

    public Button(String imageFile, int pX, int pY, int pWidth, int pHeight, String text = "", int textSize = 150, int textOffsetX = 0, int textOffsetY = 0) : base(imageFile)
    {
        x = pX;
        y = pY;

        width = pWidth;
        height = pHeight;


        if(text != "")
        {
            createText(text, textOffsetX, textOffsetY, textSize);
        }
    }

    void Update()
    {
        if (visible && justTurnedVisible && Input.GetMouseButtonUp(0))
        {
            justTurnedVisible = false;
            Console.WriteLine(justTurnedVisible);
        }
    }


    public bool pressed()
    {
        if (HitTestPoint(Input.mouseX, Input.mouseY) && Input.GetMouseButtonDown(0) && visible && !justTurnedVisible)
        {
            return true;
        }    
        return false;
    }

    void createText(String text, int textOffsetX, int textOffsetY, int textSize)
    {
        buttonText = new EasyDraw(500, 500, false);
        buttonText.Fill(0);
        buttonText.TextAlign(CenterMode.Center, CenterMode.Center);
        buttonText.TextSize(textSize);
        buttonText.Text(text);
        buttonText.SetXY(textOffsetX, textOffsetY);
        AddChild(buttonText);
    }

    void updateText(String text)
    {
        buttonText.ClearTransparent();
        buttonText.Text(text);
    }

    public void disable()
    {
        visible = false;
        justTurnedVisible = true;
    }

    public void enable() 
    {
        visible = true;
        justTurnedVisible = true;
    }
}