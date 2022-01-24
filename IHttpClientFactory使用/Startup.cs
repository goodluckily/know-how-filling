using IHttpClientFactory使用.IService;
using IHttpClientFactory使用.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Refit;
using System.Net.Http;
using Polly;
using System.Threading;
using Microsoft.Extensions.Caching.Memory;
using Polly.CircuitBreaker;
using Polly.Bulkhead;
using System.Net;

namespace IHttpClientFactory使用
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {


            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "IHttpClientFactory使用", Version = "v1" });
            });

            //1.HttpClientFactory 基础使用方法一
            services.AddHttpClient();

            //2.HttpClientFactory 基础使用方法二 (依据string Key)
            services.AddHttpClient("github", option =>
            {
                option.BaseAddress = new Uri("https://api.github.com/");
                option.Timeout = TimeSpan.FromMinutes(2);
                option.DefaultRequestHeaders.Add("Accept", "application/vnd.github.v3+json");
                option.DefaultRequestHeaders.Add("User-Agent", "HttpClientFactory-Sample");

            });

            //3.HttpClientFactory 基础使用方法二 IntelliSense(智能感知)
            services.AddHttpClient<GitHubService>();

            //3.1 也可以 统一在这里设置
            services.AddHttpClient<GitHubService>(option =>
            {
                option.BaseAddress = new Uri("https://api.github.com/");
                option.DefaultRequestHeaders.Add("Accept", "application/vnd.github.v3+json");
                option.DefaultRequestHeaders.Add("User-Agent", "HttpClientFactory-Sample");
            });

            //4.HttpClientFactory 生成的客户端 使用 Refit!!
            //services.AddHttpClient("hello", option =>
            // {
            //     option.BaseAddress = new Uri("http://localhost:5000/api/TestHttpClientFactoryFour");
            // }).AddTypedClient(c => Refit.RestService.For<IHelloClient>(c))
            //.AddHttpMessageHandler<ValidateHeaderHandler>();


            //与第三方库 Polly 集成 是适用于 .NET 的全面恢复和临时故障处理库。 开发人员通过它可以表达策略，
            //例如以流畅且线程安全的方式处理重试、断路器、超时、Bulkhead 隔离和回退

            //策略一
            var timeout = Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(10));
            var longtimeout = Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(60));
            services.AddHttpClient("HttpClientPolicy").AddPolicyHandler(request =>
                //运行一些代码以根据请求选择策略
                request.Method == HttpMethod.Get ? timeout : longtimeout
            );


            //添加多个 Polly 处理程序
            #region 11
            //services.AddHttpClient("multiplepolicies", client =>
            //     {
            //         client.BaseAddress = new Uri("http://192.168.6.101:8069/");
            //     })
            //        //1.添加重试策略。 若请求失败，最多可重试三次
            //        .AddTransientHttpErrorPolicy(x =>
            //        {
            //            return x.RetryAsync(3, (res, index) =>
            //            {
            //                Console.WriteLine($"执行错误，异常行为：{res?.Result}");
            //                Console.WriteLine($"第{index}次重试");
            //            });
            //        })
            //        //2.调用添加断路器策略。 如果尝试连续失败了 5 次，则会阻止后续外部请求 30 秒。 断路器策略处于监控状态。 通过此客户端进行的所有调用都共享同样的线路状态
            //        .AddTransientHttpErrorPolicy(x => x.CircuitBreakerAsync(5, TimeSpan.FromSeconds(30)));
            #endregion

            #region 22
            var RetryPolicy2 = Policy.HandleResult<HttpResponseMessage>(message =>
                {
                    var contextC = message.Content.ReadAsStringAsync().Result;
                    return (message.StatusCode == HttpStatusCode.InternalServerError || message.StatusCode == HttpStatusCode.RequestTimeout || message.StatusCode == HttpStatusCode.OK || message.StatusCode == HttpStatusCode.NotFound);
                }).RetryAsync(3, (res, Index) =>
                {
                    Console.WriteLine($"执行错误，异常行为：{res?.Result}");
                    Console.WriteLine($"第{Index}次重试");
                });

            services.AddHttpClient("myhttpclienttest", client =>
            {
                client.BaseAddress = new Uri("http://192.168.6.101:8069/");
            }).AddPolicyHandler(RetryPolicy2);
            #endregion

            #region 策略
            //程序熔断 策略
            //var policyExecues = Policy<object>.Handle<Exception>().CircuitBreaker(3, TimeSpan.FromSeconds(3));
            //for (int i = 0; i < 10; i++)
            //{
            //    try
            //    {
            //        policyExecues.Execute(() =>
            //        {
            //            throw new Exception("异常");
            //        });
            //    }
            //    catch (Exception ex)
            //    {
            //        Console.WriteLine(ex.Message);
            //    }

            //    Thread.Sleep(1000);
            //}


            //程序超时 策略
            //try
            //{
            //    var timeOutPolicy = Policy.Timeout(2, Polly.Timeout.TimeoutStrategy.Pessimistic);
            //    timeOutPolicy.Execute(()=> 
            //    {
            //        Console.WriteLine("--------------开始-------------------");
            //        Thread.Sleep(3000);
            //    });
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //}


            //4、舱壁隔离
            //实际上是一个限流的功能，为服务定义最大的流量和队列，控制请求量过大而被压崩。
            //方法：Bulkhead
            //var BulkheadPolicy = Policy.Bulkhead(3);
            //for (int i = 0; i < 6; i++)
            //{
            //    Task.Factory.StartNew(() =>
            //    {
            //        try
            //        {
            //            BulkheadPolicy.Execute(() =>
            //            {
            //                Console.WriteLine($"--------------开始{i}-------------------");
            //            });
            //        }
            //        catch (Exception ex)
            //        {
            //            //The bulkhead semaphore and queue are full and execution was rejected
            //            Console.WriteLine(ex.Message);
            //        }
            //    });
            //}

            //5、缓存策略
            //类似于AOP的机制为应用嵌入缓存。当缓存命中快速响应换存，而不是每次都请求服务。
            //方法：Cache
            //Policy.Cache 可以指定 过期时间,绝对过期时间,滑动过期时间
            //MemoryCache memoryCache = null;
            //var cachePoliy = Policy.Cache((Polly.Caching.ISyncCacheProvider)memoryCache,TimeSpan.FromMinutes(5));


            //6、失败降级 也叫  回退
            //当我的服务不可用的时候，我们响应一个更加友好的结果，而不是报错。
            //方法：FallBack
            //var fallbackPoily = Policy.Handle<Exception>().Fallback(() =>
            //{
            //Console.WriteLine("你的程序出错了,我是替代程序");
            //});

            //fallbackPoily.Execute(() =>
            //{
            //Console.WriteLine("--------------开始-------------------");
            //throw new ArgumentNullException();
            //});


            //7、组合策略
            //可以将上面的策略组合在一起
            //我们将超时策略（Timeout）加上回退(FallBack）策略组合使用

            ////报错策略
            //var policyEx = Policy.Handle<Exception>().Fallback(() =>
            //{
            //    Console.WriteLine("你的程序超时了,我是替代程序");
            //});
            ////超时策略
            //var policyExAndTimeOut = policyEx.Wrap(Policy.Timeout(3, Polly.Timeout.TimeoutStrategy.Pessimistic));
            ////执行
            //policyExAndTimeOut.Execute(() =>
            //{
            //    Thread.Sleep(4000);
            //});

            #endregion

            //总结

            //Polly使用步骤
            //• 定义要处理的异常类型或返回值
            //• 定义要处理动作（重试、熔断、降级响应等）
            //• 使用定义的策略来执行代码

            //失败重试场景
            //1、服务失败是暂时的，可以自愈的。
            //比如说网络闪断
            //服务的部分结点不可用
            //2、服务是幂等的，重复调用不会有副作用

            //最佳实践
            //1、设置失败重试次数
            //2、设置带有步长策略的失败等待时间
            //3、设置降级响应
            //重试次数达到某个值，就应该让服务降级
            //4、设置断路器
            //重试很多次还是不可用，就要短路了。这就是熔断


            //HttpClient 定义策略


            #region 定义策略

            ////服务中注册策略1
            //var reg = services.AddPolicyRegistry();
            ////捕捉到异常HttpResponseMessage 且 响应码是BadRequest, 重试3次
            //reg.Add("Retry3", Policy.HandleResult<HttpResponseMessage>(message => { return message.StatusCode == System.Net.HttpStatusCode.BadRequest; }).RetryAsync(3));


            ////服务中注册策略2
            //var reg2 = services.AddPolicyRegistry();
            ////捕捉到异常HttpResponseMessage 且  NotFound  ，直接返回ok
            //reg2.Add("RetryBack", Policy.HandleResult<HttpResponseMessage>(message => { return message.StatusCode == System.Net.HttpStatusCode.NotFound; }).Fallback(new HttpResponseMessage() { StatusCode = System.Net.HttpStatusCode.OK }));

            #endregion

            #region 使用策略

            //services.AddHttpClient("client1").AddPolicyHandlerFromRegistry("Retry3");

            //services.AddHttpClient("client2").AddPolicyHandlerFromRegistry("RetryBack");


            ////动态定义策略机制
            //services.AddHttpClient("client3").AddPolicyHandlerFromRegistry((reg, mes) =>
            //{
            //    //这行代码意思如果是HttpGet 就使用Retry3策略，  如果不是就熔断不做任何处理
            //    return mes.Method == HttpMethod.Get ? reg.Get<IAsyncPolicy<HttpResponseMessage>>("Retry3") : Policy.NoOpAsync<HttpResponseMessage>();
            //});



            #endregion

            #region 策略组合

            //var breakPolicy = Policy<HttpResponseMessage>.Handle<HttpRequestException>().AdvancedCircuitBreakerAsync(
            //   failureThreshold: 0.8,
            //   samplingDuration: TimeSpan.FromSeconds(10),
            //   minimumThroughput: 100,
            //   durationOfBreak: TimeSpan.FromSeconds(20),
            //   onBreak: (r, t) => { },
            //   onReset: () => { },
            //   onHalfOpen: () => { });

            //var message = new HttpResponseMessage()
            //{
            //    Content = new StringContent("{}")
            //};
            //var fallback = Policy<HttpResponseMessage>.Handle<BrokenCircuitException>().FallbackAsync(message);
            //var retry = Policy<HttpResponseMessage>.Handle<Exception>().WaitAndRetryAsync(3, i => TimeSpan.FromSeconds(1));
            //var fallbackBreak = Policy.WrapAsync(fallback, retry, breakPolicy);
            //services.AddHttpClient("httpv3").AddPolicyHandler(fallbackBreak);


            //var bulk = Policy.BulkheadAsync<HttpResponseMessage>(
            //    //最大并发 处理 数量
            //    maxParallelization: 30,
            //    //如果不定义这个参数， 最大请求数超量，就会报错。定义了这个参数其他请求就会排队，如果排队大于20就在报错 
            //    maxQueuingActions: 20,
            //    //当我们请求被拒绝，被限流了  做的处理
            //    onBulkheadRejectedAsync: contxt => Task.CompletedTask
            //    );

            //var message2 = new HttpResponseMessage()
            //{
            //    Content = new StringContent("{}")
            //};
            //var fallback2 = Policy<HttpResponseMessage>.Handle<BulkheadRejectedException>().FallbackAsync(message);
            //var fallbackbulk = Policy.WrapAsync(fallback2, bulk);
            //services.AddHttpClient("httpv4").AddPolicyHandler(fallbackbulk);


            #endregion


            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "IHttpClientFactory使用 v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
