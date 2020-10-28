using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;


namespace CryptocurrencyTraderOnDeskTop
{



    class Program
    {

        static void Main(string[] args)
        {
            Console.WriteLine("開始");

            Task t = Get();
            Console.WriteLine("またない");
            Console.WriteLine("またない");
            Console.WriteLine("またない");
            t.Wait();
            Console.WriteLine("まったよ");



            Console.WriteLine("終了 ----------なにかキーを入力してください。----------");
            Console.ReadKey();
        }

        private static async Task waitm()
        {
            await Task.Delay(5000);
            Console.WriteLine("まったよ");
        }


        private static async Task<string> Get()
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync("https://api.zaif.jp/api/1/currencies/btc");
            var  con = await response.Content.ReadAsStringAsync();
            return con.ToString();
        }

    }
}
