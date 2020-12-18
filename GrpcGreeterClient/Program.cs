using System;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Net.Client;

namespace GrpcGreeterClient
{
    class Program
    {
        static async System.Threading.Tasks.Task Main(string[] args)
        {
            Console.WriteLine("Connecting");
            using var channel = GrpcChannel.ForAddress("http://localhost:5000");
            var client =  new Greeter.GreeterClient(channel);
            Console.WriteLine("Connected");

            // client demo 1: say hello
            // var reply = await client.SayHelloAsync( new HelloRequest { Name = "Mike" });
            // Console.WriteLine("Greeting: " + reply.Message);

            // client demo 2: server stream
            // var call = client.GetStockPrices(new GetPriceRequest() { StockId = "2330" });
            // while (await call.ResponseStream.MoveNext(new System.Threading.CancellationToken()))
            // {
            //     Console.WriteLine("Greeting: " + call.ResponseStream.Current.Price);
            //     // "Greeting: Hello World" is written multiple times
            // }

            // client demo 3: client stream
            // var stockId = "2330";
            // var call = client.UpdateStockPrices();
            // for (var i = 0; i < 5; i++)
            // {
            //     Console.WriteLine($"Update Stock: {stockId}; Price: {100 + i}");
            //     await call.RequestStream.WriteAsync(new UpdatePriceRequest
            //     { 
            //         StockId = stockId,
            //         Price = 100 + i
            //     });
            //     await Task.Delay(TimeSpan.FromSeconds(1));
            // }
            // await call.RequestStream.CompleteAsync();

            // var response = await call;
            // Console.WriteLine($"Status: {response.Success}");

            // client demo 4: bi-directional streaming
            // var call = client.Echo();

            // // read from server
            // var readTask = Task.Run(async () =>
            // {
            //     await foreach (var response in call.ResponseStream.ReadAllAsync())
            //     {
            //         Console.WriteLine($"echo: {response.Message}");
            //     }
            // });

            // // send to server
            // while (true)
            // {
            //     var result = Console.ReadLine();
            //     if (string.IsNullOrEmpty(result))
            //     {
            //         break;
            //     }

            //     await call.RequestStream.WriteAsync(new EchoRequest() { Message = result });
            // }

            // Console.WriteLine("Complete");
            // await call.RequestStream.CompleteAsync();
            // await readTask;
        }
    }
}
