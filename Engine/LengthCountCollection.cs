using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WordGenerator.Engine
{
    class LengthCountCollection
    {
        Dictionary<int, int> m_cummulatedLengths = new Dictionary<int, int>();
        int m_totalCount;

        private void CoreAdd(int length, int increment)
        {
            if (!m_cummulatedLengths.ContainsKey(length))
                m_cummulatedLengths.Add(length, increment);
            else
                m_cummulatedLengths[length] = m_cummulatedLengths[length] + increment;
        }

        public void Add(int length, int increment)
        {
            for (int i = 0; i <= length; i++) CoreAdd(i, increment);
            m_totalCount += increment;
        }

        public double GetProbaOfAtLeast(int length)
        {
            if (m_cummulatedLengths.ContainsKey(length))
                return (double)m_cummulatedLengths[length] / m_totalCount;
            else
                return 0;
        }

        public void Clear()
        {
            m_cummulatedLengths.Clear();
            m_totalCount = 0;
        }
    }
}
