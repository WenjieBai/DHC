using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Security.Cryptography;

namespace SketchCountTest
{

    public class CountSketch
    {

        public static RNGCryptoServiceProvider cryptoRand = new RNGCryptoServiceProvider();

        private double[,] sketch;
        private double[,] laplaceNoise;
        private readonly uint _width;
        private uint _depth;
        private int streamSize;

        private List<uint> a_coe;
        private List<uint> b_coe;


        public CountSketch(uint depth, uint width)
        {

            _width = width;
            _depth = depth;
            sketch = new double[_depth, _width];
            a_coe = new List<uint>();
            b_coe = new List<uint>();

            //hash functions
            for (int i = 0; i < _depth; i++)
            {
                byte[] a = new byte[4];
                cryptoRand.GetBytes(a);

                byte[] b = new byte[4];
                cryptoRand.GetBytes(b);

                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(a);
                    Array.Reverse(b);
                }

                uint a_int = BitConverter.ToUInt32(a, 0);
                uint b_int = BitConverter.ToUInt32(b, 0);

                a_coe.Add(a_int);
                b_coe.Add(b_int);

            }

            // Laplace noise
            //for (int i = 0; i < _depth; i++)
            //{
            //    Console.WriteLine("Laplace noise added to sketch row "+i);
            //    for (int j = 0; j < _width; j++)
            //    {
            //        sketch[i, j] = LaplaceNoise(eps, _depth);
            //    }
            //}

            //Console.WriteLine("sketch created.");

        }

        public static double LaplaceNoise(double epsilon, double k, double sensitivity = 1.0)
        {
            if (double.IsPositiveInfinity(epsilon)) return 0.0;
            double b = sensitivity * k * 1.0 / epsilon;
            //byte[] temp = new byte[4];
            //cryptoRand.GetBytes(temp);
            //int seed = BitConverter.ToInt32(temp, 0);
            Random r = new Random();
            double u = r.NextDouble() - 0.5;
            double X = -b * Math.Log(1.0 - 2.0 * Math.Abs(u));
            if (u < 0.0) X = -X;
            return X;
        }

        private List<uint> HashPassword(string password)
        {
            List<uint> hashPositions = new List<uint>();
            for (int i = 0; i < _depth; i++)
            {
                uint hc = (uint)password.GetHashCode();
                var hashPosition = (a_coe[i] * hc + b_coe[i]) % _width;
                hashPositions.Add(hashPosition);
            }
            return hashPositions;
        }


        public void UpdateSketch_InputAnItem_CountMin(string password, int count)
        {
            var hashPostitions = HashPassword(password);
            for (int i = 0; i < _depth; i++)
            {
                sketch[i, hashPostitions[i]] += count;
            }
            streamSize += count;
        }


        public void UpdateSketch_InputAnList_CountMin(List<string> pwdList, List<int> count)
        {

            for (int i = 0; i < pwdList.Count; i++)
            {
                UpdateSketch_InputAnItem_CountMin(pwdList[i], count[i]);
            }
            Console.WriteLine("sketch has been trained");
        }

     

        public double EstimateMin(string passwordName)
        {
            var hashPostitions = HashPassword(passwordName);

            List<double> itemCounts = new List<double>();
            for (int i = 0; i < _depth; i++)
            {
                itemCounts.Add(sketch[i, hashPostitions[i]]);
            }
            return itemCounts.Min();
        }


        /// <summary>
        /// Count median
        /// </summary>
        /// <param name="password"></param>

        public void UpdateSketch_InputAnItem_CountMedian(string password, int count)
        {
            var hashPostitions = HashPassword(password);
            for (int i = 0; i < _depth; i++)
            {
                sketch[i, hashPostitions[i]] += PairwiseIndependentHashFunctions.GetHashValue_CountMedian(password, i) * count;
            }
            streamSize += count;
        }


        public void UpdateSketch_InputAnList_CountMedian(List<string> pwdList, List<int> countList)
        {
            for (int i = 0; i < pwdList.Count; i++)
            {
                UpdateSketch_InputAnItem_CountMedian(pwdList[i], countList[i]);
            }
        }


        public double EstimateMedian(string passwordName)
        {
            var hashPostitions = HashPassword(passwordName);

            List<double> itemCounts = new List<double>();
            for (int i = 0; i < _depth; i++)
            {
                itemCounts.Add(sketch[i, hashPostitions[i]]);
            }

            itemCounts.Sort();
            int halfCount = itemCounts.Count() / 2;

            if (halfCount % 2 == 0)
            {
                return Math.Abs((itemCounts[halfCount - 1] + itemCounts[halfCount]) / 2);
            }
            else
                return Math.Abs(itemCounts[halfCount]);

        }

    }
}
