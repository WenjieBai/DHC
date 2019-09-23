using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace SketchCountTest
{

    class Program
    {
        static void Main(string[] args)
        {

            //string path = @"/Users/wenjiebai/Projects/SketchCountTest/test.txt";
            //    using (StreamWriter sw = new StreamWriter(path))
            //    {
            //        for (int i = 1; i <= 1000; i++)
            //        {
            //            for (int j = 0; j < i; j++)
            //            {
            //                sw.WriteLine("password" + i);
            //            }
            //        }
            //    }


            //var ck1 = new CountSketch(10, 2000);
            //ck1.UpdateSketch_InputAnFile_CountMin(path);

            //var ck2 = new CountSketch(10, 2000);
            //ck2.UpdateSketch_InputAnFile_CountMedian(path);
            //Console.WriteLine("password999");
            //Console.WriteLine("count min sketch " + ck1.EstimateMin("password999"));
            //Console.WriteLine("count median sketch " + ck2.EstimateMedian("password999"));
            //Console.WriteLine("password99");
            //Console.WriteLine("count min sketch " + ck1.EstimateMin("password99"));
            //Console.WriteLine("count median sketch " + ck2.EstimateMedian("password99"));
            //Console.WriteLine("password9");
            //Console.WriteLine("count min sketch " + ck1.EstimateMin("password1"));
            //Console.WriteLine("count median sketch " + ck2.EstimateMedian("password1"));

            //Console.WriteLine("min "+ck.EstimateMin("password1"));
            //Console.WriteLine("median " + ck.EstimateMedian("password1"));
            //    int a = 3/2;
            //    Console.WriteLine(a);


            EstimateProbability.Estimate();


            //Random rnd = new Random();
            //int numberOfAccounts = 100;
            //List<int> randomNumbers = Enumerable.Range(0, numberOfAccounts).OrderBy(x => rnd.Next()).Take(numberOfAccounts / 2).ToList();
            //List<int> randomNumbers1 = Enumerable.Range(0, numberOfAccounts).Except(randomNumbers).ToList();

            //Console.WriteLine(randomNumbers.Count+" "+randomNumbers1.Count);
            //foreach (var item in randomNumbers)
            //{
            //    Console.WriteLine("r " + item);
            //}

            //foreach (var item in randomNumbers1)
            //{
            //    Console.WriteLine("r1 " + item);
            //}



        }



    }
}
