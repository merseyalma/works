using Investment.Framework.Biz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CalTool
{
    class Program
    {
        static void Main(string[] args)
        {


//            List<float> closeList = new List<float>() {
//            34.03f,
//            37.43f,
//41.17f,
//45.29f,
//49.82f,
//54.8f,
//58.01f,
//59.51f,
//57.98f,
//63.05f,
//63.93f,
//62.85f,
//60.3f, 
//58.58f,
//56.88f ,58.84f,56.940f,57.570f,57.580f,57.880f,57.530f,54.450f,49.940f,50.290f,48.600f,49.060f,
//50.080f
//            };
            List<float> closeList = new List<float>()
            {
             9.230f,   10.15f,11.170f,12.290f,13.520f,14.870f,14.000f,
             14.980f,14.710f,

            };
            closeList = new List<float>()
            {
                38.58f,42.44f,46.68f
            };

            closeList = new List<float>()
            {
             25.42f,  27.96f,30.76f,33.84f
            };

            List<float> sma_values1List = new List<float>();

            List<float> sma_values2List = new List<float>();

            for (int i = 1; i < closeList.Count; i++)
            {
                sma_values1List.Add(Math.Max(closeList[i] - closeList[i - 1], 0));

                sma_values2List.Add(Math.Abs(closeList[i] - closeList[i - 1]));
            }

            BzSMA sma1 = new BzSMA(sma_values1List, sma_values1List[0]/6, 6, 1);

            BzSMA sma2 = new BzSMA(sma_values2List, sma_values2List[0]/6, 6, 1);

            float[] sma1Val = sma1.CalSMA();
            float[] sma2Val = sma2.CalSMA();

            for (int i = 0; i < sma1Val.Length; i++)
            {
                Console.WriteLine(sma1Val[i].ToString());
            }

            Console.WriteLine("");
            for (int i = 0; i < sma2Val.Length; i++)
            {
                Console.WriteLine(sma2Val[i].ToString());
            }

            //  Console.WriteLine((sma1Val * 1f, / sma2Val).ToString());
            Console.ReadKey();
        }



    }
}
