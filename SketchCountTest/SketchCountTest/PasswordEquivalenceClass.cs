
using System;
namespace SketchCountTest
{
    public class PasswordEquivalenceClass : IComparable<PasswordEquivalenceClass>
    {
        public int NumberOfUsersSharingASpecificPassword { get; set; }
        public int PasswordOccurrence { get; set; }
        public string Strength { get; set; }
        public double EstimatedProbability { get; set; }
        public double CheckingPriority { get; set; }
        public double HashCost { get; set; }

        int IComparable<PasswordEquivalenceClass>.CompareTo(PasswordEquivalenceClass other)
        {
            return other.CheckingPriority.CompareTo(this.CheckingPriority);
            throw new NotImplementedException();
        }

    }
}
