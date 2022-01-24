using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace IHttpClientFactory使用.Service
{
    public class GitHubService
    {
        private readonly HttpClient _client;

        public GitHubService(HttpClient client)
        {
            client.BaseAddress = new Uri("https://api.github.com/");
            client.DefaultRequestHeaders.Add("Accept","application/vnd.github.v3+json");
            client.DefaultRequestHeaders.Add("User-Agent","HttpClientFactory-Sample");
            _client = client;
        }

        public async Task<string> GetAspNetDocsIssues()
        {
            var asdfsa = await _client.GetAsync("/repos/aspnet/AspNetCore.Docs/issues?state=open&sort=created&direction=desc");
            if (asdfsa.IsSuccessStatusCode)
            {
                return await asdfsa.Content.ReadAsStringAsync();
            }
            return "";
        }
    }
}
