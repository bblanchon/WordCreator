using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace WordCreator.Engine
{
    class Generator
    {
        MarkovHash m_markov = new MarkovHash();
        LengthCountCollection m_lengths = new LengthCountCollection();

        private static char EOF = (char)0;
        
        private void Learn(string word, int order, int increment)
        {
            for (int i = 1; i < order; i++)
            {
                var substring = word.Substring(0, i);
                m_markov.Add(substring, word[i], increment);
            }

            for (int i = order; i < word.Length; i++)
            {
                var substring = word.Substring(i - order, order);
                m_markov.Add(substring, word[i], increment); ;
            }

            if (word.Length >= order)
            {
                var substring = word.Substring(word.Length - order, order);
                m_markov.Add(substring, EOF, increment); ;
            }
        }

        int orderMax = 2;

        private void Learn(string word, int increment)
        {
            m_lengths.Add(word.Length, increment);
            Learn(word, Math.Min(orderMax, word.Length), increment);
        }

        public void Learn(string word)
        {
            Learn(word, +1);
        }

        public void Learn(IEnumerable<string> list)
        {
            foreach (var word in list) Learn(word);
        }

        public void Unlearn(string word)
        {
            Learn(word, -1);
        }

        public void Unlearn(IEnumerable<string> list)
        {
            foreach (var word in list) Unlearn(word);
        }

        public void Clear()
        {
            m_lengths.Clear();
            m_markov.Clear();
        }

        private IEnumerable<CharProbability> SuggestNextChar(string word)
        {
            string tail;

            if (word.Length > orderMax)
                tail = word.Substring(word.Length - orderMax, orderMax);
            else
                tail = word;

            if (m_markov.ContainsKey(tail) && m_markov[tail].TotalCount > 0 )
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

        public IEnumerable<StringProbability> Suggest(string word, double minProbability = 0.05)
        {
            return Suggest(word.ToLower(), 1, minProbability).OrderByDescending(x => x.Probability);
        }
    }
}
