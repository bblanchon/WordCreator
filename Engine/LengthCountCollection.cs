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

        private void CoreAdd(int length)
        {
            if (!m_cummulatedLengths.ContainsKey(length))
                m_cummulatedLengths.Add(length, 1);
            else
                m_cummulatedLengths[length] = m_cummulatedLengths[length] + 1;
        }

        public void Add(int length)
        {
            for (int i = 0; i <= length; i++) CoreAdd(i);
            m_totalCount++;
        }

        public double GetProbaOfAtLeast(int length)
        {
            if (m_cummulatedLengths.ContainsKey(length))
                return (double)m_cummulatedLengths[length] / m_totalCount;
            else
                return 0;
        }
    }
}
