using System;
using ColeCoin.Practice;
using ColeCoin.Exercises;

namespace ColeCoin
{
    class Program
    {
        static void Main(string[] args)
        {
            Introduction intro = new Introduction();
            SpendBitcoin spendBitcoin = new SpendBitcoin();
            ProofOfOwnership proofOfOwnership = new ProofOfOwnership();
            Keys keys = new Keys();

            // intro.Intro();

            // spendBitcoin.Spend();

            // proofOfOwnership.Proof();

            keys.AddEntropy();
            keys.KeyDerivationFunction();
        }
    }
}
