using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebLib
{
    public class HttpLib
    {
        private HttpClient client = new HttpClient();

        private IDictionary<string, string> headers = new Dictionary<string, string>();

        public void AddHeader(string key, string value)
        {
            client.DefaultRequestHeaders.Add(key, value);
        }

        public async Task<string> GetString(string url)
        {
            // Console.WriteLine("Fetching URL " + url);
            return await (await client.GetAsync(url)).Content.ReadAsStringAsync();
        }

        public async Task<IEnumerable<string>> GetStringsBatch(params string[] urls)
        {
            IEnumerable<HttpResponseMessage> responseMessageTasks = await Task.WhenAll(urls.Select(url => client.GetAsync(url)));
            IEnumerable<string> responseStrings = await Task.WhenAll(responseMessageTasks.Select(msg => msg.Content.ReadAsStringAsync()));
            return responseStrings;
        }
    }
}
