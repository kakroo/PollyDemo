using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace PollyDemo
{
    public class DummyCaller
    {
        static Random random = new Random();
        List<System.Net.HttpStatusCode> resCodes;
        public DummyCaller()
        {
            // Adding more negatives to increase the probability of regative response.
            resCodes = new List<System.Net.HttpStatusCode>()
            {  HttpStatusCode.Accepted ,
               System.Net.HttpStatusCode.Ambiguous ,
               System.Net.HttpStatusCode.BadGateway,
               System.Net.HttpStatusCode.BadRequest,
               System.Net.HttpStatusCode.Forbidden,
               System.Net.HttpStatusCode.InternalServerError,
                System.Net.HttpStatusCode.BadGateway,
                System.Net.HttpStatusCode.Forbidden,
               System.Net.HttpStatusCode.BadRequest,
               System.Net.HttpStatusCode.Forbidden,
               System.Net.HttpStatusCode.InternalServerError,
               System.Net.HttpStatusCode.InternalServerError,
               System.Net.HttpStatusCode.InternalServerError,
               System.Net.HttpStatusCode.OK,
               System.Net.HttpStatusCode.OK,
               System.Net.HttpStatusCode.OK,

            };
        }
        public Task<HttpResponseMessage> PostAsync(Uri requestUri)
        {
            HttpResponseMessage response = RandomDataGenerator();
            return Task.Run(() => response);
        }

        private HttpResponseMessage RandomDataGenerator()
        {
            int randomNumber = random.Next(0, resCodes.Count);
            Thread.Sleep(randomNumber * 100);
            HttpResponseMessage responseMessage = new HttpResponseMessage() { StatusCode = resCodes[randomNumber] };
            Console.WriteLine($"Simulated Response Code : {responseMessage.StatusCode}");
            return responseMessage;


        }
    }
}
