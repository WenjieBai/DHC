using System;
namespace SketchCountTest
{
    public class Password : IEquatable<Password>
    {
        public int Frequency { get; set; }
        public int UpdatedFrequency { get; set; }
        public int Index { get; set; }
        public string Name { get; set; }
        public double ActualProbability { get; set; }
        public string Strength { get; set; }
        public Password()
        {

        }
        public bool Equals(Password other)
        {

            //Check whether the compared object is null. 
            if (Object.ReferenceEquals(other, null)) return false;

            //Check whether the compared object references the same data. 
            if (Object.ReferenceEquals(this, other)) return true;

            //Check whether the products' properties are equal. 
            return Name.Equals(other.Name);
        }
    }
}
