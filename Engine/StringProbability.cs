using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WordGenerator.Engine
{
    class StringProbability
    {
        public StringProbability(string s, double p)
        {
            this.String = s;
            this.Probability = p;
        }

        public string String;
        public double Probability;
    }  
}
