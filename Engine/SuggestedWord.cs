﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WordGenerator.Engine
{
    class SuggestedWord
    {
        public SuggestedWord(string word, double probability, bool isInSources)
        {
            Word = word;
            Probability = probability;
            IsInSources = isInSources;
        }

        public string Word { private set; get; }
        public double Probability { private set; get; }
        public double ProbabilityPercent { get { return Probability * 100; } }
        public bool IsInSources { private set; get; }
    }
}
