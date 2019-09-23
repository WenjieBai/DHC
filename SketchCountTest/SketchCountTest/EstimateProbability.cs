using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SketchCountTest
{
    public class EstimateProbability
    {
        public EstimateProbability()
        {
        }
        public static void Estimate()
        {
            string path = @"/Users/wenjiebai/Projects/SketchCountTest/yahoo-all.txt";
            var fpList = new GenerateFakePasswords(path);

            (var sampled, var unsampled) = fpList.Sample();

            var sampled_dist = sampled.Distinct().ToList();
            var unsampled_dist = unsampled.Distinct().ToList();

            double threshold_1;
            double threshold_2;
            (threshold_1, threshold_2) = Partition(sampled_dist);

            CountSketch cs = new CountSketch(10, (uint)(fpList.numberOfDistinctPasswords));
            cs.UpdateSketch_InputAnList_CountMin(sampled_dist.Select(x => x.Name).ToList(), sampled_dist.Select(x => x.UpdatedFrequency).ToList());

            var dictionary = ClassifySampled(sampled_dist, threshold_1, threshold_2);

            var pwdList_dist = fpList.fakePwdsList.Distinct().ToList();


            foreach (var pwd in pwdList_dist)
            {
                //if (dictionary.ContainsKey(pwd.Name))
                //    pwd.Strength = dictionary[pwd.Name];
                //else
                //{
                double ep = cs.EstimateMin(pwd.Name) / sampled.Count;

                if (ep <= threshold_1)
                {
                    pwd.Strength = "strong";
                }
                else if (ep <= threshold_2)
                {
                    pwd.Strength = "medium";
                }
                else
                {
                    pwd.Strength = "weak";
                }
                //}
            }

            

            var pwdClasses = from pwd in pwdList_dist
                             group pwd by new { numberOfSharingUsers = pwd.Frequency, strength = pwd.Strength } into g
                             orderby g.Key.numberOfSharingUsers
                             select g;

            ;
            using (StreamWriter sr = new StreamWriter(@"/Users/wenjiebai/Projects/SketchCountTest/OutputPEC.txt"))
            {
                foreach (var pwdClass in pwdClasses)
                {
                    sr.WriteLine(pwdClass.Key.numberOfSharingUsers + " " + pwdClass.Key.strength + " " + pwdClass.Count());
                    //Console.WriteLine(pwdClass.Key.numberOfSharingUsers);
                }
            }

        }


        static (double, double) Partition(List<Password> pwdList)
        {
            var pwd_ordered = pwdList.OrderByDescending(x => x.ActualProbability).ToList();


            double threshold1 = 0;
            double threshold2 = 0;
            double probsum = 0;
            for (int i = 0; i < pwd_ordered.Count; i++)
            {
                probsum += pwd_ordered[i].ActualProbability;
                if (probsum < 1.0 / 3.0)
                {
                    threshold2 = pwd_ordered[i].ActualProbability;
                }
                else if (probsum < 2.0 / 3.0)
                {
                    threshold1 = pwd_ordered[i].ActualProbability;
                }

            }
            Console.WriteLine("sum " + probsum);
            Console.WriteLine("t1 " + threshold1 + " t2 " + threshold2);
            return (threshold1, threshold2);
        }




        static Dictionary<string, string> ClassifySampled(List<Password> sampled_dist, double threshold1, double threshold2)
        {

            Dictionary<string, string> dic = new Dictionary<string, string>();
            foreach (var sampledPwd in sampled_dist)
            {

                if (sampledPwd.ActualProbability <= threshold1)
                {
                    sampledPwd.Strength = "strong";
                }
                else if (sampledPwd.ActualProbability <= threshold2)
                {
                    sampledPwd.Strength = "medium";
                }
                else
                {
                    sampledPwd.Strength = "weak";
                }
                dic.Add(sampledPwd.Name, sampledPwd.Strength);
            }

            var weak = from x in sampled_dist
                       where x.Strength == "weak"
                       select x;
            Console.WriteLine("weak " + weak.Select(x => x.ActualProbability).Sum());

            var medium = from x in sampled_dist
                         where x.Strength == "medium"
                         select x;
            Console.WriteLine("medium" + medium.Select(x => x.ActualProbability).Sum());

            var strong = from x in sampled_dist
                         where x.Strength == "strong"
                         select x;
            Console.WriteLine("strong " + strong.Select(x => x.ActualProbability).Sum());

            var q = from x in sampled_dist
                    group x by new { frequency = x.UpdatedFrequency, strength = x.Strength } into g
                    orderby g.Key.frequency
                    select g;



            int sum = 0;
            using (StreamWriter sr = new StreamWriter(@"/Users/wenjiebai/Projects/SketchCountTest/OutputPEC_trained.txt"))
            {
                foreach (var x in q)
                {
                    sr.WriteLine(x.Key.frequency + " " + x.Key.strength + " " + x.Count());
                    sum += x.Key.frequency * x.Count();
                }
            }
            Console.WriteLine("sanity check " + sum);

            return dic;
        }


        //private Dictionary<string, string> ClassifySampled_EC(List<Password> sampled_dist, double threshold1, double threshold2)
        //{

        //    Dictionary<string, string> dic = new Dictionary<string, string>();

        //    var q = from x in sampled_dist
        //            group x by new { prob = x.ActualProbability} into g
        //            orderby g.Key.frequency
        //            select g;

        //    foreach (var sampledPwd in sampled_dist)
        //    {

        //        if (sampledPwd.ActualProbability <= threshold1)
        //        {
        //            sampledPwd.Strength = "strong";
        //        }
        //        else if (sampledPwd.ActualProbability <= threshold2)
        //        {
        //            sampledPwd.Strength = "medium";
        //        }
        //        else
        //        {
        //            sampledPwd.Strength = "weak";
        //        }
        //        dic.Add(sampledPwd.Name, sampledPwd.Strength);
        //    }


        //var q = from x in sampled_dist
        //        group x by new { frequency = x.UpdatedFrequency, strength = x.Strength } into g
        //        orderby g.Key.frequency
        //        select g;



        //int sum = 0;
        //using (StreamWriter sr = new StreamWriter(@"/Users/wenjiebai/Projects/SketchCountTest/OutputPEC_trained.txt"))
        //{
        //    foreach (var x in q)
        //    {
        //        sr.WriteLine(x.Key.frequency + " " + x.Key.strength + " " + x.Count());
        //        sum += x.Key.frequency * x.Count();
        //    }
        //}
        //Console.WriteLine("sanity check " + sum);

        //    return dic;
        //}


        private List<PasswordEquivalenceClass> FakePasswords_To_EquivalenceClass(List<Password> pwd_user)
        {
            var pwd_dist = pwd_user.Distinct().ToList();

            var q = from x in pwd_dist
                    group x by x.UpdatedFrequency into g
                    orderby g.Key descending
                    select g;

            List<PasswordEquivalenceClass> sampled_pwdEC_List = new List<PasswordEquivalenceClass>();

            foreach (var item in q)
            {
                Console.WriteLine(item.Key + " " + item.Count());
                PasswordEquivalenceClass pwdEC = new PasswordEquivalenceClass()
                {
                    NumberOfUsersSharingASpecificPassword = item.Key,
                    PasswordOccurrence = item.Count()
                };
                sampled_pwdEC_List.Add(pwdEC);
            }
            return sampled_pwdEC_List;
        }


    }
}
