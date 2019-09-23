using System;
using System.Collections.Generic;
using System.Linq;

namespace SP
{
    public class EqualProbabilityMass : IPartitionPasswords
    {

        public List<PasswordEquivalenceClass> weakPasswords;
        public List<PasswordEquivalenceClass> mediumPasswords;
        public List<PasswordEquivalenceClass> strongPasswords;
        public double probabilityMass_weak;
        public double probabilityMass_medium;
        public double probabilityMass_strong;

        public PasswordRepository PR;
        double sumOfProbability;
        int _numberOfPartitions;

        public EqualProbabilityMass(PasswordRepository pr, int numberOfPaititions)
        {
            PR = pr;
            _numberOfPartitions = numberOfPaititions;
            weakPasswords = new List<PasswordEquivalenceClass>();
            mediumPasswords = new List<PasswordEquivalenceClass>();
            strongPasswords = new List<PasswordEquivalenceClass>();
        }

        public void PartitionMethord()
        {
            strongPasswords.Add(PR.PwcList[PR.PwcList.Count - 1]);
            sumOfProbability = PR.PwcList[PR.PwcList.Count - 1].EstimatedProbability * PR.PwcList[PR.PwcList.Count - 1].PasswordOccurrence;
            var probMass_Strong = sumOfProbability;

            for (int i = PR.PwcList.Count - 2; i >= 0; i--)
            {
                sumOfProbability += PR.PwcList[i].EstimatedProbability * PR.PwcList[i].PasswordOccurrence;

                if (sumOfProbability <= 1.0 / _numberOfPartitions)
                {
                    strongPasswords.Add(PR.PwcList[i]);
                }
                else if (sumOfProbability - probMass_Strong  <= (1-probMass_Strong)/2)
                {
                    mediumPasswords.Add(PR.PwcList[i]);

                } 
                else
                {
                    weakPasswords.Add(PR.PwcList[i]);
                }

            }
            probabilityMass_weak = weakPasswords.Select(x => x.EstimatedProbability * x.PasswordOccurrence).Sum();
            probabilityMass_medium = mediumPasswords.Select(x => x.EstimatedProbability * x.PasswordOccurrence).Sum();
            probabilityMass_strong = strongPasswords.Select(x => x.EstimatedProbability * x.PasswordOccurrence).Sum();
        }

        public void Partition()
        {
            foreach (var pwd in PR.PwcList)
            {
                if (pwd.Strength == "weak")
                {
                    weakPasswords.Add(pwd);
                }
                if (pwd.Strength == "medium")
                {
                    mediumPasswords.Add(pwd);
                }
                if (pwd.Strength == "strong")
                {
                    strongPasswords.Add(pwd);
                }
            }
            probabilityMass_weak = weakPasswords.Select(x => x.EstimatedProbability * x.PasswordOccurrence).Sum();
            probabilityMass_medium = mediumPasswords.Select(x => x.EstimatedProbability * x.PasswordOccurrence).Sum();
            probabilityMass_strong = strongPasswords.Select(x => x.EstimatedProbability * x.PasswordOccurrence).Sum();

            Console.WriteLine(probabilityMass_weak);
            Console.WriteLine(probabilityMass_medium);
            Console.WriteLine(probabilityMass_strong);
        }

    }
}
