using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;


namespace SecureEVotingSystem {
    [TestFixture]
    class Tests {
        private string testText = "Testing message 123";
        private ShiftCryptor testingShiftCryptor = new ShiftCryptor(20);


        [Test]
        public void IsShiftEncryptWorking() {
            var cipherText = testingShiftCryptor.Encrypt(testText);
            var encrypted = testingShiftCryptor.Decrypt(cipherText);
            Assert.AreEqual(testText, encrypted);
        }

        [Test]
        public void IsExpModWork() {
            Assert.AreEqual(4, Crypting.ExpMod(2, 2, 5));
            Assert.AreEqual(2, Crypting.ExpMod(3, 3, 5));
        }

        [TestCase(11, 21, 2, true)]
        [TestCase(12, 21, 0, false)]
        public void TestTryGetInverse(int a, int n, int invResult, bool tryingResult) {
            bool result = Crypting.TryGetInverse(a, n, out int inv);
            Assert.AreEqual(tryingResult, result);
            Assert.AreEqual(invResult, inv);
        }


        [TestCase(31, 13)]
        [TestCase(17, 71)]
        [TestCase(17, 19)]
        public void TestRSA(int p, int q) {
            RSAKeyGenerator.Generate(p, q, 
                out RSACryptor encryptor, out RSACryptor decryptor);
            
            Assert.AreEqual(1,
                encryptor.Key.Exponent * decryptor.Key.Exponent % ((p - 1) * (q - 1)));
            
            Assert.AreEqual(testText, decryptor.Crypt(encryptor.Crypt(testText)));
            Assert.AreEqual(testText, encryptor.Crypt(decryptor.Crypt(testText)));

            int testNumber = 250;
            Assert.AreEqual(testNumber, decryptor.Crypt(encryptor.Crypt(testNumber)));
            Assert.AreEqual(testNumber, encryptor.Crypt(decryptor.Crypt(testNumber)));

        }

    }
}
