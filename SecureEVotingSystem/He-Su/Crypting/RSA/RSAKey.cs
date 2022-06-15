using System;
using System.Collections.Generic;
using System.Text;

namespace SecureEVotingSystem {
    class RSAKey {
        public RSAKey(int exponent, int modulus) {
            Exponent = exponent;
            Modulus = modulus;
        }

        public int Exponent { get; set; }
        public int Modulus { get; set; }
    }
}
