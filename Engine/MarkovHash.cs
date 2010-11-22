using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WordGenerator.Engine
{
    class MarkovHash
    {
        Dictionary<string, CharCountCollection> m_hash = new Dictionary<string, CharCountCollection>();

        public void Add(string substring, char nextChar)
        {
            if (!m_hash.ContainsKey(substring))
                m_hash.Add(substring, new CharCountCollection());

            m_hash[substring].Add(nextChar);
        }

        public bool ContainsKey(string s)
        {
            return m_hash.ContainsKey(s);
        }

        public CharCountCollection this[string s]
        {
            get
            {
                return m_hash[s];
            }
        }

    }
}
