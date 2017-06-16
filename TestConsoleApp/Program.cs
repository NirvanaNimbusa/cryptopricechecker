using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace TestConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(new WebScraper().Scrape("http://fast-tundra-4616.herokuapp.com"));
            Console.ReadLine();
        }
    }

    class WebScraper
    {
        public String Scrape(string url)
        {
            WebClient client = new WebClient();
            return client.DownloadString(url);
        }
    }

}
