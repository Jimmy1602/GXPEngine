using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GXPEngine;
using GXPEngine.Core;

public class cardreader
{
    public int readcard()
    {
        int characterID = -1;
        if (Input.GetKeyDown(Key.EQUALS))
        {

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
            Console.WriteLine(characterID.ToString());

        }

        return (characterID);
    }
}
