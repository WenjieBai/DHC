using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SketchCountTest
{
    public class GenerateFakePasswords
    {

        public int numberOfAccounts = 0;
        public int numberOfDistinctPasswords = 0;
        public int numberOfEquivalenceClasses = 0;
        public List<Password> fakePwdsList;
        //public List<PasswordEquivalenceClass> pwdEC_List;
        private string _path;

        public GenerateFakePasswords(string path)
        {
            _path = path;
            fakePwdsList = new List<Password>();
            //pwdEC_List = new List<PasswordEquivalenceClass>();
        }

        public void Build()
        {
            string line;
            string[] parts;
            char delimiter = ' ';

            int numOfUsers_Sharing;
            int occurrence;
            using (StreamReader sr = new StreamReader(_path))
            {
                while (sr.EndOfStream == false)
                {

                    line = sr.ReadLine();
                    line = line.Trim();
                    parts = line.Split(delimiter);

                    numOfUsers_Sharing = Int32.Parse(parts[0]);
                    occurrence = Int32.Parse(parts[1]);

            
                    numberOfAccounts += numOfUsers_Sharing * occurrence;
                    numberOfDistinctPasswords += occurrence;
                    numberOfEquivalenceClasses++;

                    var fakePasswords_SameFrequency = EquivalenceClass_To_FakePasswords(numOfUsers_Sharing, occurrence);
                    fakePwdsList.AddRange(fakePasswords_SameFrequency);
                }
            }

            //FakePasswords_To_EquivalenceClass(fakePwdsList);
            Console.WriteLine("number of accounts " + numberOfAccounts);
            Console.WriteLine("number of distinct passwords " + numberOfDistinctPasswords);
            Console.WriteLine("file has been read");
            Console.WriteLine("number of fake passwords " + fakePwdsList.Count);
            Console.WriteLine("fake passwords have been generated");

        }

        private List<Password> EquivalenceClass_To_FakePasswords(int _numOfUsers_Sharing, int _passwordOccurence)
        {
            List<Password> fakePasswords_SameFrequence = new List<Password>();
            for (int i = 0; i < _passwordOccurence; i++)
            {
                var fakePassword = new Password()
                {
                    Frequency = _numOfUsers_Sharing,
                    Index = i + 1,
                    Name = "number of sharing users " + _numOfUsers_Sharing + "index: " + (i + 1)
                };

                fakePasswords_SameFrequence.AddRange(Enumerable.Repeat(fakePassword, fakePassword.Frequency));
            }

            return fakePasswords_SameFrequence;
        }



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



        void UpdateFrequcncy(List<Password> toBeUpdated)
        {

            var q = from x in toBeUpdated
                    group x by x.Name;
            
            Console.WriteLine("sampled_pwd_dist " + q.Count());

            foreach (var x in q)
            {
                foreach (var y in x)
                {
                    y.UpdatedFrequency = x.Count();
                    y.ActualProbability = (double) y.UpdatedFrequency / toBeUpdated.Count();
                    //Console.WriteLine("y "+y.ActualProbability);
                }
            }
        }

     

        public (List<Password>, List<Password>) Sample()
        {
            this.Build();
            Random rnd = new Random();

            List<Password> sampledPasswords = new List<Password>();
            List<Password> unsampledPasswords = new List<Password>();

            List<int> randomNumbers = Enumerable.Range(0, numberOfAccounts).OrderBy(x => rnd.Next()).Take(numberOfAccounts / 2).ToList();
            List<int> randomNumbers_un = Enumerable.Range(0, numberOfAccounts).Except(randomNumbers).ToList();

            for (int i = 0; i < randomNumbers.Count; i++)
            {

                sampledPasswords.Add(fakePwdsList[randomNumbers[i]]);
            }
            Console.WriteLine("number of sampled passwords " + sampledPasswords.Count);
            UpdateFrequcncy(sampledPasswords);

            for (int i = 0; i < randomNumbers_un.Count; i++)
            {
                unsampledPasswords.Add(fakePwdsList[randomNumbers_un[i]]);
            }

            
            Console.WriteLine("number of unsampled passwords " + unsampledPasswords.Count);

            //using (StreamWriter sample = new StreamWriter(@"/Users/wenjiebai/Projects/SketchCountTest/SampledPwd.txt"))
            //{
            //    foreach (var item in sampledPwd_)
            //    {
            //        sample.WriteLine(item.Name + " " + item.Frequency);
            //    }
            //}

            Console.WriteLine("sampling has completed");
            return (sampledPasswords, unsampledPasswords);

        }
    }
}
