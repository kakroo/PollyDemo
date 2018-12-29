using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace PollyDemo
{
    public class MyHttpClient:HttpClient
    {
        public MyHttpClient()
        {
           
        }

        //Wrapper
        public async Task<HttpResponseMessage> GetAsyncPolly()
        {   
            try
            {
                HttpResponseMessage response = new HttpResponseMessage();
                var getPolicy = PolicyManagement.GetPolicyForGetCalls();
                await getPolicy.ExecuteAsync(async () =>
                {
                    response = await SomeWork();
                    response.EnsureSuccessStatusCode();
                });

                return response;



            }
            catch (Exception ex)
            {
                Console.WriteLine("Request Failed :" + ex.Message);
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }

        }

        //Wrapper
        public async Task<HttpResponseMessage> PostAsyncPolly()
        {   
            try
            {
                HttpResponseMessage response = new HttpResponseMessage();
                var getPolicy = PolicyManagement.GetPolicyForPostCalls();
                await getPolicy.ExecuteAsync(async () =>
                {
                    Console.WriteLine("Inside ExecuteAsync for Post..");
                    response = await SomeWork();
                    return response;
                });

                return response;



            }
            catch (Exception ex)
            {
                Console.WriteLine("Request Failed :" + ex.Message);
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }

        }

        public async Task<HttpResponseMessage> SomeWork()
        {
            DummyCaller caller = new DummyCaller();
            HttpResponseMessage msg = await caller.PostAsync(new Uri("http://www.google.com"));
            Console.WriteLine("Doing Some Work..");
            Console.WriteLine("");
            return msg;

        }
       
    }
}
