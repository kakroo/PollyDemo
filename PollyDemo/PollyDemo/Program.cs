using System;

namespace PollyDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Polly Demo!");
                MyHttpClient client = new MyHttpClient();
                var getResponse = client.GetAsyncPolly();
                //Uncomment if you want to run Post Calls with Polly
                //var postResponse = client.PostAsyncPolly();

                Console.ReadKey();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
              
            }
          
        }
       
    }
}
