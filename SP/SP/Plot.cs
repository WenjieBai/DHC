using System;
using System.Collections.Generic;
using System.Linq;

namespace SP
{
    public class Plot
    {
        public Plot()
        {
        }

        public static void PlotRockYou()
        {
            string path = @"/Users/wenjiebai/Projects/SP/RockYou_CountMin/OutputPEC.txt";

            string k1 = @"/Users/wenjiebai/Projects/SP/RockYou_CountMin/RockYouHashCostK_1.txt";
            string k2 = @"/Users/wenjiebai/Projects/SP/RockYou_CountMin/RockYouHashCostK_2.txt";
            string k3 = @"/Users/wenjiebai/Projects/SP/RockYou_CountMin/RockYouHashCostK_3.txt";

            var hc_k1 = new ExtractHashCosts(k1);
            var hc_k2 = new ExtractHashCosts(k2);
            var hc_k3 = new ExtractHashCosts(k3);

            PasswordRepository pr = new PasswordRepository(path);
            pr.buildRepositoryOnPartitionedPasswords();

            var epm = new EqualProbabilityMass(pr, 3);
            epm.Partition();

          
            //var bs = new BestResponseGiven_k(epm, passwordValue);
            //var rate = bs.Alg4(new List<double> {hc_k1.hashCost[passwordValue],hc_k2.hashCost[passwordValue],hc_k3.hashCost[passwordValue]});
            //Console.WriteLine(rate);

            int numberOfPointsInEachInterval = 10;
            int numberOfIntervals = 9;
            int[] passwordValue = new int[numberOfIntervals * numberOfPointsInEachInterval];

            for (int i = 0; i < numberOfIntervals - 2; i++)
            {
                for (int j = 0; j < numberOfPointsInEachInterval; j++)
                {
                    passwordValue[numberOfPointsInEachInterval * i + j] = (int)Math.Pow(10, (i + 2)) * (j + 1) / numberOfPointsInEachInterval;

                }
            }
            List<int> distinct = passwordValue.Distinct().ToList();
            distinct.RemoveAt(distinct.Count - 1);

            double[] adversarySuccessRate = new double[distinct.Count];

            using (System.IO.StreamWriter advSuccess = new System.IO.StreamWriter(@"/Users/wenjiebai/Projects/SP/RockYouAdvSuccess_Actual.txt"))
            {
                for (int i = 0; i < distinct.Count; i++)
                {
                    var bs = new BestResponseGiven_k(epm, distinct[i]);

                    adversarySuccessRate[i] = bs.Alg4(new List<double> { hc_k1.hashCost[distinct[i]], hc_k2.hashCost[distinct[i]], hc_k3.hashCost[distinct[i]]});
                    advSuccess.WriteLine("(" + distinct[i] + "," + adversarySuccessRate[i] + ")");
                    Console.WriteLine("(" + distinct[i] + "," + adversarySuccessRate[i] + ")");
                }

            }
        }
    }
}
