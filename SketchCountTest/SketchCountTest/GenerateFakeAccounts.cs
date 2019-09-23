using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SketchCountTest
{
    public class GenerateFakeAccounts
    {
        public GenerateFakeAccounts()
        {
        }
        public int numberOfAccounts = 0;
        public int numberOfDistinctPasswords = 0;
        public int numberOfEquivalenceClasses = 0;
        public List<Password> fakeAccounts;
        private string _path;

        public GenerateFakeAccounts(string path)
        {
            _path = path;
            fakeAccounts = new List<Password>();
        }

        public void Build()
        {
            string line;
            string[] parts;
            char delimiter = ' ';

            int numOfUsers_Sharing;
            int passwordOccurrence;
            using (StreamReader sr = new StreamReader(_path))
            {
                while (sr.EndOfStream == false)
                {

                    line = sr.ReadLine();
                    line = line.Trim();
                    parts = line.Split(delimiter);

                    numOfUsers_Sharing = Int32.Parse(parts[0]);
                    passwordOccurrence = Int32.Parse(parts[1]);

                    var fakePasswords_SameFrequency = EquivalenceClass_To_FakePasswords(numOfUsers_Sharing, passwordOccurrence);

                    foreach (var fakePassword_SameFrequency in fakePasswords_SameFrequency)
                    {
                        fakeAccounts.Add(fakePassword_SameFrequency);
                    }

                    numberOfAccounts = fakeAccounts.Count;
                    numberOfDistinctPasswords = fakeAccounts.Select(x => x.Name).Distinct().Count();
                }
            }

            //FakePasswords_To_EquivalenceClass(fakePwdsList);
            Console.WriteLine("number of accounts " + numberOfAccounts);
            Console.WriteLine("file has been read");
            Console.WriteLine("number of distinct passwords " + numberOfDistinctPasswords);
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
                    Name = "number of users sharing this password:" + _numOfUsers_Sharing + "index:" + (i + 1)
                };

                fakePasswords_SameFrequence.AddRange(Enumerable.Repeat(fakePassword, fakePassword.Frequency));
            }

            return fakePasswords_SameFrequence;
        }
    }
}
