using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GXPEngine;
using GXPEngine.Core;

public class cardreader
{
    public int readcard(bool selectingforplayer2)
    {
        int characterID = -1;
       // if (Input.GetKeyDown(Key.EQUALS))
        //{
            // if you have no character
            if((Input.GetKeyDown(Key.E) && !selectingforplayer2) || (Input.GetKeyDown(Key.O) && selectingforplayer2)){ return (0); };
            
            // get character ID
            for (var i = 0; i < 10; i++)
            {
                int keynumber = 48 + i;
                if (Input.GetKeyDown(keynumber))
                {
                    characterID = i;
                }
                if (Input.GetKeyDown(Key.MINUS))
                {
                    characterID += 10;
                }
            }

       // }

        return (characterID);
    }
}
