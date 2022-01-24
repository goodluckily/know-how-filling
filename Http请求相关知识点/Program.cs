using Http请求相关知识点.Helper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Http请求相关知识点
{
    class Program
    {
        static async Task Main(string[] args)
        {

            var socketsHandler = new SocketsHttpHandler()
            {
                //SSl 证书的问题
                SslOptions = new System.Net.Security.SslClientAuthenticationOptions()
                {
                    RemoteCertificateValidationCallback = (sender, cer, chain, err) => true
                },
                //手动设置 Cookies
                UseCookies = false
            };

            #region Post 设置Cookie PluLoad 请求得到数据


            //var httpPostClient = new HttpClient(socketsHandler);

            ////手动设置 Cookies
            //var message = new HttpRequestMessage(HttpMethod.Post, new Uri("http://192.168.6.101:8067/api/Profile/GetProfilesByPage"));


            //message.Headers.Add("Cookie", "authtoken=eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyZWZyZXNoVGltZSI6IjIwMjEvMTIvMTMgMjM6MzQ6MTQiLCJleHBpcmVUaW1lIjoiMjAyMS8xMi8xMyAyMzoyNDoxNCIsImVtYWlsIjoiMSIsInVuaXF1ZV9uYW1lIjoiYWRtaW4iLCJwcmltYXJ5c2lkIjoiNDAyNjUzMTg0IiwibmJmIjoxNjM5MzY1ODU0LCJleHAiOjE2Mzk0MDkwNTQsImlhdCI6MTYzOTM2NTg1NH0.v5ZLHqM6_x6_QduUa-ixQ7A7YyMjY-dtVq91EIyuCsk");

            ////设置请求内容
            //message.Content = new StringContent("{\"pageIndex\":1,\"current\":1,\"pageSize\":10,\"sort\":\"pfSpend\",\"isAsc\":false}", Encoding.UTF8, "application/json");

            //var result = await httpPostClient.SendAsync(message);

            //var responseString = await result.Content.ReadAsStringAsync();//返回结果

            #endregion


            #region 文件 流等


            //1 MultipartFormDataContent =》multipart / form - data

            //2 FormUrlEncodedContent =》application / x - www - form - urlencoded

            //3 StringContent =》application / json等

            //4 StreamContent =》binary


            var path = @"C:\Users\cy\Pictures\Camera Roll\北京.jpg";
            var bytes = File.ReadAllBytes(path);

            using (var fileCilent = new HttpClient(socketsHandler))
            {

                //var requestMessage = new HttpRequestMessage();
                //requestMessage.Method = HttpMethod.Post;
                //requestMessage.RequestUri = new Uri("http://192.168.6.189:3020/api/upImgFile");
                //requestMessage.Content = new StringContent("");


                //requestMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");


                //var response = await fileCilent.SendAsync(requestMessage);




                #region 中文fileName 尚未解决


                //fileCilent.DefaultRequestHeaders.Add("Accept", "application/json");

                //var thisFName = Path.GetFileNameWithoutExtension(path);//中文设置错误的问题 不知道为什么?

                //var fName = Guid.NewGuid().ToString("n"); //
                //var fType = Path.GetExtension(path);

                //var fileName = fName + fType;
                //var fileName1 = thisFName + fType;


                //ByteArrayContent byteArrayContent = new ByteArrayContent(bytes);//填充图片文件二进制字节  //Add("Content-Type", "application/json; charset=utf-8")

                //MultipartFormDataContent multipartFormDataContent = new MultipartFormDataContent();
                //multipartFormDataContent.Add(new StringContent("61b3284cad83f82d479d73ff"), "_id");
                //multipartFormDataContent.Add(byteArrayContent, "files", fileName1);

                //var result222 = await fileCilent.PostAsync("http://192.168.6.189:3020/api/upImgFile", multipartFormDataContent);//post请求
                //var dfasfsa12f = await result222.Content.ReadAsStringAsync();

                #endregion
            }

            //var parm = new Dictionary<string, object>();
            //parm.Add("_id", "61b3284cad83f82d479d73ff");
            //parm.Add("files", bytes);
            //var aaa = HttpHelper.PostFile("http://192.168.6.189:3020/api/upImgFile", parm);


            #endregion

            Console.WriteLine("Ok");
        }


    }
}
