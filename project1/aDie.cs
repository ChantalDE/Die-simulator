using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;


namespace project1
{
    /// <summary>
    /// This is the die class. The die will be constructed every time th e
    /// </summary>
    class Die : RandNum
    {
        //constructor for empty seed
        public Die()
        {
            rand = new Random();
        }

        //constructor for seed
        public Die(int seedNum) {
            rand = new Random(seedNum);
        }

        //uses the single static random object, so the same sequence is being used when rolling both dice
        public int Roll()
        {
            //returns random number between (1-6)
            int num = rand.Next(1, 7);
            return num;
        }

        public Image[] diePics = {  Properties.Resources.one, Properties.Resources.two,
                                    Properties.Resources.three, Properties.Resources.four,
                                    Properties.Resources.five, Properties.Resources.six,
                                    Properties.Resources.static_die
                                 };
    }
        
}
