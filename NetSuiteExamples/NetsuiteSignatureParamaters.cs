namespace NetSuiteExamples
{
    class NetsuiteSignatureParamaters
    {
        public string NetsuiteUrl { get; set; }
        public string NetsuiteId { get; set; }
        public string ConsumerSecret { get; set; }
        public string ConsumerKey { get; set; }
        public string TokenSecret { get; set; }
        public string TokenKey { get; set; }
        public string HttpMethod { get; set; }
        public string DeploymentId { get; set; }
        public string ScriptId { get; set; }

        public string SignatureMethod
        {
            get
            {
                return "HMAC-SHA1";
            }
        }
    }
}
