using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WordGenerator.Engine
{
    class CharCountCollection : IEnumerable<CharCount>
    {
        Dictionary<char, CharCount> m_hash = new Dictionary<char, CharCount>();
        int m_totalCount = 0;

        public void Add(char c, int increment)
        {
            if (!m_hash.ContainsKey(c))
                m_hash.Add(c, new CharCount(c));

            m_hash[c].Count += increment;
            m_totalCount += increment;
        }

        public IEnumerator<CharCount> GetEnumerator()
        {
            return m_hash.Values.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return m_hash.Values.GetEnumerator();
        }

        public int TotalCount
        {
            get { return m_totalCount; }
        }
    }
}
