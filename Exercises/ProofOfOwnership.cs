using System;
using NBitcoin;
using QBitNinja.Client;
using QBitNinja.Client.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ColeCoin.Exercises
{
    public class ProofOfOwnership
    {
        public void Proof()
        {
            var message = "I am Craig Wright";
            var signature = "IN5v9+3HGW1q71OqQ1boSZTm0/DCiMpI8E4JB1nD67TCbIVMRk/e3KrTT9GvOuu3NGN0w8R2lWOV2cxnBp+Of8c=";

            var address = new BitcoinPubKeyAddress("1A1zP1eP5QGefi2DMPTfTL5SLmv7DivfNa");
            
            bool isCraigWrightSatoshi = address.VerifyMessage(message, signature);
            Console.WriteLine("Is Craig Wright Satoshi? " + isCraigWrightSatoshi);
        }
    }
}