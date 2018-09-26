using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DemoMultiThreads_extensions
{
    public static class ExtensionsDemo
    {
        public static async Task<string> GoGoogleIt(this HttpClient httpClient, string query)
        {
            var response = await httpClient?.GetAsync($"https://www.google.co.uk/search?q={ query }");
            return await response.Content.ReadAsStringAsync();
        }

        
    }


}
