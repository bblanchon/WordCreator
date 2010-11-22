using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace WordGenerator.Engine
{
    class Generator
    {
        MarkovHash m_markov = new MarkovHash();
        LengthCountCollection m_lengths = new LengthCountCollection();

        private static char EOF = (char)0;
        
        private void Learn(string word, int order)
        {
            for (int i = 1; i < order; i++)
            {
                var substring = word.Substring(0, i);
                m_markov.Add(substring, word[i]);
            }

            for (int i = order; i < word.Length; i++)
            {
                var substring = word.Substring(i - order, order);
                m_markov.Add(substring, word[i]);
            }

            if (word.Length >= order)
            {
                var substring = word.Substring(word.Length - order, order);
                m_markov.Add(substring, EOF);
            }
        }

        int orderMax = 2;

        private void Learn(string word)
        {
            m_lengths.Add(word.Length);
            Learn(word, Math.Min(orderMax, word.Length));
        }

        public void Learn(WordList list)
        {
            foreach (var word in list.Words) Learn(word);
        }

        private IEnumerable<CharProbability> SuggestNextChar(string word)
        {
            string tail;

            if (word.Length > orderMax)
                tail = word.Substring(word.Length - orderMax, orderMax);
            else
                tail = word;

            if (m_markov.ContainsKey(tail))
            {
                var coll = m_markov[tail];
                return coll.OrderByDescending(x => x.Count).Select(x => new CharProbability(x.Char,(double)x.Count/coll.TotalCount));
            }
            else
            {
                return Enumerable.Empty<CharProbability>();
            }
        }

        private IEnumerable<StringProbability> Suggest(string word, double probability, double minProba)
        {
            if (probability < minProba) yield break;

            foreach (var cp in SuggestNextChar(word))
            {
                if (cp.Char == EOF)
                {
                    var lengthProba = m_lengths.GetProbaOfAtLeast(word.Length);

                    if (lengthProba * cp.Probability >= minProba)
                        yield return new StringProbability (word, probability);
                }
                else
                {
                    var lengthProba = m_lengths.GetProbaOfAtLeast(word.Length+1);

                    foreach (var sp in Suggest(word + cp.Char, lengthProba * cp.Probability, minProba))
                        yield return sp;
                }
            }
        }

        public IEnumerable<string> Suggest(string word, double minProbability=0.05)
        {
            return Suggest(word.ToLower(), 1, minProbability).OrderByDescending(x=>x.Probability).Select(x => x.String);
        }
    }
}
