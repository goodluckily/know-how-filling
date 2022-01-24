using IHttpClientFactory使用.Model;
using System.Threading.Tasks;
using Refit;

namespace IHttpClientFactory使用.IService
{
    public interface IHelloClient
    {
        [Get("/helloworld")]
        Task<Reply> GetMessageAsync();
    }
}
