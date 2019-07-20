using System;
using System.Security.Cryptography;
using System.Text;

namespace NetSuiteExamples
{
    class NetSuiteSignatureGenerator
    {
        public string Generate(NetsuiteSignatureParamaters paramaters)
        {
            var newTimestamp = GenerateTimestamp();
            var newNonce = GenerateNonce();
            return GenerateWithNonceAndTimestamp(paramaters, newTimestamp, newNonce);
        }

        public string GenerateWithNonceAndTimestamp(NetsuiteSignatureParamaters paramaters, int timestamp, string nonce)
        {
            return CreateAuth(paramaters, GenerateSignature(paramaters, timestamp, nonce), timestamp, nonce);
        }

        private string CreateAuth(NetsuiteSignatureParamaters paramaters, string signature, int timestamp, string nonce)
        {
            return string.Format(
                    "OAuth realm=\"{0}\",oauth_consumer_key=\"{1}\",oauth_token=\"{2}\",oauth_signature_method=\"{3}\",oauth_timestamp=\"{4}\",oauth_nonce=\"{5}\",oauth_version=\"1.0\",oauth_signature=\"{6}\"",
                    paramaters.NetsuiteId,
                    paramaters.ConsumerKey,
                    paramaters.TokenKey,
                    paramaters.SignatureMethod,
                    timestamp,
                    nonce,
                    signature
                );
        }

        private string GenerateSignature(NetsuiteSignatureParamaters paramaters, int timestamp, string nonce)
        {
            var rightSide = string.Format(
                "deploy={0}&oauth_consumer_key={1}&oauth_nonce={2}&oauth_signature_method={3}&oauth_timestamp={4}&oauth_token={5}&oauth_version={6}&script={7}",
                paramaters.DeploymentId,
                paramaters.ConsumerKey,
                nonce,
                paramaters.SignatureMethod,
                timestamp,
                paramaters.TokenKey,
                "1.0",
                paramaters.ScriptId
            );

            var baseString = string.Format(
                "{0}&{1}&{2}",
                paramaters.HttpMethod,
                Uri.EscapeDataString(paramaters.NetsuiteUrl.ToLower()),
                Uri.EscapeDataString(rightSide)
            );

            var signature = Generate(paramaters.ConsumerSecret, paramaters.TokenSecret, baseString);

            return Uri.EscapeDataString(signature);
        }

        private string Generate(string consumerSecret, string tokenSecret, string baseString)
        {
            var key = string.Format(
                "{0}&{1}",
                Uri.EscapeDataString(consumerSecret),
                Uri.EscapeDataString(tokenSecret)
            );

            var signature = CreateSignature(baseString, key);

            return signature;
        }

        private string CreateSignature(string data, string key)
        {
            // Initialize the keyed hash object using the secret key as the key
            HMACSHA1 hashObject = new HMACSHA1(Encoding.UTF8.GetBytes(key));

            // Computes the signature by hashing the data with the secret key as the key
            byte[] signature = hashObject.ComputeHash(Encoding.UTF8.GetBytes(data));

            // Base 64 Encode
            return Convert.ToBase64String(signature);
        }

        public int GenerateTimestamp()
        {
            return ((int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds);
        }

        public string GenerateNonce()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }
    }
}
