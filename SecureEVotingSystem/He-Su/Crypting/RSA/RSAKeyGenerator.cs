using System;
using System.Collections.Generic;
using System.Text;

namespace SecureEVotingSystem {
    static class RSAKeyGenerator {
        public static void Generate(int p, int q,
            out RSACryptor encryptor, out RSACryptor decryptor) {
            int fn = (p - 1) * (q - 1);
            int e = 0, d = 0;
            for (int i = fn / 4; i <= fn; i++) {
                if (Crypting.TryGetInverse(i, fn, out int inv)) {
                    e = i;
                    d = inv;
                    break;
                }
            }
            int n = p * q;
            encryptor = new RSACryptor(new RSAKey(e, n));
            decryptor = new RSACryptor(new RSAKey(d, n));
        }
    }
}
