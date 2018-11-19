using System;
using NBitcoin;
using QBitNinja.Client;
using QBitNinja.Client.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ColeCoin.Exercises
{
    public class Keys
    {
        public void AddEntropy()
        {
            RandomUtils.AddEntropy("hello");
            RandomUtils.AddEntropy(new byte[] { 1, 2, 3 });
            var nsaProofKey = new Key();
            Console.WriteLine("nsaProofKey: " + nsaProofKey);
        }
        
        public void KeyDerivationFunction()
        {
            var privateKey = new Key();

            var bitcoinPrivateKey = privateKey.GetWif(Network.Main);
            Console.WriteLine("Private Key: " + bitcoinPrivateKey); // L1tZPQt7HHj5V49YtYAMSbAmwN9zRjajgXQt9gGtXhNZbcwbZk2r

            BitcoinEncryptedSecret encryptedBitcoinPrivateKey = bitcoinPrivateKey.Encrypt("password");
            Console.WriteLine("Encrypted Private Key: " + encryptedBitcoinPrivateKey); // 6PYKYQQgx947Be41aHGypBhK6TA5Xhi9TdPBkatV3fHbbKrdDoBoXFCyLK

            var decryptedBitcoinPrivateKey = encryptedBitcoinPrivateKey.GetSecret("password");
            Console.WriteLine("Decrypted Private Key: " + decryptedBitcoinPrivateKey); // L1tZPQt7HHj5V49YtYAMSbAmwN9zRjajgXQt9gGtXhNZbcwbZk2r

            Console.ReadLine();
        }
    }
}