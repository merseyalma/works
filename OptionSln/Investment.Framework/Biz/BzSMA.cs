using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Investment.Framework.Biz
{
    public class BzSMA
    {

        private List<float> m_values = new List<float>();
        private int m_n = 1;
        private int m_m = 1;

        private int m_day = 0;

        private float m_startSmaValue = 0.0f;

        private float[] resultSma;
        public BzSMA(List<float> values, float startSmaValue, int n, int m)
        {
            m_values = values;
            m_n = n;
            m_m = m;

            m_startSmaValue =  startSmaValue;
            resultSma = new float[values.Count];
            m_day = values.Count;
        }

        public float[] CalSMA()
        {
            CalSMA(m_day);

              return resultSma;

        }

        private float CalSMA(int day)
        {
            if (day == 1)
            {
                resultSma[0] = m_startSmaValue;

                return m_startSmaValue;
            }
            else
            {
                float result = (m_values[day - 1] * m_m + CalSMA(day - 1) * (m_n - m_m)) / m_n;

                resultSma[day - 1] = result;

                return result;
            }
        }
    }
}
