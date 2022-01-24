using IHttpClientFactory使用.IService;
using IHttpClientFactory使用.Model;
using IHttpClientFactory使用.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace IHttpClientFactory使用.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestHttpClientFactortController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly GitHubService _gitHubService;
        //private readonly IHelloClient _helloClient;

        public bool GetBranchesError { get; private set; }

        public IEnumerable<GitHubBranch> Branches { get; private set; }
        public TestHttpClientFactortController(IHttpClientFactory httpClientFactory, GitHubService  gitHubService /*IHelloClient helloClient*/)
        {
            _httpClientFactory = httpClientFactory;
            _gitHubService = gitHubService;
            //_helloClient = helloClient;
        }


        /// <summary>
        /// IHttpClientFactory 基础使用方法一
        /// </summary>
        /// <returns></returns>
        [HttpGet("TestHttpClientFactortOne")]
        public async Task<IActionResult> TestHttpClientFactoryOneAsync()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "https://api.github.com/repos/dotnet/AspNetCore.Docs/branches");
            request.Headers.Add("Accept", "application/vnd.github.v3+json");
            request.Headers.Add("User-Agent", "HttpClientFactory-Sample");

            var client = _httpClientFactory.CreateClient();

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();

                var responseStr = await response.Content.ReadAsStringAsync();

                //Branches = await JsonSerializer.DeserializeAsync<IEnumerable<GitHubBranch>>(responseStream);
            }
            else
            {
                GetBranchesError = true;
                //Branches = Array.Empty<GitHubBranch>();
            }
            return Ok("ok");
        }


        /// <summary>
        /// IHttpClientFactory 基础使用方法二
        /// </summary>
        /// <returns></returns>
        [HttpGet("TestHttpClientFactoryTwo")]
        public async Task<IActionResult> TestHttpClientFactoryTwoAsync()
        {
            var httpMessage = new HttpRequestMessage(HttpMethod.Get, "repos/dotnet/AspNetCore.Docs/pulls");
            var client = _httpClientFactory.CreateClient("github");

            var response = await client.SendAsync(httpMessage);

            if (response.IsSuccessStatusCode)
            {
                var responseStr = await response.Content.ReadAsStringAsync();
            }
            else
            {

            }
            return Ok("ok");
        }

        /// <summary>
        /// IHttpClientFactory 基础使用方法三
        /// </summary>
        /// <returns></returns>
        [HttpGet("TestHttpClientFactoryThree")]
        public async Task<IActionResult> TestHttpClientFactoryThreeAsync()
        {
            var reponstStr = await _gitHubService.GetAspNetDocsIssues();
            return Ok("ok");
        }


        /// <summary>
        /// IHttpClientFactory 基础使用方法四
        /// </summary>
        /// <returns></returns>
        [HttpGet("TestHttpClientFactoryFour")]
        public async Task<ActionResult<Reply>> TestHttpClientFactoryFourAsync()
        {
            //return await _helloClient.GetMessageAsync();

            return new Reply();
        }


        /// <summary>
        /// IHttpClientFactory 基础使用方法四
        /// </summary>
        /// <returns></returns>
        [HttpGet("helloworld")]
        public async Task<ActionResult<Reply>> helloworld()
        {
            //return await _helloClient.GetMessageAsync();
            return new Reply();
        }



        /// <summary>
        /// IHttpClientFactory 基础使用方法四
        /// </summary>
        /// <returns></returns>
        [HttpGet("TestPolicy")]
        public async Task<IActionResult> TestPolicyAsync()
        {
            var client = _httpClientFactory.CreateClient("myhttpclienttest");
            for (int i = 0; i < 6; i++)
            {
                var aa = await client.GetAsync("http://192.168.6.101:8069/Test/addAdsLogType?RetryCount=1&ThreadSleep=200");
                if (aa.IsSuccessStatusCode)
                {
                    var aaa = await aa.Content.ReadAsStringAsync();
                }
            }
            return Ok("ok");
        }
    }

    public class GitHubBranch
    {

    }
}
