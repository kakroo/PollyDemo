using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using Polly;
using Polly.Retry;

namespace PollyDemo
{
    /// <summary>
    /// Policy management via Polly.
    /// </summary>
    public static class PolicyManagement
    {

        // IMPLEMENTATION
        //For Get : using retry 5 times with retry time of 2 seconds + jitter of 0-2 seconds
        //For Put : retrying 3 times with retry time of 1 seconds for httpStatusCodesWorthRetrying (check this variable), anything other than these codes, it will not retry.

        static int retryCountForGet = 5;
        static int retryCountForPost = 3;
        static int retryTimeForGet = 2;
        static int retryTimeForPost = 1;
        static int maxTimeForJitter = 2000;


        /// <summary>
        /// Gets the policy for get calls.
        /// </summary>
        /// <returns>The policy for get calls.</returns>
        public static Policy GetPolicyForGetCalls()
        {
            try
            {
                Random jitter = new Random();
                return Policy.Handle<HttpRequestException>()
                                .WaitAndRetryAsync(retryCountForGet,
                                sleepDurationProvider: (retryAttempt) => TimeSpan.FromSeconds(retryTimeForGet) + TimeSpan.FromMilliseconds(jitter.Next(0, maxTimeForJitter)),
                                //sleepDurationProvider: (retryAttempt) => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)) + TimeSpan.FromMilliseconds(jitter.Next(0, 100)), //for exponential backoff
                                onRetry: (exception, calculatedWaitDuration) =>
                                {
                                    Console.WriteLine("Retrying Get Call....");
                                    Console.WriteLine($"Wait Duration {calculatedWaitDuration}");
                                    //Console.WriteLine($"Exception : {exception.Message}");
                                    Console.WriteLine("");
                                    Console.WriteLine("");

                                }
                                );

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return Policy.NoOp();
            }

        }
        /// <summary>
        /// Gets the policy for post calls.
        /// </summary>
        /// <returns>The policy for post calls.</returns>
        public static RetryPolicy<HttpResponseMessage> GetPolicyForPostCalls()
        {


            return Policy.HandleResult<HttpResponseMessage>(r => FetchCodesWorthRetrying().Contains(r.StatusCode))
                        .WaitAndRetryAsync(
                           retryCount: retryCountForPost,
                           sleepDurationProvider: (retryAttempt) => TimeSpan.FromSeconds(retryTimeForPost),
                           onRetry: (exception, calculatedWaitDuration) =>
                           {
                               Console.WriteLine("Retrying Post Call....");
                               Console.WriteLine($"Wait Duration {calculatedWaitDuration}");
                               Console.WriteLine("");
                               Console.WriteLine("");
                           }
                           );
        }
       
        /// <summary>
        /// Fetchs the codes worth retrying.
        /// </summary>
        /// <returns>The codes worth retrying.</returns>
        public static HttpStatusCode[] FetchCodesWorthRetrying()
        {
            //TODO: Check if we need to add/remove some codes from here
            HttpStatusCode[] httpStatusCodesWorthRetrying = 
                                    {
                                       HttpStatusCode.RequestTimeout, // 408
                                       HttpStatusCode.InternalServerError, // 500
                                       HttpStatusCode.BadGateway, // 502
                                       HttpStatusCode.ServiceUnavailable, // 503
                                       HttpStatusCode.GatewayTimeout, // 504
                                    };
            return httpStatusCodesWorthRetrying;
        }
    }
}
