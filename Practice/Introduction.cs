using System;
using NBitcoin;
using QBitNinja.Client;
using QBitNinja.Client.Models;
using System.Collections.Generic;
using System.Linq;

namespace ColeCoin.Practice
{
    public class Introduction
    {
        public void Intro() {
            // Console.WriteLine("Hello World! " + new Key().GetWif(Network.Main));

            Key privateKey = new Key(); // generate a random private key
            PubKey publicKey = privateKey.PubKey; // get public key from private key
            Console.WriteLine("Public Key: " + publicKey); // public key 
            Console.WriteLine("MainNet Address: " + publicKey.GetAddress(Network.Main)); // MainNet address 
            Console.WriteLine("TestNet Address: " + publicKey.GetAddress(Network.TestNet)); // TestNet address 

            var publicKeyHash = publicKey.Hash;
            Console.WriteLine("Public Key Hash: " + publicKeyHash); // public key hash 
            var mainNetAddress = publicKeyHash.GetAddress(Network.Main);
            var testNetAddress = publicKeyHash.GetAddress(Network.TestNet);

            Console.WriteLine("MainNet ScriptPubKey: " + mainNetAddress.ScriptPubKey);
            Console.WriteLine("TestNet ScriptPubKey: " + testNetAddress.ScriptPubKey);

            var paymentScript1 = publicKeyHash.ScriptPubKey;
            var sameMainNetAddress = paymentScript1.GetDestinationAddress(Network.Main);
            bool isSameMainNetAddress = mainNetAddress == sameMainNetAddress;
            Console.WriteLine("isSameMainNetAddress: " + isSameMainNetAddress); // True

            // generate our Bitcoin secret(also known as Wallet Import Format or simply WIF) from our private key for the mainnet
            BitcoinSecret mainNetPrivateKey = privateKey.GetBitcoinSecret(Network.Main);
            // generate our Bitcoin secret(also known as Wallet Import Format or simply WIF) from our private key for the testnet
            BitcoinSecret testNetPrivateKey = privateKey.GetBitcoinSecret(Network.TestNet);
            Console.WriteLine("MainNet Private Key: " + mainNetPrivateKey); 
            Console.WriteLine("TestNet Private Key: " + testNetPrivateKey);

            bool WifIsBitcoinSecret = mainNetPrivateKey == privateKey.GetWif(Network.Main);
            Console.WriteLine("WifIsBitcoinSecret: " + WifIsBitcoinSecret); // True

            // Create a client
            QBitNinjaClient client = new QBitNinjaClient(Network.Main);
            // Parse transaction id to NBitcoin.uint256 so the client can eat it
            var transactionId = uint256.Parse("f13dc48fb035bbf0a6e989a26b3ecb57b84f85e0836e777d6edf60d87a4a2d94");
            // Query the transaction
            GetTransactionResponse transactionResponse = client.GetTransaction(transactionId).Result;
            NBitcoin.Transaction transaction = transactionResponse.Transaction;
            Console.WriteLine("Transaction ID: " + transactionResponse.TransactionId); // f13dc48fb035bbf0a6e989a26b3ecb57b84f85e0836e777d6edf60d87a4a2d94
            Console.WriteLine("Transaction ID: " + transaction.GetHash()); // f13dc48fb035bbf0a6e989a26b3ecb57b84f85e0836e777d6edf60d87a4a2d94
            Console.WriteLine();

            List<ICoin> receivedCoins = transactionResponse.ReceivedCoins;
            foreach (var coin in receivedCoins)
            {
                Console.WriteLine("Coin:");
                Money amount = (Money) coin.Amount;

                Console.WriteLine(amount.ToDecimal(MoneyUnit.BTC));
                var paymentScript = coin.TxOut.ScriptPubKey;
                Console.WriteLine(paymentScript);  // It's the ScriptPubKey
                var address = paymentScript.GetDestinationAddress(Network.Main);
                Console.WriteLine(address); // 1HfbwN6Lvma9eDsv7mdwp529tgiyfNr7jc
                Console.WriteLine();
            }

            var inputs = transaction.Inputs;
            foreach (TxIn input in inputs)
            {
                OutPoint previousOutpoint = input.PrevOut;
                // Console.WriteLine(previousOutpoint.Hash); // hash of prev tx
                // Console.WriteLine(previousOutpoint.N); // idx of out from prev tx, that has been spent in the current tx
                // Console.WriteLine();
            }

            Money twentyOneBtc = new Money(21, MoneyUnit.BTC);
            var scriptPubKey = transaction.Outputs.First().ScriptPubKey;
            TxOut txOut = new TxOut(twentyOneBtc, scriptPubKey);

            OutPoint firstOutPoint = receivedCoins.First().Outpoint;
            // Console.WriteLine(firstOutPoint.Hash); // f13dc48fb035bbf0a6e989a26b3ecb57b84f85e0836e777d6edf60d87a4a2d94
            // Console.WriteLine(firstOutPoint.N); // 0

            Console.WriteLine("Transaction Inputs: " + transaction.Inputs.Count); // 9

            // OutPoint prevOutPoint = transaction.Inputs.First().PrevOut;
            // bool coinbaseTx = false;
            // while (!coinbaseTx)
            // {
            //     var prevTransaction = client.GetTransaction(prevOutPoint.Hash).Result.Transaction;
            //     prevOutPoint = prevTransaction.Inputs.First().PrevOut;
            //     coinbaseTx = prevTransaction.IsCoinBase;
            //     Console.WriteLine("isCoinbaseTx: " + coinbaseTx); // False
            // }
        }
    }
}