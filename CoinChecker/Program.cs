using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using WebLib;
using System.Threading;
using System.Collections.Generic;
using System.Linq;

namespace TestConsoleApp
{
    public class Program
    {
        static readonly Func<string, string> URL_FOR = (string currency) => { return string.Format("https://api.gdax.com/products/{0}-USD/book?level=2", currency); }; 
        static void Main(string[] args)
        {
            Dictionary<string, string> prices = GetPricesBatch();
            dynamic ethPrices = JsonConvert.DeserializeObject(prices["ETH"]);
            dynamic ltcPrices = JsonConvert.DeserializeObject(prices["LTC"]);
            dynamic btcPrices = JsonConvert.DeserializeObject(prices["BTC"]);
            while(true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Price at " + DateTime.Now.ToString("hh:mm:ss"));
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("ETH buy: " + ethPrices.bids[0][0] + " sell: " + ethPrices.asks[0][0]);
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("LTC buy: " + ltcPrices.bids[0][0] + " sell: " + ltcPrices.asks[0][0]);
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("BTC buy: " + btcPrices.bids[0][0] + " sell: " + btcPrices.asks[0][0]);
                Thread.Sleep(30000);
            }
        }

        static Dictionary<string, string> GetPrices()
        {
            HttpLib restLib = new HttpLib();
            restLib.AddHeader("User-Agent", "GDAXBotter");
            Task<string> ethPricePromise = restLib.GetString(URL_FOR("ETH"));
            Task<string> ltcPricePromise = restLib.GetString(URL_FOR("LTC"));
            Task<string> btcPricePromise = restLib.GetString(URL_FOR("BTC"));
            ethPricePromise.Wait();
            ltcPricePromise.Wait();
            btcPricePromise.Wait();
            Dictionary<string, string> result = new Dictionary<string, string>();
            result["ETH"] = ethPricePromise.Result;
            result["BTC"] = btcPricePromise.Result;
            result["LTC"] = ltcPricePromise.Result;
            return result;
        }

        static Dictionary<string, string> GetPricesBatch()
        {
            HttpLib restLib = new HttpLib();
            restLib.AddHeader("User-Agent", "GDAXBotter");
            Task<IEnumerable<string>> prices = restLib.GetStringsBatch(URL_FOR("ETH"), URL_FOR("BTC"), URL_FOR("LTC"));
            prices.Wait();
            string[] priceArray = prices.Result.ToArray();
            Dictionary<string, string> result = new Dictionary<string, string>();
            result["ETH"] = priceArray[0];
            result["BTC"] = priceArray[1];
            result["LTC"] = priceArray[2];
            return result;
        }

    }
}
