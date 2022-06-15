﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SecureEVotingSystem {
    class Validator {
        public RSACryptor Encryptor { get; }
        private RSACryptor decryptor { get; }

        private Dictionary<string, string> votersPasswords =
            new Dictionary<string, string> {
                {"1", "1" },
                {"2", "2" },
                {"3", "3" },
                {"4", "4" },
                {"5", "5" },
                {"6", "6" },
                {"7", "7" },
                {"8", "8" },
                {"9", "9" },
                {"10", "10" },
            };

        private List<string> autorizedVoters = new List<string>();

        public Validator() {
            int p = 31;
            int q = 17;
            RSAKeyGenerator.Generate(p, q, 
                out RSACryptor _encryptor, out RSACryptor _decryptor);
            Encryptor = _encryptor;
            decryptor = _decryptor;
        }

        public bool BlindSigningKey(string password, string numberOfUser, int blindKey,
            out int blindedSignedKey) {
            if (!(votersPasswords.ContainsKey(numberOfUser) &&
                votersPasswords[numberOfUser] == password)) {
                blindedSignedKey = 0;
                return false;
            }
            blindedSignedKey = decryptor.Crypt(blindKey);
            return true;
        }


    }
}
