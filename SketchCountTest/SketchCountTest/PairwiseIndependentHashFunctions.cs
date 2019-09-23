using System;
using System.Security.Cryptography;

namespace SketchCountTest
{
    public class PairwiseIndependentHashFunctions
    {
        public static RNGCryptoServiceProvider cryptoRand = new RNGCryptoServiceProvider();

        public PairwiseIndependentHashFunctions(string password)
        {
        }

        public static uint GetHashValue_CountMin(string password)
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

            uint hc = (uint)password.GetHashCode();

            return (a_int * hc) + b_int;
        }

        public static int GetHashValue_CountMedian(string password, int hashFunctionIndex)
        {

            var hc = (hashFunctionIndex + password).GetHashCode();
            byte[] hv;

            byte[] bytes = BitConverter.GetBytes(hc);

            using (SHA256 mySHA256 = SHA256.Create())
            {

                hv = mySHA256.ComputeHash(bytes);

            }
            var hv1 = BitConverter.ToInt32(hv, 0);

            var hv2 = Math.Abs(hv1 % 2);

            int hv3 = 0;

            if (hv2 == 0)
                hv3 = -1;
            if (hv2 == 1)
                hv3 =1;

            return hv3;

        }

    }
}
