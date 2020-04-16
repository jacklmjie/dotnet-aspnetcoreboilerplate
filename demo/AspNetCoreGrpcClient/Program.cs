﻿using AspNetCoreGrpcClient.Filter;
using AspNetCoreGrpcService;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Grpc.Net.Client;
using IdentityModel.Client;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace GrpcGreeterClient
{
    /// <summary>
    /// Install-Package Grpc.Net.Client
    /// Install-Package Google.Protobuf
    /// Install-Package Grpc.Tools
    /// </summary>
    class Program
    {
        static async Task Main(string[] args)
        {
            // The port number(5001) must match the port of the gRPC server.
            var channel = GrpcChannel.ForAddress("https://localhost:5001");
            //var client = new Greeter.GreeterClient(channel);
            //var reply = await client.SayHelloAsync(
            //                  new HelloRequest { Name = "hello lmj" });
            //Console.WriteLine("Greeter服务 返回数据: " + reply.Message);

            //await LuCatTest();
            //await ClientInterceptorTest();

            await IdentityTest();

            Console.ReadKey();
        }

        static async Task IdentityTest()
        {
            // discover endpoints from metadata
            var client = new HttpClient();

            var disco = await client.GetDiscoveryDocumentAsync("http://localhost:5002");
            if (disco.IsError)
            {
                Console.WriteLine(disco.Error);
                return;
            }

            // request token
            var tokenResponse = await client.RequestPasswordTokenAsync(new PasswordTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = "ro.client",
                ClientSecret = "secret",

                UserName = "alice",
                Password = "password",
                Scope = "grpc1"
            });

            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                return;
            }

            Console.WriteLine(tokenResponse.Json);
            Console.WriteLine("\n\n");

            var headers = new Metadata { { "Authorization", $"Bearer {tokenResponse.Json["access_token"]}" } };
            var catClient = new LuCat.LuCatClient(GrpcChannel.ForAddress("https://localhost:5001"));
            var catReply = await catClient.SuckingCatAsync(new Empty(), headers);
            Console.WriteLine("调用授权后的撸猫服务：" + catReply.Message);
        }

        static async Task ClientInterceptorTest()
        {
            //客户端拦截器
            var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var invoker = channel.Intercept(new ClientLoggerInterceptor());
            var catClient = new LuCat.LuCatClient(invoker);
            var catReply = await catClient.SuckingCatAsync(new Empty());
            Console.WriteLine("调用撸猫服务：" + catReply.Message);
        }

        static async Task LuCatTest()
        {
            //grpc双向流
            var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var catClient = new LuCat.LuCatClient(channel);
            //获取猫总数
            var catCount = await catClient.CountAsync(new Empty());
            Console.WriteLine($"一共{catCount.Count}只猫。");
            var rand = new Random(DateTime.Now.Millisecond);

            var cts = new CancellationTokenSource();
            //指定在2.5s后进行取消操作
            cts.CancelAfter(TimeSpan.FromSeconds(2.5));
            var bathCat = catClient.BathTheCat(cancellationToken: cts.Token);
            //定义接收吸猫响应逻辑
            var bathCatRespTask = Task.Run(async () =>
            {
                try
                {
                    await foreach (var resp in bathCat.ResponseStream.ReadAllAsync())
                    {
                        Console.WriteLine(resp.Message);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                //catch (RpcException ex) when (ex.StatusCode == StatusCode.Cancelled)
                //{
                //    Console.WriteLine("Stream cancelled.");
                //}
            });
            //随机给10个猫洗澡
            for (int i = 0; i < 10; i++)
            {
                await bathCat.RequestStream.WriteAsync(new BathTheCatReq() { Id = rand.Next(0, catCount.Count) });
            }
            //发送完毕
            await bathCat.RequestStream.CompleteAsync();
            Console.WriteLine("客户端已发送完10个需要洗澡的猫id");
            Console.WriteLine("接收洗澡结果：");
            //开始接收响应
            await bathCatRespTask;

            Console.WriteLine("洗澡完毕");
        }
    }
}
