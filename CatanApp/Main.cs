using System;
using System.Collections.Generic;
using System.Drawing;

namespace CatanApp
{
    public class MainClass
    {
        public static void Main()
        {
            Catan catan_test = new Catan(0);
            for (int i = 0; i < 19; i++)
            {
                Console.WriteLine(catan_test.Board.Tiles[i].Resource);
                Console.WriteLine(catan_test.Board.Tiles[i].Chit);
            }
        }
    }
}
