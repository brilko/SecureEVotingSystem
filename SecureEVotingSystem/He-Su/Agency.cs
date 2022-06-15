using System;
using System.Collections.Generic;
using System.Text;

namespace SecureEVotingSystem {
    class Agency {
        private List<RSAKey> listOfAutorizedKeys = new List<RSAKey>();
        private Dictionary<RSACryptor, Tuple<string, int>> listCipheredBallots =
            new Dictionary<RSACryptor, Tuple<string, int>>();
        private List<Tuple<RSACryptor, ShiftCryptor, int>> listOfDecrypts =
            new List<Tuple<RSACryptor, ShiftCryptor, int>>();

        public bool CheckValidatorSignKeyVoter(RSACryptor validatorEncryptor, 
            int signedHashKey, RSAKey voterOpenKey) {
            if (voterOpenKey.GetHashCode() == validatorEncryptor.Crypt(signedHashKey)) {
                listOfAutorizedKeys.Add(voterOpenKey);
                return true;
            }
            return false;
        }

        public bool CheckSignedCipheredBallot(
            RSACryptor voterEncryptor, string cipheredBallot, 
                int signedHashCipheredBallot) {
            if (voterEncryptor.Crypt(signedHashCipheredBallot) == 
                Crypting.GetHash(cipheredBallot)) {
                if (listCipheredBallots.ContainsKey(voterEncryptor)) {
                    listCipheredBallots[voterEncryptor] = new Tuple<string, int> (
                        cipheredBallot, signedHashCipheredBallot);
                } 
                return true;
            }
            return false;
        }

        public List<int> GetResult(
                List<Tuple<RSACryptor, ShiftCryptor, int>> listOfDecrypts) {
            this.listOfDecrypts = listOfDecrypts;
            var results = new List<int> { 0, 0 };
            foreach (var decrypt in listOfDecrypts) {
                string cryptedBallot = listCipheredBallots[decrypt.Item1].Item1;
                string ballot = decrypt.Item2.Decrypt(cryptedBallot);
                int voice = int.Parse(ballot);
                results[voice - 1]++;
            }
            return results;
        }

    }
}
