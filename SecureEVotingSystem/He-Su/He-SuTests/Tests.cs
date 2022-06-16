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

        [TestCase(4, 2, 2, 5)]
        [TestCase(2, 3, 3, 5)]
        [TestCase(250, 250, 91, 403)]
        [TestCase(251, 251, 91, 403)]
        [TestCase(236, 50, 91, 403)]
        [TestCase(25, 25, 91, 403)]
        [TestCase(30, 30, 91, 403)]
        [TestCase(90, 90, 91, 403)]
        [TestCase(91, 91, 91, 403)]
        public void IsExpModWork(int result, int b, int e, int m) {
            Assert.AreEqual(result, Crypting.ExpMod(b, e, m));
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

            var cryptingText1 = encryptor.Crypt(testText);
            var cryptingText2 = decryptor.Crypt(cryptingText1);

            Assert.AreEqual(testText, cryptingText2);

            cryptingText1 = decryptor.Crypt(testText);
            cryptingText2 = encryptor.Crypt(cryptingText1);

            Assert.AreEqual(testText, cryptingText2);

            int testNumber = 250;
            var cryptingNumber1 = encryptor.Crypt(testNumber);
            var cryptingNumber2 = decryptor.Crypt(cryptingNumber1);

            Assert.AreEqual(testNumber, cryptingNumber2);

            cryptingNumber1 = decryptor.Crypt(testNumber);
            cryptingNumber2 = encryptor.Crypt(cryptingNumber1);

            Assert.AreEqual(testNumber, cryptingNumber2);

        }

        [Test]
        public void SomeTest() {
            string q = "qweqweqweqw";
            int hash = q.GetHashCode();
            Assert.IsTrue(true);
        }

    }
}
