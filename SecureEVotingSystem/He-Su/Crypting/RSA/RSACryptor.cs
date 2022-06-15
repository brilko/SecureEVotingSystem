using System;
using System.Collections.Generic;
using System.Text;

namespace SecureEVotingSystem {
    class RSACryptor {
        public RSAKey Key { get; }

        public RSACryptor(RSAKey key) {
            Key = key;
        }

        public string Crypt(string text) {
            return Crypting.RSACiphering(text, Key);
        }

        public int Crypt(int number) {
            return Crypting.RSACiphering(number, Key);
        }
    }
}
