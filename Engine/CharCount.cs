using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WordGenerator.Engine
{
    class CharCount
    {
        public CharCount(char c)
        {
            this.Char = c;
            this.Count = 0;
        }

        public readonly char Char;
        public int Count;

        public override string ToString()
        {
            return string.Format("'{0}' x {1}", Char, Count);
        }
    }
}
