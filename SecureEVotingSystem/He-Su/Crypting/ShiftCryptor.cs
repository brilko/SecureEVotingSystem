using System;
using System.Collections.Generic;
using System.Text;

namespace SecureEVotingSystem {
    class ShiftCryptor {
        public int Key { get; }

        public ShiftCryptor(int key) {
            Key = key;
        }

        public string Encrypt(string text) {
            return Crypting.CryptingText(text, (n) => n + Key);
        }

        public string Decrypt(string cipherText) {
            return Crypting.CryptingText(cipherText, (n) => n - Key);
        }
    }
}
