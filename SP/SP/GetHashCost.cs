using System;
using System.Collections.Generic;
using System.Linq;

namespace SP
{
    public class GetHashCost
    {
        public GetHashCost()
        {

        }
        public static void GetHashCost_RockYou()
        {
            string path = @"/Users/wenjiebai/Projects/SP/RockYou_CountMin/OutputPEC_trained.txt";

            PasswordRepository pr = new PasswordRepository(path);
            pr.buildRepositoryOnPartitionedPasswords();

            var epm = new EqualProbabilityMass(pr, 3);
            epm.Partition();

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
            List<double> hashCost = new List<double>();

            System.IO.StreamWriter k_1 = new System.IO.StreamWriter(@"/Users/wenjiebai/Projects/SP/RockYou_CountMin/RockYouHashCostK_1.txt");
            System.IO.StreamWriter k_2 = new System.IO.StreamWriter(@"/Users/wenjiebai/Projects/SP/RockYou_CountMin/RockYouHashCostK_2.txt");
            System.IO.StreamWriter k_3 = new System.IO.StreamWriter(@"/Users/wenjiebai/Projects/SP/RockYou_CountMin/RockYouHashCostK_3.txt");

            for (int i = 0; i < distinct.Count; i++)
            {

                var bs = new BestResponseGiven_k(epm, distinct[i]);
                (hashCost, adversarySuccessRate[i]) = bs.Alg5();
                Console.WriteLine("adv success" +adversarySuccessRate[i]);

                Console.WriteLine("Point No. "+i);
                foreach (var item in hashCost)
                {
                    Console.WriteLine(item);
                }
                k_1.WriteLine("(" + distinct[i] + "," + hashCost[0] + ")");
                k_2.WriteLine("(" + distinct[i] + "," + hashCost[1] + ")");
                k_3.WriteLine("(" + distinct[i] + "," + hashCost[2] + ")");
            }

            k_1.Close();
            k_2.Close();
            k_3.Close();
        }
    }
}
