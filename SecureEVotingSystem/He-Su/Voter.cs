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
        public string Number { get; }
        public string Password { get; }
        public Voter(string number, string password, RSACryptor validatorEncryptor) {
            int p = 29;
            int q = 23;
            RSAKeyGenerator.Generate(p, q,
                out RSACryptor _encrypter, out RSACryptor _decrypter);
            encrypter = _encrypter;
            decrypter = _decrypter;
            int n = validatorEncryptor.Key.Modulus;
            for (int i = n / 2; i >= 0; i--) {
                if (Crypting.TryGetInverse(i, n, out int inv)) {
                    R = i;
                    G = inv;
                    break;
                }
            }
            int someSiftKey = 23; //Случайное число 
            shiftCrypter = new ShiftCryptor(someSiftKey);
            Number = number;
            Password = password;
        }

        public int GetSignedKey(RSACryptor validatorEncryptor) {
            return (validatorEncryptor.Crypt(R) * Crypting.GetHash(encrypter.Key)) % validatorEncryptor.Key.Modulus;
        }

        public bool DeblindAndCheckValidatorSign(RSACryptor validatorEncrypter,
            int blindSignedHashKey, out RSACryptor encryptor, out int signedKey) {
            signedKey = G * blindSignedHashKey % validatorEncrypter.Key.Modulus;
            var hashKey = validatorEncrypter.Crypt(signedKey);
            encryptor = encrypter;
            return Crypting.GetHash(encrypter.Key) == hashKey;
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
