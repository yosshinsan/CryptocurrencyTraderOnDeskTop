using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Security.Cryptography;

namespace CryptocurrencyTraderOnDeskTop
{



    class Program
    {

        static void Main(string[] args)
        {
            //Console.WriteLine("開始");

            //Task t = Get();
            //Console.WriteLine("またない");
            //Console.WriteLine("またない");
            //Console.WriteLine("またない");
            //t.Wait();
            //Console.WriteLine("まったよ");



            //Console.WriteLine("終了 ----------なにかキーを入力してください。----------");
            //Console.ReadKey();

            HttpClient http = new HttpClient();
            string apiKey = "";
            string secret = "";
            string uri = "";

            Task <string> getVal = Send(http,apiKey,secret,uri);

        }

        private static async Task<string> Send(HttpClient http, string apiKey, string secret, string uri)
        {
            //if (parameters == null)
            //    parameters = new Dictionary<string, string>();

            //// パラメータ文字列を作成
            //var content = new FormUrlEncodedContent(parameters);
            //string param = await content.ReadAsStringAsync();

            // nonceにunixtimeを用いる
            string nonce = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();

            //// POSTするメッセージを作成
            //var uri = new Uri(http.BaseAddress, path);
            //string message = nonce + uri.ToString() + param;

            string message = nonce + uri;

            // メッセージをHMACSHA256で署名
            byte[] hash = new HMACSHA256(Encoding.UTF8.GetBytes(secret)).ComputeHash(Encoding.UTF8.GetBytes(message));
            string sign = BitConverter.ToString(hash).ToLower().Replace("-", "");//バイト配列をを16進文字列へ

            // HTTPヘッダをセット
            http.DefaultRequestHeaders.Clear();
            http.DefaultRequestHeaders.Add("ACCESS-KEY", apiKey);
            http.DefaultRequestHeaders.Add("ACCESS-NONCE", nonce);
            http.DefaultRequestHeaders.Add("ACCESS-SIGNATURE", sign);

            // 送信
            HttpResponseMessage res;
            //if (method == "POST")
            //{
            //    res = await http.PostAsync(path, content);
            //}
            //else if (method == "GET")
            //{
            //    res = await http.GetAsync(path);
            //}
            //else
            //{
            //    throw new ArgumentException("method は POST か GET を指定してください。", method);
            //}

            res = await http.GetAsync(uri);


            //返答内容を取得
            string text = await res.Content.ReadAsStringAsync();

            //通信上の失敗
            if (!res.IsSuccessStatusCode)
                return "";

            return text;
        }



        private static async Task waitm()
        {
            await Task.Delay(5000);
            Console.WriteLine("まったよ");
        }


        private static async Task<string> Get()
        {
            //https://qiita.com/rawr/items/f78a3830d894042f891b

            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync("https://api.zaif.jp/api/1/currencies/btc");
            var  con = await response.Content.ReadAsStringAsync();
            return con.ToString();
        }

        /// <summary>coincheck取引所のAPIを実行します。
        /// </summary>
        /// <param name="http">取引所と通信する HttpClient。</param>
        /// <param name="path">APIの通信URL（取引所サイトからの相対）。</param>
        /// <param name="apiKey">APIキー。</param>
        /// <param name="secret">秘密キー。</param>
        /// <param name="method">APIのメソッド名。</param>
        /// <param name="parameters">APIのパラメータのリスト（Key:パラメータ名, Value:パラメータの値）。</param>
        /// <returns>レスポンスとして返されるJSON形式の文字列。</returns>
        internal async Task<string> Send(HttpClient http, Uri path, string apiKey, string secret, string method, Dictionary<string, string> parameters = null)
        {
            if (parameters == null)
                parameters = new Dictionary<string, string>();

            // パラメータ文字列を作成
            var content = new FormUrlEncodedContent(parameters);
            string param = await content.ReadAsStringAsync();

            // nonceにunixtimeを用いる
            string nonce = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();

            // POSTするメッセージを作成
            var uri = new Uri(http.BaseAddress, path);
            string message = nonce + uri.ToString() + param;

            // メッセージをHMACSHA256で署名
            byte[] hash = new HMACSHA256(Encoding.UTF8.GetBytes(secret)).ComputeHash(Encoding.UTF8.GetBytes(message));
            string sign = BitConverter.ToString(hash).ToLower().Replace("-", "");//バイト配列をを16進文字列へ

            // HTTPヘッダをセット
            http.DefaultRequestHeaders.Clear();
            http.DefaultRequestHeaders.Add("ACCESS-KEY", apiKey);
            http.DefaultRequestHeaders.Add("ACCESS-NONCE", nonce);
            http.DefaultRequestHeaders.Add("ACCESS-SIGNATURE", sign);

            // 送信
            HttpResponseMessage res;
            if (method == "POST")
            {
                res = await http.PostAsync(path, content);
            }
            else if (method == "GET")
            {
                res = await http.GetAsync(path);
            }
            else
            {
                throw new ArgumentException("method は POST か GET を指定してください。", method);
            }

            //返答内容を取得
            string text = await res.Content.ReadAsStringAsync();

            //通信上の失敗
            if (!res.IsSuccessStatusCode)
                return "";

            return text;
        }

    }
}
