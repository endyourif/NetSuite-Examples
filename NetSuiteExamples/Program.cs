using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace NetSuiteExamples
{
    class Program
    {
        static void Main(string[] args)
        {
            var parameters = new NetsuiteSignatureParamaters
            {
                ConsumerKey = string.Empty,
                ConsumerSecret = string.Empty,
                DeploymentId = string.Empty,
                HttpMethod = string.Empty,
                NetsuiteId = string.Empty,
                NetsuiteUrl = string.Empty,
                ScriptId = string.Empty,
                TokenKey = string.Empty,
                TokenSecret = string.Empty
            };

            var generator = new NetSuiteSignatureGenerator();

            var signature = generator.Generate(parameters);

            Console.WriteLine(signature);

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", signature);

                var result = client.GetAsync(string.Empty).Result;

                if (!result.IsSuccessStatusCode)
                {
                    var content = result.Content.ReadAsStringAsync().Result;

                    throw new Exception(content);
                }
            }

            Console.ReadKey();
        }
    }
}
