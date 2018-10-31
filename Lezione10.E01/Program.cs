using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Lezione12.E01
{
    class Program
    {
        static DateTime Start;
        static void Main(string[] args)
        {
            //Console.Write("Premi invio per partire");
            //Console.ReadLine();
            //Start = DateTime.Now;
            //MainAsync().ContinueWith((t) => {
            //    Console.WriteLine("Metodi Async completati");
            //});
            //Console.WriteLine("Metodo Main completato");
            //Console.ReadLine();
            GetDemo().ContinueWith((t) => {
                Console.WriteLine(t.Result);
            });
        }

        public static async Task GetDemoAsync()
        {
            string t = await GetDemo();
            Console.WriteLine(t);
        }

        public static Task<string> GetDemo()
        {

            HttpClient client = new HttpClient();

            Task<string> callTask = client.GetStringAsync("http://msdn.microsoft.com");

            return callTask;
        }


        private static async Task MainAsync()
        {
            Task<int> s = Sum(2, 3);
            Task e = EndWork();
            int res = await s;
            await e;
            Console.WriteLine($"Il risultato di 2 + 3 è {res} - {(DateTime.Now - Start).Seconds} secondi");
        }

        static async Task EndWork()
        {
            await Task.Run(async () => {
                await Task.Delay(1000);
                Console.WriteLine($"Lavoro Eseguito! {(DateTime.Now - Start).Seconds} secondi");
            });
        }

        static async Task<int> Sum(int x, int y)
        {
            return await Task.Run(async () => {
                await Task.Delay(5000);
                return x + y;
            });
        }
    }
}
