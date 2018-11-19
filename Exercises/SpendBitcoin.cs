using System;
using NBitcoin;
using QBitNinja.Client;
using QBitNinja.Client.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ColeCoin.Exercises
{
    public class SpendBitcoin
    {
        public void Spend()
        {
            var privateKey = new Key();
            var bitcoinPrivateKey = new BitcoinSecret("cTXMvqkS73yQR4ap7jHTtrLWbD17F4iKs6rPZLTTJ2VYHK3nspxh");
            var network = bitcoinPrivateKey.Network;
            Console.WriteLine("Network: " + network);
            var address = bitcoinPrivateKey.GetAddress();

            // Console.WriteLine(bitcoinPrivateKey); // cTXMvqkS73yQR4ap7jHTtrLWbD17F4iKs6rPZLTTJ2VYHK3nspxh
            // Console.WriteLine(address); // mnAVCavS9rbCU1QwJXDwYV49BYw77wjrhE

            var client = new QBitNinjaClient(network);
            var transactionId = uint256.Parse("401d985e0b1aa2367682a3f2055673591a7bd0131d80bc7d2e4f7c7441bc566e");
            var transactionResponse = client.GetTransaction(transactionId).Result;

            /*
                Transaction ID and # of Confirmations
            */
            Console.WriteLine("Transaction ID: " + transactionResponse.TransactionId); // 401d985e0b1aa2367682a3f2055673591a7bd0131d80bc7d2e4f7c7441bc566e
            if (transactionResponse.Block != null)
            {
                Console.WriteLine("Confirmations: " + transactionResponse.Block.Confirmations); // 91
            }

            /*
                From Where?
            */
            var receivedCoins = transactionResponse.ReceivedCoins;
            OutPoint outPointToSpend = null;
            foreach (var coin in receivedCoins)
            {
                if (coin.TxOut.ScriptPubKey == bitcoinPrivateKey.ScriptPubKey)
                {
                    outPointToSpend = coin.Outpoint;
                }
            }
            if(outPointToSpend == null)
                throw new Exception("TxOut doesn't contain our ScriptPubKey");
            Console.WriteLine("We want to spend {0}. outpoint:", outPointToSpend.N + 1);

            /*
                Create Transaction
            */
            var transaction = Transaction.Create(network);
            transaction.Inputs.Add(new TxIn()
            {
                PrevOut = outPointToSpend
            });

            var myAddress = BitcoinAddress.Create("mnAVCavS9rbCU1QwJXDwYV49BYw77wjrhE");

            // How much I want to send
            var myAddressAmount = new Money(0.0004m, MoneyUnit.BTC);

            // Miner Fee
            var minerFee = new Money(0.00007m, MoneyUnit.BTC);

            // How much I want to get back as change
            var txInAmount = (Money)receivedCoins[(int) outPointToSpend.N].Amount;
            var changeAmount = txInAmount - myAddressAmount - minerFee;

            TxOut myAddressTxOut = new TxOut()
            {
                Value = myAddressAmount,
                ScriptPubKey = myAddress.ScriptPubKey
            };

            TxOut changeBackTxOut = new TxOut()
            {
                Value = changeAmount,
                ScriptPubKey = bitcoinPrivateKey.ScriptPubKey
            };

            transaction.Outputs.Add(myAddressTxOut);
            transaction.Outputs.Add(changeBackTxOut);

            var message = "Just sending some delicious fake bitcoin...";
            var bytes = Encoding.UTF8.GetBytes(message);
            transaction.Outputs.Add(new TxOut()
            {
                Value = Money.Zero,
                ScriptPubKey = TxNullDataTemplate.Instance.GenerateScriptPubKey(bytes)
            });

            /*
                Sign Transaction
            */
            transaction.Inputs[0].ScriptSig = myAddress.ScriptPubKey;
            transaction.Sign(bitcoinPrivateKey, receivedCoins.ToArray());

            /*
                Propagate Transaction
            */
            BroadcastResponse broadcastResponse = client.Broadcast(transaction).Result;

            if (!broadcastResponse.Success)
            {
                Console.Error.WriteLine("ErrorCode: " + broadcastResponse.Error.ErrorCode);
                Console.Error.WriteLine("Error message: " + broadcastResponse.Error.Reason);
            }
            else
            {
                Console.WriteLine("Success! You can check out the hash of the transaciton in any block explorer:");
                Console.WriteLine(transaction.GetHash()); // https://live.blockcypher.com/btc-testnet/tx/8f0c3347d5db09b18e585c503098b2d51e50c167a47c7bbb5b99b0fa30e7a751/
            }
        }
    }
}