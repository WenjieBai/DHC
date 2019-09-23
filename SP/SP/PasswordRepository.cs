using System;
using System.Collections.Generic;
using System.IO;

namespace SP
{
    public class PasswordRepository
    {
        public int numberOfAccounts = 0;
        public int numberOfDistinctPasswords = 0;
        public int numberOfEquivalenceClasses = 0;
        public List<PasswordEquivalenceClass> PwcList;
        private string _path;



        public PasswordRepository(string path)
        {
            _path = path;
            PwcList = new List<PasswordEquivalenceClass>();

        }

        public void buildRepository()
        {
            string line;
            string[] parts;
            char delimiter = ' ';
            using (StreamReader sr = new StreamReader(_path))
            {
                while (sr.EndOfStream == false)
                {

                    line = sr.ReadLine();
                    line = line.Trim();
                    parts = line.Split(delimiter);
                    var pe = new PasswordEquivalenceClass
                    {
                        NumberOfUsersSharingASpecificPassword = Int32.Parse(parts[0]),
                        PasswordOccurrence = Int32.Parse(parts[1])
                    };

                    numberOfAccounts += pe.NumberOfUsersSharingASpecificPassword * pe.PasswordOccurrence;
                    numberOfDistinctPasswords += pe.PasswordOccurrence;
                    numberOfEquivalenceClasses++;
                    PwcList.Add(pe);
                }

            }

            foreach (var pwc in PwcList)
            {
                pwc.EstimatedProbability = (double)pwc.NumberOfUsersSharingASpecificPassword / numberOfAccounts;
            }
            Console.WriteLine(numberOfAccounts);
            Console.WriteLine(numberOfDistinctPasswords);
            Console.WriteLine(numberOfEquivalenceClasses);
        }

        public void buildRepositoryOnPartitionedPasswords()
        {
            char[] delimiters = { ' ' };
            string line;
            string[] parts1;

            using (System.IO.StreamReader file = new System.IO.StreamReader(_path))
            {
                while (file.EndOfStream == false)
                {

                    line = file.ReadLine();
                    line = line.Trim();
                    parts1 = line.Split(delimiters);

                    var pe = new PasswordEquivalenceClass
                    {
                        NumberOfUsersSharingASpecificPassword = Int32.Parse(parts1[0]),
                        Strength = parts1[1],
                        PasswordOccurrence = Int32.Parse(parts1[2])
                    };

                    numberOfAccounts += pe.NumberOfUsersSharingASpecificPassword * pe.PasswordOccurrence;
                    numberOfDistinctPasswords += pe.PasswordOccurrence;
                    numberOfEquivalenceClasses++;
                    PwcList.Add(pe);

                }
            }

            foreach (var pwc in PwcList)
            {
                pwc.EstimatedProbability = (double)pwc.NumberOfUsersSharingASpecificPassword / numberOfAccounts;
            }

            Console.WriteLine(numberOfAccounts);
            Console.WriteLine(numberOfDistinctPasswords);
            Console.WriteLine(numberOfEquivalenceClasses);
        }

        

    }
}
