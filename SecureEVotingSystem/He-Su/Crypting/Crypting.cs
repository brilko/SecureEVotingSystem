using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace SecureEVotingSystem {
    static class Crypting {
        public static string CryptingText(string text, Func<int, int> crypt) {
            var charArray = text.ToCharArray();
            var codes = charArray.Select(c => (int)c);
            var cryptCodes = codes.Select(n => crypt(n));
            var cryptChars = cryptCodes.Select(n => (char)n);
            var arrayCriptCodes = cryptChars.ToArray();
            var cryptText = new string(arrayCriptCodes);
            return cryptText;
        }

        public static int ExpMod(int b, int e, int m) {
            int a = 1;
            for (int i = 0; i < e; i++) {
                a = (a * b) % m;
            }
            return a;
        }

        public static string RSACiphering(string text, RSAKey key) {
            return CryptingText(text, (n) => ExpMod(n, key.Exponent, key.Modulus));
        }

        public static int RSACiphering(int number, RSAKey key) {
            return ExpMod(number, key.Exponent, key.Modulus);
        }

        //Ключи подобраны так, чтобы модули были больше 258
        private static int maxHash = 250; 

        public static int GetHash(RSAKey key) {
            return Math.Abs(key.GetHashCode()) % maxHash;
        }

        public static int GetHash(string text) {
            return Math.Abs(text.GetHashCode()) % maxHash;
        }

        public static bool TryGetInverse(int a, int n, out int inv) {
            int t = 0;
            int r = n;
            int nt = 1;
            int nr = a;
            int quotient, tmp;
            while (nr != 0) {
                quotient = (int)Math.Floor((double)(r / nr));
                tmp = t - quotient * nt;
                t = nt;
                nt = tmp;
                tmp = r - quotient * nr;
                r = nr;
                nr = tmp;
            }
            if (r > 1) {
                inv = 0;
                return false;
            }
            if (t < 0) {
                t += n;
            }
            inv = t;
            return true;
        }
    }
}
