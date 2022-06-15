using System;
using System.Collections.Generic;
using System.Text;

namespace SecureEVotingSystem {
    class Voter {
        private RSACryptor encrypter { get; }
        private RSACryptor decrypter { get; }
        private int R { get; }
        private int G { get; }
        private ShiftCryptor shiftCrypter { get; }
        public Voter() {
            int p = 19;
            int q = 23;
            RSAKeyGenerator.Generate(p, q,
                out RSACryptor _encrypter, out RSACryptor _decrypter);
            encrypter = _encrypter;
            decrypter = _decrypter;
            int n = decrypter.Key.Modulus;
            for (int i = n / 2; i >= 0; i--) {
                if (Crypting.TryGetInverse(i, n, out int inv)) {
                    R = i;
                    G = inv;
                    break;
                }
            }
            int someSiftKey = 23; //Случайное число 
            shiftCrypter = new ShiftCryptor(someSiftKey);
        }

        public int GetSignedKey() {
            return decrypter.Crypt(R) * Crypting.GetHash(encrypter.Key);
        }

        public bool DeblindAndCheckValidatorSign(RSACryptor validatorEncrypter,
            int blindSignedHashKey, out RSACryptor encryptor, out int signedKey) {
            signedKey = validatorEncrypter.Crypt(
                    G * blindSignedHashKey % validatorEncrypter.Key.Modulus);
            encryptor = encrypter;
            return Crypting.GetHash(encrypter.Key) == signedKey;
        }

        public Tuple<RSACryptor, string, int> PrepareBallot(string ballot) {
            string cipheredBallot = shiftCrypter.Encrypt(ballot);
            int signedcipheredBallot = decrypter.Crypt(Crypting.GetHash(cipheredBallot));
            return new Tuple<RSACryptor, string, int>(
                encrypter, cipheredBallot, signedcipheredBallot);
        }

        public Tuple<RSACryptor, ShiftCryptor, int> GetDecrypt() {
            int signedShiftKey = decrypter.Crypt(shiftCrypter.Key);
            return new Tuple<RSACryptor, ShiftCryptor, int>(
                encrypter, shiftCrypter, signedShiftKey);
        }
    }
}
