using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WordCreator.Engine
{
    class CharProbability
    {
        public CharProbability(char c, double p)
        {
            this.Char = c;
            this.Probability = p;
        }

        public char Char;
        public double Probability;

        public override string ToString()
        {
            return string.Format("'{0}' x {1:0.##}%", Char == 0 ? "\\0" : Char.ToString(), Probability * 100);
        }
    }

}
