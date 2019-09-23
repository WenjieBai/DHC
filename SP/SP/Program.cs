using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SP
{
    class Program
    {
        static void Main(string[] args)
        {
            //string path = @"/Users/wenjiebai/Projects/SP/rockyouFreq.txt";

            //PasswordRepository pr = new PasswordRepository(path);
            //pr.buildRepository();
            //var epm = new EqualProbabilityMass(pr, 3);
            //epm.PartitionMethord();

            //var bs = new BestResponseGiven_k(epm, 30000);
            //var r = bs.Alg5();

            //Console.WriteLine(r.Item2);



            //string path = @"/Users/wenjiebai/Projects/SP/PasswordClass.txt";

            //PasswordRepository pr = new PasswordRepository(path);
            //pr.buildRepositoryOnActualPasswords();

            //var epm = new EqualProbabilityMass(pr, 3);
            //epm.PartitionMethordBasedOnActualPasswords();

            //var bs = new BestResponseGiven_k(epm, 30000000);
            //var r = bs.Alg5();

            //Console.WriteLine(r);


            //Plot.PlotRockYou();

            //Console.WriteLine(epm.probabilityMass_weak);
            //Console.WriteLine(epm.probabilityMass_medium);
            //Console.WriteLine(epm.probabilityMass_strong);
            //Console.WriteLine(epm.probabilityMass_medium + epm.probabilityMass_strong + epm.probabilityMass_weak);



            //GetHashCost.GetHashCost_RockYou();
            Plot.PlotRockYou();

        }
    }
}

