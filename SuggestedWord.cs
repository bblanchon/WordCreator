using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WordCreator
{
    class SuggestedWord
    {
        public SuggestedWord(string word, double probability, bool isInSources, bool isInUserWords)
        {
            Word = word;
            Probability = probability;
            IsInSources = isInSources;
            IsInUserWords = isInUserWords;
        }

        public string Word { private set; get; }
        public double Probability { private set; get; }
        public double ProbabilityPercent { get { return Probability * 100; } }
        public bool IsInSources { private set; get; }
        public bool IsInUserWords { private set; get; }
    }
}
