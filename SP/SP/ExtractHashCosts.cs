using System;
using System.Collections.Generic;
using System.IO;

namespace SP
{
    public class ExtractHashCosts
    {
        public Dictionary<double, double> hashCost;

        public ExtractHashCosts(string path)
        {
            hashCost = new Dictionary<double, double>();
            string line;
            string[] parts;
            char[] delimiters = { ',', '(', ')' };

            using (StreamReader sr = new StreamReader(path))
            {
                while (sr.EndOfStream == false)
                {

                    line = sr.ReadLine();
                    line = line.Trim();
                    parts = line.Split(delimiters);
                    hashCost.Add(Double.Parse(parts[1]), Double.Parse(parts[2]));
                }
            }

        }
    }
}

