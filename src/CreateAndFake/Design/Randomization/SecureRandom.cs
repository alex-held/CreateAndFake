﻿using System.Security.Cryptography;

namespace CreateAndFake.Design.Randomization
{
    /// <summary>For generating secure but slow random values.</summary>
    public sealed class SecureRandom : ValueRandom
    {
        /// <summary>Source generator used for random bytes.</summary>
        private static readonly RandomNumberGenerator s_Gen = RandomNumberGenerator.Create();

        /// <summary>Sets up the randomizer.</summary>
        /// <param name="onlyValidValues">Option to prevent generating invalid values.</param>
        public SecureRandom(bool onlyValidValues = true) : base(onlyValidValues) { }

        /// <summary>Generates a byte array filled with random bytes.</summary>
        /// <param name="length">Length of the array to generate.</param>
        /// <returns>The generated byte array.</returns>
        protected override byte[] NextBytes(short length)
        {
            byte[] buffer = new byte[length];
            s_Gen.GetBytes(buffer);
            return buffer;
        }
    }
}
